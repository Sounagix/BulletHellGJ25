using System;
using UnityEngine;

public class FoodController : InteractableController
{
    public static event Action OnCustomerReceivesFood;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private ThroweableFood _food;
    private bool _isPlayerOwner = false;

    protected override void OnPlayerTouched()
    {
        InventoryManager.EventAddFoodToInventory?.Invoke(_food);
    }

    public void ResetObject(Vector2 spawnPoint, bool isPlayerOwner, ThroweableFood food)
    {
        base.ResetObject(spawnPoint);
        _food = food;
        _spriteRenderer.sprite = food._sprite;
        _isPlayerOwner = isPlayerOwner;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isPlayerOwner)
        {
            base.OnTriggerEnter2D(collision);
            return;
        }

        CustomerController customer = collision.GetComponent<CustomerController>();
        if (customer != null)
        {
            customer.DeliverFood(_food.FoodType);
            OnInteract();
        }
    }
}
