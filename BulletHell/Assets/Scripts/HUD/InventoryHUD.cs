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
        InventoryManager.OnTryToAddFoodToInventory += AddFoodToHUD;
        InventoryManager.OnRemoveFoodFromInventory += RemoveLastImg;
    }

    private void OnDisable()
    {
        InventoryManager.OnTryToAddFoodToInventory -= AddFoodToHUD;
        InventoryManager.OnRemoveFoodFromInventory -= RemoveLastImg;
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
            image.gameObject.SetActive(true);
            image.sprite = _foodImages[i - 1].sprite;

        }
        _foodImages[0].sprite = sprite;
        _foodImages[0].gameObject.SetActive(true);
    }

    private void RemoveLastImg()
    {
        for (int i = 0; i < _numOfFoodInInventory - 1; i++)
        {
            _foodImages[i].sprite = _foodImages[i + 1].sprite;
        }

        Image image = _foodImages[_numOfFoodInInventory - 1];
        image.gameObject.SetActive(false);
        _numOfFoodInInventory--;
    }

    public void OnReset()
    {
        _numOfFoodInInventory = 0;

        foreach (Image image in _foodImages)
            image.gameObject.SetActive(false);
    }
}
