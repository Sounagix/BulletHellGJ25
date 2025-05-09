using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    protected Transform _poolParent;

    [SerializeField, Min(0)]
    protected int _poolSize;

    [SerializeField]
    protected T _prefab;

    protected Queue<T> _pool = new();

    public virtual void CreatePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            T obj = Instantiate(_prefab, _poolParent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public virtual T GetFromPool()
    {
        if (_pool.Count == 0)
            return null;

        T obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public virtual void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
