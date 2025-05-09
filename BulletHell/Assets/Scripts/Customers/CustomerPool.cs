using UnityEngine;

public class CustomerPool : ObjectPool<CustomerController>
{
    private Transform _playerTr;
    public void CreateCustomerPool(Transform playerTr)
    {
        _playerTr = playerTr;
        CreatePool();
    }

    public override void CreatePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CustomerController customer = Instantiate(_prefab, _poolParent);
            customer.SetUp(_playerTr);
            customer.gameObject.SetActive(false);
            _pool.Enqueue(customer);
        }
    }
}
