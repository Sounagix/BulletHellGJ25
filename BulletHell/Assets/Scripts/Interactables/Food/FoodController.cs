using NUnit.Framework;
using UnityEngine;

public class FoodController : InteractableController
{
    [SerializeField]
    private ThroweableFood[] _foodObjects;

    private SpriteRenderer _spriteRenderer;

    private int _index;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _index = Random.Range(0, _foodObjects.Length);
        _spriteRenderer.sprite = _foodObjects[_index]._sprite;
    }

    protected override void OnPlayerTouched()
    {
        InventoryManager.EventAddFoodToInventory?.Invoke(_foodObjects[_index]);
        // Add to the Player's Inventory
    }
}
