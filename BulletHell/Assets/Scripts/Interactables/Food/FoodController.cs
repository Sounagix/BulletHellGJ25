using System;
using UnityEngine;

public class FoodController : InteractableController
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private ThroweableFood _food;
    private bool _isPlayerOwner = false;
    private bool _isSpawned = false;
    protected override void OnPlayerTouched()
    {
        InventoryManager.OnTryToAddFoodToInventory?.Invoke(_food);
    }

    #region Unity Callbacks

    public override void Update()
    {
        if (_isPlayerOwner)
            return;
     
        // If player is the owner, the update won't be applied here
        // The food will return to the pool just by collisions
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (!_rb && !isActive)
            return;

        if (_isPlayerOwner)
            base.FixedUpdate();
        else if (_isSpawned)
        {
            Vector2 dir = _movementStats.MovementForce * _movementStats.MovementDir;
            _rb.AddForce(dir, ForceMode2D.Impulse);
            _isSpawned = false;
        }
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
            ReturnToThePool();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        int mask = 1 << collision.gameObject.layer;
        if ((mask & _borderLayer.value) != 0)
        {
            ReturnToThePool();
            PTCManager.OnEventPTCCreate?.Invoke(PTCType.BorderCollision, transform.position);
        }
    }

    #endregion

    public void ResetObject(Vector2 spawnPoint, bool isPlayerOwner, ThroweableFood food, bool wasChangeable = false)
    {
        base.ResetObject(spawnPoint, wasChangeable);
        _food = food;
        _spriteRenderer.sprite = food._sprite;
        _isPlayerOwner = isPlayerOwner;
        _isSpawned = true;
    }
}
