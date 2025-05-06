using System;
using UnityEngine;


[Serializable]
public class WeaponSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private SpawnRate _spawnRateRange;

    [SerializeField]
    private float _spawnCount = 1;

    private float _currentWeaponTime;
    private float _spawnWeaponEveryThisSeconds;
    private InteractablePool _weaponPool;
    private PlayerManager _player;

    public void SetUp(InteractablePool weaponPool, PlayerManager player)
    {
        _weaponPool = weaponPool;
        _player = player;
        _spawnWeaponEveryThisSeconds = UnityEngine.Random.Range(_spawnRateRange.MinRate, _spawnRateRange.MaxRate);
    }

    private void Update()
    {
        if (!_weaponPool || !_player)
            return;

        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        _currentWeaponTime += Time.deltaTime;

        if (_currentWeaponTime < _spawnWeaponEveryThisSeconds)
            return;

        _currentWeaponTime = 0;

        if(_spawnCount > 1)
            SpawnMultipleWeapons();
        else
            SpawnWeaponTargetPlayer();
    }

    private Vector2 GetTargetPos(int objectIndex)
    {
        float angle = (360f / _spawnCount) * objectIndex;
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        Vector2 spawnPos = (Vector2)transform.position + offset;

        return spawnPos;
    }

    private void SpawnMultipleWeapons() 
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            InteractableController weapon = _weaponPool.GetFromPool();
            if (!weapon)
                continue;

            Vector2 targetPos = GetTargetPos(i);

            weapon.ResetObject(transform.position);
            weapon.UpdateTargetPosition(targetPos);
        }
    }

    private void SpawnWeaponTargetPlayer() 
    {
        InteractableController weapon = _weaponPool.GetFromPool();

        if (!weapon)
            return;

        Vector2 spawnPos = (Vector2)transform.position;

        weapon.ResetObject(spawnPos);
        weapon.UpdateTargetPosition(_player.transform.position);
    }
}
