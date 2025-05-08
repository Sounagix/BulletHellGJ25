using System;
using UnityEngine;


[Serializable]
public class WeaponSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float _spawnEveryThisSeconds;

    [SerializeField]
    private float _spawnCount = 1;

    private float _currentWeaponTime;
    //private float _spawnWeaponEveryThisSeconds;
    private InteractablePool _weaponPool;
    private PlayerManager _player;
    private ThroweableWeapon[] _weaponData;
    public void SetUp(InteractablePool weaponPool, PlayerManager player, ThroweableWeapon[] weaponData)
    {
        _weaponData = weaponData;
        _weaponPool = weaponPool;
        _player = player;
        //_spawnWeaponEveryThisSeconds = UnityEngine.Random.Range(_spawnRateRange.Min, _spawnRateRange.Max);
    }

    private void Update()
    {
        if (!_weaponPool || !_player || _spawnCount == 0)
            return;

        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        _currentWeaponTime += Time.deltaTime;

        if (_currentWeaponTime < _spawnEveryThisSeconds)
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
            WeaponController weapon = (WeaponController)_weaponPool.GetFromPool();
            if (!weapon)
                continue;

            Vector2 targetPos = GetTargetPos(i);

            ThroweableWeapon weaponSO = _weaponData[UnityEngine.Random.Range(0, _weaponData.Length)];
            weapon.ResetObject(transform.position, weaponSO);
            weapon.UpdateTargetPosition(targetPos);
        }
    }

    private void SpawnWeaponTargetPlayer() 
    {
        WeaponController weapon = (WeaponController)_weaponPool.GetFromPool();

        if (!weapon)
            return;

        Vector2 spawnPos = (Vector2)transform.position;

        ThroweableWeapon weaponSO = _weaponData[UnityEngine.Random.Range(0, _weaponData.Length)];
        weapon.ResetObject(transform.position, weaponSO);
        weapon.UpdateTargetPosition(_player.transform.position);
    }
}
