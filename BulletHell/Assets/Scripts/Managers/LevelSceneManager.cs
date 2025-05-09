using System;
using UnityEngine;

public class LevelSceneManager : MonoBehaviour
{
    [SerializeField]
    private Level[] _levelData;

    private Level _currentLevel;

    private int _currentNumOfClientsServed = 0;

    private bool _activateTutorial = true;

    private int _index = 0;

    public static LevelSceneManager Instance;


    private void OnEnable()
    {
        StatisticsManager.OnPlayerDeliverFood += OnPlayerDeliverFood;
    }

    private void OnDisable()
    {
        StatisticsManager.OnPlayerDeliverFood -= OnPlayerDeliverFood;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _currentLevel = _levelData[_index];
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ActivateTutorial()
    {
        return _activateTutorial;
    }

    public bool IsTutorialActive()
    {
        return _activateTutorial;
    }

    public void DeactiveTutorial()
    {
        _activateTutorial = false;
    }

    public Level GetCurrentLevel()
    {
        return _currentLevel;
    }

    private void OnPlayerDeliverFood(FoodType type)
    {
        _currentNumOfClientsServed++;
        if (_currentNumOfClientsServed >= _currentLevel._numOfCLientsToServe)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        _index++;
        _currentNumOfClientsServed = 0;
        _currentLevel = _levelData[_index];
        GameManager.Instance.ChangeScene((int)SceneID.Game);
    }

    public bool CanSpawnCustomer()
    {
        return _currentNumOfClientsServed < _currentLevel._numOfCLientsToServe;
    }
}
