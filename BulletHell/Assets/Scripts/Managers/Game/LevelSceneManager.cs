using System;
using UnityEngine;

public class LevelSceneManager : MonoBehaviour
{
    public static Action RequestLoadScene;

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
        MasterAudioManager.Instance.PlayOneShot(OST_SOUND.OST, transform);
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

    public void ResetLevel()
    {
        _currentNumOfClientsServed = 0;
        _index = 0;
        _currentLevel = _levelData[_index];
    }

    private void NextLevel()
    {
        _index++;
        _currentNumOfClientsServed = 0;
        _currentLevel = _levelData[_index];
        RequestLoadScene?.Invoke();
    }

    public bool CanSpawnCustomer()
    {
        return _currentNumOfClientsServed < _currentLevel._numOfCLientsToServe;
    }

    public void ResetCurrentLevel()
    {
        _currentNumOfClientsServed = 0;
    }

    public float GetShootRate()
    {
        return UnityEngine.Random.Range(_currentLevel._minShootRate, _currentLevel._maxShootRate);
    }

    public float GetShootForce()
    {
        return UnityEngine.Random.Range(_currentLevel._minShootForce, _currentLevel._maxShootForce);
    }

    public float GetSpawnRate()
    {
        return UnityEngine.Random.Range(_currentLevel._minSpawnRate, _currentLevel._maxSpawnRate);
    }
}
