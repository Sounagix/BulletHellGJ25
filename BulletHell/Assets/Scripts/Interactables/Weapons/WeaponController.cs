using System;
using UnityEngine;

public class WeaponController : InteractableController
{
    [Header("Weapon Controller")]
    [SerializeField]
    private float _attackPower = 1;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public static event Action<float> OnWeaponHitPlayer;

    public void ResetObject(Vector2 spawnPoint, ThroweableWeapon weapon, bool wasChangeable = false)
    {
        base.ResetObject(spawnPoint, wasChangeable);
        _spriteRenderer.sprite = weapon._sprite;
    }

    protected override void OnPlayerTouched()
    {
        // Apply Damage to Player
        OnWeaponHitPlayer?.Invoke(_attackPower);
    }
}
