using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public static event Action<CustomerController> OnCustomerFinished;
    public static event Action<GameObject> OnCustomerUnstable;

    [SerializeField]
    private TextMeshProUGUI _foodText;

    [SerializeField]
    private MovementStats movementStats;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private PatienceRange _patienceRange;

    [SerializeField]
    private float _rangeFromPlayer = 5f;

    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private FoodType _currentFoodType;
    private CustomerRenderer _currentCustomerRenderer;
    private CustomerState _currentState = CustomerState.Spawned;
    private Transform _spotToWait;
    private float _patienceSeconds = 0;
    private float _currentPatienceTime = 0;
    private Transform _player;

    public void SetUp(Transform player)
    {
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
        _player = player;
    }

    private void FixedUpdate()
    {
        if (!_player || _currentState == CustomerState.Normal)
            return;

        if(_currentState == CustomerState.Unstable)
        {
            UpdateTargetPos(_player.position);
            Vector2 diff = transform.position - _player.position;
            float distanceFromPlayer = diff.sqrMagnitude;

            if (distanceFromPlayer < _rangeFromPlayer)
            {
                _rb.linearVelocity *= 0;
                return;
            }
        }

        _rb.linearVelocity = movementStats.MovementDir * movementStats.MovementForce;
    }

    public void UpdateTargetPos(Vector2 target)
    {
        movementStats.MovementDir = (target - (Vector2)transform.position).normalized;
    }

    public void ResetCustomer(Vector2 startingPoint, CustomerRenderer customerRenderer, Transform spotToWait)
    {
        // State
        _currentState = CustomerState.Spawned;
        // Transform
        transform.position = startingPoint;
        transform.rotation = _originalRotation;
        transform.localScale = _originalScale;
        // Renderer
        _currentCustomerRenderer = customerRenderer;
        _renderer.sprite = _currentCustomerRenderer.NormalState;
        // Spot
        _spotToWait = spotToWait;
        // Patience
        _currentPatienceTime = 0;
        _patienceSeconds = UnityEngine.Random.Range(_patienceRange.MinPatience, _patienceRange.MaxPatience);
        // Select food
        SelectFood();
    }

    private void Update()
    {
        if (!_player)
            return;

        HandlePatience();
    }

    private void HandlePatience() 
    {
        if (_currentState != CustomerState.Normal)
            return;

        _currentPatienceTime += Time.deltaTime;
        if (_currentPatienceTime < _patienceSeconds)
            return;

        _renderer.sprite = _currentCustomerRenderer.UnstableState;
        _currentState = CustomerState.Unstable;
        OnCustomerUnstable?.Invoke(_spotToWait.gameObject);
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

    private void SelectFood()
    {
        if (!_foodText)
            return;

        FoodType[] values = (FoodType[])Enum.GetValues(typeof(FoodType));
        _currentFoodType = values[UnityEngine.Random.Range(0, values.Length)];

        _foodText.text = _currentFoodType.ToString();
    }
}
