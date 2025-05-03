using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CustomerPool : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField]
    private Transform _poolParent;

    [SerializeField]
    [Min(0)]
    public int _poolSize;

    [SerializeField]
    public CustomerController _customerPrefab;

    private Queue<CustomerController> _pool = new(); 

    public void CreateCustomerPool(List<Transform> customerPath) 
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CustomerController customer = Instantiate(_customerPrefab, _poolParent);
            customer.SetUp(customerPath);
            customer.gameObject.SetActive(false);
            _pool.Enqueue(customer);
        }
    }

    public CustomerController GetCustomerFromPool()
    {
        if (_pool.Count == 0)
            return null;

        CustomerController customer = _pool.Dequeue();
        customer.gameObject.SetActive(true);
        return customer;
    }

    public void AddCustomerToPool(CustomerController customer)
    {
        customer.gameObject.SetActive(false);
        _pool.Enqueue(customer);
    }
}
