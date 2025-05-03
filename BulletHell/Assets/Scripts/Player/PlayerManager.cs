using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    #region Static/Const Variables

    public static event Action<float, bool> OnPlayerTakeDamage;
    public static event Action<float, bool> OnPlayerHealed;

    #endregion

    [SerializeField]

    private PlayerController _playerController;

    [SerializeField]
    private DamageableStats _damageableStats;

    [SerializeField]
    private PlayerSFXController playerSFXController;

    [SerializeField]
    [Range(0, 1)]
    private float _heartBeatHealthThreshHold;

    private GameSceneManager _gameSceneManager;
    private Vector3 _initialPos;
    private Quaternion _initialRotation;
    private Vector3 _initialScale;

    public void SetUp(GameSceneManager gpManager)
    {
        _damageableStats.CurrentHP = _damageableStats.MaxHP;
        _initialPos = transform.position;
        _initialRotation = transform.rotation;
        _initialScale = transform.localScale;

        _gameSceneManager = gpManager;

        _playerController.SetUp(this/*, playerSFXController*/);
        //playerSFXController.PlaySpawnPlayer();
    }

    #region Unity Callbacks

    private void OnEnable()
    {
        //ProjectileController.OnPlayerHit += ReceiveDamage;
    }

    private void OnDisable()
    {
        //ProjectileController.OnPlayerHit -= ReceiveDamage;
    }

    #endregion

    public void ResetPlayer()
    {
        //playerSFXController.PlaySpawnPlayer();
        _damageableStats.IsDeathFinished = false;
        _damageableStats.CurrentHP = _damageableStats.MaxHP;
        transform.SetPositionAndRotation(_initialPos, _initialRotation);
        transform.localScale = _initialScale;
    }

    #region IDamageable

    public void OnReceiveDamage(float damage)
    {
        _damageableStats.CurrentHP = Mathf.Max(0, _damageableStats.CurrentHP - damage);
        bool isLowHealth = _damageableStats.CurrentHP <= _damageableStats.MaxHP * _heartBeatHealthThreshHold;

        //if (isLowHealth)
        //    playerSFXController.PlayHeartBeat();

        if (damage > 0)
        {
            OnPlayerTakeDamage?.Invoke(damage, isLowHealth);
        }

        if (_damageableStats.CurrentHP <= 0)
        {
            _damageableStats.CurrentHP = 0;
            StartCoroutine(_gameSceneManager.GameOver());
        }
    }

    public void OnReceiveHealth(float amount)
    {
        _damageableStats.CurrentHP += amount;
        _damageableStats.CurrentHP = Mathf.Min(_damageableStats.CurrentHP, _damageableStats.MaxHP);
        bool isNotLowHealth = _damageableStats.CurrentHP > _damageableStats.MaxHP * _heartBeatHealthThreshHold;

        //if (isNotLowHealth)
        //    playerSFXController.StopHeartBeat();

        if (amount > 0)
        {
            OnPlayerHealed?.Invoke(amount, isNotLowHealth);
        }
    }

    public void OnDeath()
    {
        _damageableStats.IsDeathFinished = true;
    }

    public void StartDeathAnimation()
    {
        playerSFXController.StopHeartBeat();
        playerSFXController.PlayPlayerDeathSFX();
    }

    public float HPPercentage()
    {
        if (_damageableStats.MaxHP == 0)
            return 0;

        return _damageableStats.CurrentHP / _damageableStats.MaxHP;
    }

    #endregion
}