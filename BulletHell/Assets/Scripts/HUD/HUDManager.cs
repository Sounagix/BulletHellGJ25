using System.Collections;
using TMPro;
using UnityEngine;

public class HUDManager : Manager
{
    [SerializeField]
    private InventoryHUD _inventoryHUD;

    [SerializeField]
    private LifeHUD _lifeHUD;

    [SerializeField]
    private TimeHUD _timeHUD;

    [SerializeField]
    private TextMeshProUGUI _customersCounter;

    private int _currentCustomersCounter = 0;

    private PlayerManager _player;
    public PlayerManager Player { set { _player = value; } }

    private void OnEnable()
    {
        StatisticsManager.OnPlayerDeliverFood += OnUpdateCustomersCounter;
    }

    private void OnDisable()
    {
        StatisticsManager.OnPlayerDeliverFood -= OnUpdateCustomersCounter;
    }

    public override void Initialize()
    {
        if (_isInitialized)
            return;

        _lifeHUD.SetUp(_player.DamageableStats.MaxHP);
        _inventoryHUD.SetUp();
        _timeHUD.SetUp();

        _isInitialized = true;

    }

    private void Start()
    {
        _currentCustomersCounter = 0;
        _customersCounter.text = _currentCustomersCounter.ToString() + "/" + LevelSceneManager.Instance.GetCurrentLevel()._numOfCLientsToServe.ToString();
    }

    public override void Shutdown()
    {
    }

    public void OnReset()
    {
        _lifeHUD.OnReset(_player.DamageableStats.MaxHP);
        _inventoryHUD.OnReset();
        _timeHUD.OnReset();
    }

    private void OnUpdateCustomersCounter(FoodType type)
    {
        _currentCustomersCounter++;
        _customersCounter.text = _currentCustomersCounter.ToString() + "/" + LevelSceneManager.Instance.GetCurrentLevel()._numOfCLientsToServe.ToString();
    }
}
