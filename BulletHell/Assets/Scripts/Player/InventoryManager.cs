using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    /// <summary>
    /// This event when interacting with food. It doesn't mean the food will be added
    /// </summary>
    public static Action<ThroweableFood> OnTryToAddFoodToInventory;
    /// <summary>
    /// This event is thrown when the food has been successfully added to the inventory
    /// </summary>
    public static Action<FoodType> OnInventoryUpdated;
    public static Action OnRemoveFoodFromInventory;
    public static Action<ThroweableFood> EventShootFood;

    public static int _maxNumFood = 5;

    private Stack<ThroweableFood> _foodStack = new();

    private void OnEnable()
    {
        OnTryToAddFoodToInventory += AddToInventory;
        PlayerController.OnPlayerMouseLeftClickedUp += PopInventory;
    }

    private void OnDisable()
    {
        OnTryToAddFoodToInventory -= AddToInventory;
        PlayerController.OnPlayerMouseLeftClickedUp -= PopInventory;
    }

    private void AddToInventory(ThroweableFood throweableFood)
    {
        if (_foodStack.Count < _maxNumFood)
        {
            _foodStack.Push(throweableFood);
            OnInventoryUpdated?.Invoke(throweableFood.FoodType);
            GlowOnPick.OnGlowActive?.Invoke();
        }
    }

    private void PopInventory()
    {
        if (_foodStack.Count > 0)
        {
            ThroweableFood currFood = _foodStack.Pop();
            if (currFood != null)
            {
                FoodType foodType = _foodStack.Count == 0 ? FoodType.None : _foodStack.Peek().FoodType;
                OnInventoryUpdated?.Invoke(foodType);
                OnRemoveFoodFromInventory?.Invoke();
                EventShootFood?.Invoke(currFood);
            }
        }
    }
}
