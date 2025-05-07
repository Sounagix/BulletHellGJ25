using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static Action<ThroweableFood> EventAddFoodToInventory;

    public static Action EventRemoveFromInventory;

    public static Action<ThroweableFood> EventShootFood;

    public static int _maxNumFood = 5;

    private Stack<ThroweableFood> _foodStack = new();

    private void OnEnable()
    {
        EventAddFoodToInventory += AddToInventory;
        PlayerController.OnPlayerMouseLeftClickedUp += PopInventory;
    }

    private void OnDisable()
    {
        EventAddFoodToInventory -= AddToInventory;
        PlayerController.OnPlayerMouseLeftClickedUp -= PopInventory;
    }

    private void AddToInventory(ThroweableFood throweableFood)
    {
        if (_foodStack.Count < _maxNumFood)
        {
            _foodStack.Push(throweableFood);
        }
    }

    private void PopInventory()
    {
        if (_foodStack.Count > 0)
        {
            ThroweableFood current = _foodStack.Pop();
            if (current != null)
            {
                EventShootFood?.Invoke(current);
                EventRemoveFromInventory?.Invoke();
            }
        }
    }
}
