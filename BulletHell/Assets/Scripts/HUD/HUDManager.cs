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
    private TextMeshProUGUI _indicationsText;

    [SerializeField]
    private TextMeshProUGUI _customersCounter;

    [SerializeField]
    private string _fistIndecationString, _secondIndicationsString;

    [SerializeField]
    private float _timeToShowIndications;

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
        if (!LevelSceneManager.Instance.IsTutorialActive())
        {
            ShowIndicationsTextInHUD();
        }
        _currentCustomersCounter = 0;
        _customersCounter.text = _currentCustomersCounter.ToString() + "/" + LevelSceneManager.Instance.GetCurrentLevel()._numOfCLientsToServe.ToString();

    }

    public override void Shutdown()
    {
    }

    public void ShowIndicationsTextInHUD()
    {
        StartCoroutine(ShowIndicationsText());
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

    private IEnumerator ShowIndicationsText()
    {
        _indicationsText.gameObject.SetActive(true);
        _indicationsText.text = _fistIndecationString + " "
            + LevelSceneManager.Instance.GetCurrentLevel()._numOfCLientsToServe.ToString() + " "
            + _secondIndicationsString;
        yield return new WaitForSeconds(_timeToShowIndications);
        _indicationsText.gameObject.SetActive(false);
    }


}
