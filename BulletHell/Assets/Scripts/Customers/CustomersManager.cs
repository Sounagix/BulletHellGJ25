using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomersManager : Manager
{
    [Header("Customer Pool")]
    [SerializeField]
    private CustomerPool _customerPool;

    [SerializeField]
    private AnimationCurve spawnRateCurve;

    [SerializeField]
    private List<CustomerRenderer> _customerRenderers;

    [SerializeField]
    private List<Transform> _customerWayPoints;

    [Header("Customer Spots")]
    [SerializeField]
    private Transform _startingPoint;

    [SerializeField]
    private Grid _spotGrid;

    [SerializeField]
    private GameObject _customerSpotPrefab;

    [SerializeField]
    private int _spotsToCreate;

    private float _currentTime;
    private GameSceneManager _gameSceneManager;

    [SerializeField]
    private InteractablePool _weaponPool;
    public GameSceneManager GameSceneManager { set { _gameSceneManager = value; } }
    private Queue<GameObject> _freeSpots = new();

    private Transform _playerTr;
    public Transform PlayerTr { set { _playerTr = value; } }

    private ThroweableFood[] _foodData;
    public ThroweableFood[] FoodData {  set { _foodData = value; } }

    private List<Transform> _reverseCustomerWayPoints;
    private FoodType _currentInventoryType = FoodType.None;

    private int _numOfCustomersToServe = 0;

    public override void Initialize()
    {
        _customerPool.CreateCustomerPool(_playerTr);
        _reverseCustomerWayPoints = new(_customerWayPoints);
        _reverseCustomerWayPoints.Reverse();
        _numOfCustomersToServe = LevelSceneManager.Instance.GetCurrentLevel()._numOfCLientsToServe;
        InitializeCustomerSpots();
    }

    private void InitializeCustomerSpots()
    {
        for (int i = 0; i < _spotsToCreate; i++)
        {
            Vector3Int cellPosition = _spotGrid.WorldToCell(_spotGrid.transform.position) + new Vector3Int(i, 0, 0);
            Vector3 worldPosition = _spotGrid.GetCellCenterWorld(cellPosition);
            GameObject spot = Instantiate(_customerSpotPrefab, worldPosition, Quaternion.identity, _spotGrid.transform);
            _freeSpots.Enqueue(spot);
        }
    }

    public override void Shutdown()
    {
    }

    private void OnEnable()
    {
        CustomerController.OnCustomerFinished += ReleaseCustomer;
        CustomerController.OnCustomerUnstable += OnCustomerUnstable;
        InventoryManager.OnInventoryUpdated += OnInventoryUpdated;

    }

    private void OnDisable()
    {
        CustomerController.OnCustomerFinished -= ReleaseCustomer;
        CustomerController.OnCustomerUnstable -= OnCustomerUnstable;
        InventoryManager.OnInventoryUpdated -= OnInventoryUpdated;
    }

    private void Update()
    {
        if (!_gameSceneManager)
            return;

        _currentTime += Time.deltaTime;

        float elapsedTime = Time.timeSinceLevelLoad;
        float spawnRate = spawnRateCurve.Evaluate(elapsedTime);

        if (_currentTime < spawnRate)
            return;

        _currentTime = 0;
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        if (_freeSpots.Count == 0 && !LevelSceneManager.Instance.CanSpawnCustomer())
            return;

        CustomerController customer = _customerPool.GetFromPool();

        if (!customer)
            return;

        CustomerRenderer renderer = _customerRenderers[UnityEngine.Random.Range(0, _customerRenderers.Count)];
        GameObject spot = _freeSpots.Dequeue();


        ThroweableFood foodSO = _foodData[UnityEngine.Random.Range(0, LevelSceneManager.Instance.GetCurrentLevel()._maxFoodsToSpawn)];
        bool isReversed = UnityEngine.Random.value > 0.5f;

        customer.ResetCustomer(_startingPoint.position, foodSO, renderer, spot.transform, 
            isReversed ? _reverseCustomerWayPoints : _customerWayPoints, _currentInventoryType);
        customer.UpdateTargetPos(spot.transform.position);
    }

    private void ReleaseCustomer(CustomerController customer)
    {
        _customerPool.ReturnToPool(customer);
    }

    private void OnCustomerUnstable(GameObject freeSpot)
    {
        _freeSpots.Enqueue(freeSpot);
    }

    private void OnInventoryUpdated(FoodType foodType) 
    {
        _currentInventoryType = foodType;
    }
}
