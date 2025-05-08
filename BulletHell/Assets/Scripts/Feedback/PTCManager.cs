using System;
using UnityEngine;

public enum PTCType
{
    BorderCollision,
    PlayerDamaged,
}

public class PTCManager : MonoBehaviour
{
    public static Action <PTCType, Vector2> OnEventPTCCreate;

    [SerializeField]
    private Transform _ptcPool;

    [SerializeField]
    private ParticleSystem _borderCollisionPTCPrefab;

    [SerializeField]
    private ParticleSystem _playerDamagedPTCPrefab;



    private void OnEnable()
    {
        OnEventPTCCreate += CreatePTC;
    }

    private void CreatePTC(PTCType pTCType, Vector2 pos)
    {
        switch (pTCType)
        {
            case PTCType.BorderCollision:
                CreatePTC(_borderCollisionPTCPrefab, pos);
                break;
            case PTCType.PlayerDamaged:
                CreatePTC(_playerDamagedPTCPrefab, pos);
                break;
        }
    }

    private void CreatePTC(ParticleSystem ptc, Vector2 pos)
    {
        var borderPTC = Instantiate(ptc, _ptcPool);
        borderPTC.transform.position = pos;
        Destroy(borderPTC.gameObject, 1.0f);
    }
}
