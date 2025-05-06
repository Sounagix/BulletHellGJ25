using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryHUD : MonoBehaviour
{
    private int _numOfFoodInInventory = 0;

    [SerializeField]
    private UnityEngine.UI.Image[] _foodImages;

    [SerializeField]
    private Sprite _emptySlotImg;

    private void OnEnable()
    {
        InventoryManager.EventAddFoodToInventory += AddFoodToHUD;
        InventoryManager.EventRemoveFromInventory += RemoveLastImg;
    }

    private void OnDisable()
    {
        InventoryManager.EventAddFoodToInventory -= AddFoodToHUD;
        InventoryManager.EventRemoveFromInventory -= RemoveLastImg;
    }

    private void AddFoodToHUD(ThroweableFood food)
    {
        if (_numOfFoodInInventory < InventoryManager._maxNumFood)
        {
            _numOfFoodInInventory++;
            UpdateImages(food._sprite);
        }
    }

    private void UpdateImages(Sprite sprite)
    {
        for (int i = _numOfFoodInInventory - 1; i > 0; i--)
        {
            _foodImages[i].sprite = _foodImages[i - 1].sprite;
        }
        _foodImages[0].sprite = sprite;
    }

    private void RemoveLastImg()
    {
        for (int i = 0; i < _numOfFoodInInventory - 1; i++)
        {
            _foodImages[i].sprite = _foodImages[i + 1].sprite;
        }

        _foodImages[_numOfFoodInInventory - 1].sprite = _emptySlotImg;
        _numOfFoodInInventory--;
    }
}
