using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public static event Action<CustomerController> OnCustomerFinished;
    public static event Action<GameObject> OnCustomerUnstable;
    public static event Action<Transform> OnCustomerThrowProjectil;

    [Header("Physics")]
    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private MovementStats movementStats;

    [Header("Graphics")]
    [SerializeField]
    private CustomerGraphics _customerGraphics;

    [Header("Logic")]
    [SerializeField]
    private PatienceRange _patienceRange;

    [SerializeField]
    private float _shootRate;

    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private CustomerState _currentState = CustomerState.Spawned;
    private Transform _spotToWait;
    private float _patienceSeconds = 0;
    private float _currentPatienceTime = 0;
    private ThroweableFood _desiredFood;

    public void SetUp(Transform player)
    {
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (_currentState == CustomerState.Normal)
            return;

        _rb.linearVelocity = movementStats.MovementDir * movementStats.MovementForce;
    }

    private void HandleMovement()
    {
        //UpdateTargetPos(_player.position);
        //Vector2 diff = transform.position - _player.position;
        //float distanceFromPlayer = diff.sqrMagnitude;
    }

    public void UpdateTargetPos(Vector2 target)
    {
        movementStats.MovementDir = (target - (Vector2)transform.position).normalized;
    }

    public void ResetCustomer(Vector2 startingPoint, ThroweableFood desiredFood, CustomerRenderer customerRenderer, Transform spotToWait)
    {
        // State
        _currentState = CustomerState.Spawned;
        // Transform
        transform.position = startingPoint;
        transform.rotation = _originalRotation;
        transform.localScale = _originalScale;
        // Spot
        _spotToWait = spotToWait;
        // Patience
        _currentPatienceTime = 0;
        _patienceSeconds = UnityEngine.Random.Range(_patienceRange.MinPatience, _patienceRange.MaxPatience);
        // Select food
        _desiredFood = desiredFood;
        // Renderer
        _customerGraphics.SetUp(customerRenderer, desiredFood._sprite, _patienceSeconds);
    }

    private void Update()
    {
        HandlePatience();
    }

    private void HandlePatience()
    {
        if (_currentState != CustomerState.Normal)
            return;

        _currentPatienceTime += Time.deltaTime;
        _customerGraphics.UpdatePatienceBar(Time.deltaTime);
        if (_currentPatienceTime < _patienceSeconds)
            return;

        _customerGraphics.TurnUnstable();
        _currentState = CustomerState.Unstable;
        OnCustomerUnstable?.Invoke(_spotToWait.gameObject);
        InvokeRepeating(nameof(ThrowProjectile), 0, _shootRate);
    }

    private void ThrowProjectile()
    {
        if (!_currentState.Equals(CustomerState.Unstable))
            return;

        OnCustomerThrowProjectil?.Invoke(transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_currentState == CustomerState.Spawned && _spotToWait != null && other.transform == _spotToWait)
        {

            _currentState = CustomerState.Normal;
            transform.position = other.transform.position;
            _rb.linearVelocity *= 0;
        }
    }

    public void DeliverFood(FoodType foodType)
    {
        if (_desiredFood.FoodType.Equals(foodType))
        {
            _currentState = CustomerState.Served;
            if (IsInvoking(nameof(ThrowProjectile)))
                CancelInvoke(nameof(ThrowProjectile));
            _customerGraphics.FadeOut(() => OnCustomerFinished?.Invoke(this));
            // Walk away
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            UpdateTargetPos(randomDir);
        }
        else
        {
            // Aumentar niveles de inestabilidad?
        }
    }
}
