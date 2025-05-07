using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    private int _numOfFoodInInventory = 0;

    [SerializeField]
    private Image[] _foodImages;

    public void SetUp() 
    {
        OnReset();
    }

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
            Image image = _foodImages[i];
            image.color = new Color(1, 1, 1, 1);
            image.sprite = _foodImages[i - 1].sprite;

        }
        _foodImages[0].sprite = sprite;
        _foodImages[0].color = new Color(1, 1, 1, 1);
    }

    private void RemoveLastImg()
    {
        for (int i = 0; i < _numOfFoodInInventory - 1; i++)
        {
            _foodImages[i].sprite = _foodImages[i + 1].sprite;
        }

        Image image = _foodImages[_numOfFoodInInventory - 1];
        image.color = new Color(1, 1, 1, 0);
        image.sprite = null;
        _numOfFoodInInventory--;
    }

    public void OnReset()
    {
        _numOfFoodInInventory = 0;

        foreach (Image image in _foodImages) 
        {
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
        }
    }
}
