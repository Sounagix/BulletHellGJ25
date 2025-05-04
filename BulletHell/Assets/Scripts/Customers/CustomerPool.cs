using System.Collections.Generic;
using UnityEngine;

public class CustomerPool : ObjectPool<CustomerController>
{
    private List<Transform> _customerPath;

    public void CreateCustomerPool(List<Transform> customerPath)
    {
        _customerPath = customerPath;
        CreatePool();
    }

    public override void CreatePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CustomerController customer = Instantiate(_prefab, _poolParent);
            customer.SetUp(_customerPath);
            customer.gameObject.SetActive(false);
            _pool.Enqueue(customer);
        }
    }
}
