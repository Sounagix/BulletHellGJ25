using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Manager, IDamageable
{
    #region Static/Const Variables

    public static event Action<float> OnPlayerTakeDamage;
    public static event Action<float, bool> OnPlayerHealed;

    #endregion

    [SerializeField]

    private PlayerController _playerController;

    [SerializeField]
    private DamageableStats _damageableStats;
    public DamageableStats DamageableStats
    {
        get { return _damageableStats; }
    }
    [SerializeField]
    private PlayerSFXController _playerSFXController;

    [SerializeField]
    [Range(0, 1)]
    private float _heartBeatHealthThreshHold;

    [SerializeField]
    private Animator _animator;

    private GameSceneManager _gameSceneManager;
    public GameSceneManager GameSceneManager { set { _gameSceneManager = value; } }

    private Vector3 _initialPos;
    private Quaternion _initialRotation;
    private Vector3 _initialScale;

    private Queue<FoodType> _inventory;
    public Queue<FoodType> Inventory;

    public override void Initialize()
    {
        if (_isInitialized)
            return;

        _damageableStats.CurrentHP = _damageableStats.MaxHP;
        _initialPos = transform.position;
        _initialRotation = transform.rotation;
        _initialScale = transform.localScale;

        _playerController.SetUp(this/*, playerSFXController*/);
        //playerSFXController.PlaySpawnPlayer();

        _isInitialized = true;
    }

    public override void Shutdown()
    {
    }

    #region Unity Callbacks

    private void OnEnable()
    {
        WeaponController.OnWeaponHitPlayer += OnReceiveDamage;
    }

    private void OnDisable()
    {
        WeaponController.OnWeaponHitPlayer -= OnReceiveDamage;
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

    private void Update()
    {
        _animator.SetFloat("velX", _playerController.GetVelocity().x);
        _animator.SetFloat("velY", _playerController.GetVelocity().y);
    }

    #region IDamageable

    public void OnReceiveDamage(float damage)
    {
        Shaker.ShakeCam?.Invoke();
        PTCManager.OnEventPTCCreate?.Invoke(PTCType.PlayerDamaged, transform.position);
        MasterAudioManager.Instance.PlayOneShot(PLAYER_SOUNDS.TAKE_DAMAGE, transform);
        StatisticsManager.OnPlayerReciveDamage?.Invoke();
        _damageableStats.CurrentHP = Mathf.Max(0, _damageableStats.CurrentHP - damage);
        bool isLowHealth = _damageableStats.CurrentHP <= _damageableStats.MaxHP * _heartBeatHealthThreshHold;

        //if (isLowHealth)
        //    playerSFXController.PlayHeartBeat();

        if (damage > 0)
        {
            OnPlayerTakeDamage?.Invoke(_damageableStats.CurrentHP);
        }

        if (_damageableStats.CurrentHP <= 0)
            StartCoroutine(_gameSceneManager.GameOver());
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
        _playerSFXController.StopHeartBeat();
        _playerSFXController.PlayPlayerDeathSFX();
    }

    public float HPPercentage()
    {
        if (_damageableStats.MaxHP == 0)
            return 0;

        return _damageableStats.CurrentHP / _damageableStats.MaxHP;
    }

    #endregion

    public void PauseGame() 
    {
        if (!_gameSceneManager || _gameSceneManager.GameState != GameState.Playing)
            return;

        _gameSceneManager.PauseGame();
    }
}