using System;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static Action<FoodType> OnPlayerDeliverFood;

    public static Action OnPlayerReciveDamage;

    public static Action<FoodType> OnPlayerPickFood;


    private Dictionary<FoodType, int> _foodDeliveries = new Dictionary<FoodType, int>();
    private Dictionary<FoodType, int> _foodPicked = new Dictionary<FoodType, int>();

    private int _numOfPlayerReciveDamage = 0;

    public static StatisticsManager Instance;

    private void OnEnable()
    {
        OnPlayerDeliverFood += HandlePlayerDeliverFood;
        OnPlayerReciveDamage += HandlePlayerTakingDamage;
        OnPlayerPickFood += HandlePlayerPickFood;
    }

    private void OnDisable()
    {
        OnPlayerDeliverFood -= HandlePlayerDeliverFood;
        OnPlayerReciveDamage -= HandlePlayerTakingDamage;
        OnPlayerPickFood -= HandlePlayerPickFood;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandlePlayerDeliverFood(FoodType type)
    {
        if (_foodDeliveries.ContainsKey(type))
        {
            _foodDeliveries[type]++;
        }
        else
        {
            _foodDeliveries.Add(type, 1);
        }
    }

    private void HandlePlayerPickFood(FoodType type)
    {
        if (_foodPicked.ContainsKey(type))
        {
            _foodPicked[type]++;
        }
        else
        {
            _foodPicked.Add(type, 1);
        }
    }

    private void HandlePlayerTakingDamage()
    {
        _numOfPlayerReciveDamage++;
    }

    public void ResetInfo()
    {
        _foodDeliveries.Clear();
        _foodPicked.Clear();
        _numOfPlayerReciveDamage = 0;
    }

    public int GetPresicion()
    {
        int totalDishesDelivered = GetTotalDishesDeliveredCorrectly();
        int totalIngredientCollected = GetTotalIngredientCollected();
        if (totalIngredientCollected == 0)
        {
            return 0;
        }
        return (int)((float)totalDishesDelivered / totalIngredientCollected * 100);
    }

    public int GetTotalDishesDeliveredCorrectly()
    {
        int total = 0;
        foreach (var delivery in _foodDeliveries)
        {
            total += delivery.Value;
        }
        return total;
    }

    public int GetTotalIngredientCollected()
    {
        int total = 0;
        foreach (var ingredient in _foodPicked)
        {
            total += ingredient.Value;
        }
        return total;
    }

    public int GetTotalTimesPlayerDamaged()
    {
        return _numOfPlayerReciveDamage;
    }
}
