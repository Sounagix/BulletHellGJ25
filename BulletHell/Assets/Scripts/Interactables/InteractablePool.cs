public class InteractablePool : ObjectPool<InteractableController>
{
    public override void CreatePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            InteractableController interactable = Instantiate(_prefab, _poolParent);
            interactable.SetUp();
            interactable.gameObject.SetActive(false);
            _pool.Enqueue(interactable);
        }
    }
}