using System;
using UnityEngine;

public class WeaponController : InteractableController
{
    [SerializeField]
    private float _attackPower = 1;

    [SerializeField]
    private ThroweableWeapon[] _weaponsObjects;

    private SpriteRenderer _spriteRenderer;

    private ThroweableWeapon _weapon;

    private int _index;

    public static event Action<float> OnWeaponHitPlayer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _index = UnityEngine.Random.Range(0, _weaponsObjects.Length);
        _spriteRenderer.sprite = _weaponsObjects[_index]._sprite;
    }

    public void ResetObject(Vector2 spawnPoint, ThroweableWeapon weapon)
    {
        base.ResetObject(spawnPoint);
        _weapon = weapon;
        _spriteRenderer.sprite = weapon._sprite;
    }

    protected override void OnPlayerTouched()
    {
        // Apply Damage to Player
        OnWeaponHitPlayer?.Invoke(_attackPower);
    }
}
