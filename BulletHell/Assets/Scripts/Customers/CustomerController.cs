using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public static event Action<CustomerController> OnCustomerFinished;
    public static event Action<GameObject> OnReleaseSpot;
    public static event Action<Transform, float> OnCustomerThrowProjectil;

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
    private RangeFloat _patienceRange;

    [Header("Projectiles")]
    [SerializeField]
    private RangeFloat _shootRateRange;

    [SerializeField]
    private RangeFloat _projectileForceRange;

    // Transform
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private CustomerState _currentState = CustomerState.Spawned;
    // Spots
    private Transform _spotToWait;
    // Patience
    private float _patienceSeconds = 0;
    private float _currentPatienceTime = 0;
    // Food
    private ThroweableFood _desiredFood;
    // WayPoints
    private List<Transform> _waypoints = new();
    private Transform _currentWayPoint;
    private int _currentWapointIndex;
    private float _proximityWayPoint = 0.1f;
    // Projectiles
    private float _currentProjectileForce = 0;
    private float _currentShootRate = 0;

    public void SetUp(Transform player)
    {
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
    }

    #region Unity Callbacks
    private void FixedUpdate()
    {
        if (_currentState == CustomerState.Normal)
            return;

        _rb.linearVelocity = movementStats.MovementDir * movementStats.MovementForce;
    }

    private void Update()
    {
        HandlePatience(Time.deltaTime);
        HandleMoveDir();
    }

    private void OnEnable()
    {
        InventoryManager.OnInventoryUpdated += OnHighlightCustomer;
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryUpdated -= OnHighlightCustomer;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_currentState == CustomerState.Spawned && _spotToWait != null && other.transform == _spotToWait)
        {
            _currentState = CustomerState.Normal;
            transform.position = other.transform.position;
            _rb.linearVelocity *= 0;
            MasterAudioManager.Instance.PlayOneShot(CLIENT_SOUND.NEW_CLIENT, transform);
        }
    }

    #endregion

    private void HandleMoveDir()
    {
        if (_currentState != CustomerState.Unstable
            || _waypoints == null || _waypoints.Count == 0) return;

        if (Vector2.Distance(transform.position, _currentWayPoint.position) > _proximityWayPoint)
            return;

        _currentWapointIndex++;
        if (_currentWapointIndex >= _waypoints.Count)
            _currentWapointIndex = 0;

        _currentWayPoint = _waypoints[_currentWapointIndex];
        UpdateTargetPos(_currentWayPoint.position);
    }

    public void UpdateTargetPos(Vector2 target)
    {
        movementStats.MovementDir = (target - (Vector2)transform.position).normalized;
    }

    public void ResetCustomer(Vector2 startingPoint, ThroweableFood desiredFood, CustomerRenderer customerRenderer,
        Transform spotToWait, List<Transform> waypoints, FoodType currentInventoryType)
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
        _patienceSeconds = UnityEngine.Random.Range(_patienceRange.Min, _patienceRange.Max);
        // Select food
        _desiredFood = desiredFood;
        // Renderer
        _customerGraphics.SetUp(customerRenderer, desiredFood._sprite, _patienceSeconds);
        // Way Points
        _waypoints = waypoints;
        _currentWayPoint = waypoints[0];
        _currentWapointIndex = 0;
        // Projectiles
        _currentProjectileForce = UnityEngine.Random.Range(_projectileForceRange.Min, _projectileForceRange.Max);
        _currentShootRate = UnityEngine.Random.Range(_shootRateRange.Min, _shootRateRange.Max);
        // Highlight Customer
        OnHighlightCustomer(currentInventoryType);
    }

    private void HandlePatience(float time)
    {
        if (_currentState != CustomerState.Normal)
            return;

        _currentPatienceTime += time;
        _customerGraphics.UpdatePatienceBar(time);
        if (_currentPatienceTime < _patienceSeconds)
            return;

        // Update State
        _customerGraphics.TurnUnstable();
        _currentState = CustomerState.Unstable;
        // Update Movement
        UpdateTargetPos(_currentWayPoint.position);
        // Invoke Callbacks
        OnReleaseSpot?.Invoke(_spotToWait.gameObject);
        _spotToWait = null;
        InvokeRepeating(nameof(ThrowProjectile), 0, _currentShootRate);
    }

    private void ThrowProjectile()
    {
        if (!_currentState.Equals(CustomerState.Unstable))
            return;

        OnCustomerThrowProjectil?.Invoke(transform, _currentProjectileForce);
    }

    public void DeliverFood(FoodType foodType)
    {
        if (_desiredFood.FoodType.Equals(foodType))
        {
            if (_spotToWait)
                OnReleaseSpot?.Invoke(_spotToWait.gameObject);
            TutorialManager.OnTutorialUpdate?.Invoke(TUTORIAL.FEED_CUSTOMER);
            MasterAudioManager.Instance.PlayOneShot(CLIENT_SOUND.CORRECT_DELIVERY, transform);
            StatisticsManager.OnPlayerDeliverFood?.Invoke(foodType);
            _currentState = CustomerState.Served;
            if (IsInvoking(nameof(ThrowProjectile)))
                CancelInvoke(nameof(ThrowProjectile));
            _customerGraphics.FadeOut(() => OnCustomerFinished?.Invoke(this));
            // Walk away
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            UpdateTargetPos(randomDir);
        }
        else if (_currentState.Equals(CustomerState.Normal))
        {
            HandlePatience(time: 1f);
            MasterAudioManager.Instance.PlayOneShot(CLIENT_SOUND.INCORRECT_DELIVERY, transform);
        }

    }

    private void OnHighlightCustomer(FoodType foodType)
    {
        if (_currentState == CustomerState.Served)
            return;

        _customerGraphics.OnHighlightCustomer(_desiredFood.FoodType.Equals(foodType));
    }
}
