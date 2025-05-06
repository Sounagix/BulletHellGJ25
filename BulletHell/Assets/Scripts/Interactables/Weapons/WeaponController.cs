using System;
using UnityEngine;

public class WeaponController : InteractableController
{
    [SerializeField]
    private float _attackPower = 1;

    public static event Action<float> OnWeaponHitPlayer;
    protected override void OnPlayerTouched()
    {
        // Apply Damage to Player
        OnWeaponHitPlayer?.Invoke(_attackPower);
    }
}
