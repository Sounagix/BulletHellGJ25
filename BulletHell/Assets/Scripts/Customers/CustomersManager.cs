using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomersManager : Manager
{
    [SerializeField]
    private CustomerPool _customerPool;

    [SerializeField]
    private Transform _customerStartingPoint;

    [SerializeField]
    private List<Transform> _customerPath;

    [SerializeField]
    private AnimationCurve spawnRateCurve;

    private float _currentTime;
    private float _currentSpawnRate;

    public override void Initialize()
    {
        _customerPool.CreateCustomerPool(_customerPath);
    }

    public override void Shutdown()
    {
    }

    private void Start()
    {
        Initialize();
    }


    private void OnEnable()
    {
        CustomerController.OnCustomerFinished += ReleaseCustomer;
    }

    private void OnDisable()
    {
        CustomerController.OnCustomerFinished -= ReleaseCustomer;
    }

    private void Update()
    {
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
        CustomerController customer = _customerPool.GetCustomerFromPool();
        if (!customer)
            return;

        customer.ResetCustomer(_customerStartingPoint.position);
        customer.UpdateMoveDir();
    }

    private void ReleaseCustomer(CustomerController customer) 
    {
        _customerPool.AddCustomerToPool(customer);
    }
}
