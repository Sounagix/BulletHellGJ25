using System.Collections.Generic;
using UnityEngine;

public class InteractableSpawnManager : Manager
{
    [Header("Weapons")]
    [SerializeField]
    private InteractablePool _weaponPool;

    [SerializeField]
    private List<WeaponSpawnPoint> _weaponSpawnPoints;

    [SerializeField]
    private ThroweableWeapon[] _weaponData;

    [Header("Food")]
    [SerializeField]
    private InteractablePool _foodPool;

    [SerializeField]
    private List<FoodSpawnPoint> _foodSpawnPoints;

    [SerializeField]
    private ThroweableFood[] _foodData;

    private PlayerManager _player;
    public PlayerManager Player { set { _player = value; } }

    private GameSceneManager _gameSceneManager;
    public GameSceneManager GameSceneManager { set { _gameSceneManager = value; } }

    public override void Initialize()
    {
        _weaponPool.CreatePool();
        _foodPool.CreatePool();

        foreach (WeaponSpawnPoint weaponPoint in _weaponSpawnPoints)
        {
            weaponPoint.SetUp(_weaponPool, _player, _weaponData);
        }

        foreach (FoodSpawnPoint foodPoint in _foodSpawnPoints)
        {
            foodPoint.SetUp(_foodPool, _foodData);
        }
    }

    public override void Shutdown()
    {
    }

    #region Unity Callbacks

    private void OnEnable()
    {
        InteractableController.OnBackToThePool += OnBackToThePool;
        InteractableController.OnInteractableChange += OnInteractableChange;
        CustomerController.OnCustomerThrowProjectil += OnCustomerThrowProjectile;
    }

    private void OnDisable()
    {
        InteractableController.OnBackToThePool -= OnBackToThePool;
        InteractableController.OnInteractableChange -= OnInteractableChange;
        CustomerController.OnCustomerThrowProjectil -= OnCustomerThrowProjectile;
    }

    #endregion

    private void OnBackToThePool(InteractableController interactable)
    {
        switch (interactable.InteractableType)
        {
            case InteractableType.Weapon:
                _weaponPool.ReturnToPool(interactable);
                break;
            case InteractableType.Food:
                _foodPool.ReturnToPool(interactable);
                break;
        }
    }

    /// <summary>
    /// The Unstable effect to replace an Interactable by another one.
    /// </summary>
    /// <param name="interactable"></param>
    private void OnInteractableChange(Transform interactable)
    {
        //TODO: Trigger Glitch Effect here on the new object
        InteractableController objectToSpawn;
        bool isWeapon = UnityEngine.Random.value > 0.5f;
        if (isWeapon)
        {
            objectToSpawn = _weaponPool.GetFromPool();
            ThroweableWeapon weaponSO = _weaponData[Random.Range(0, _weaponData.Length)];

            (objectToSpawn as WeaponController).ResetObject(interactable.position, weaponSO, wasChangeable: true);
        }
        else
        {
            objectToSpawn = _foodPool.GetFromPool();
            ThroweableFood foodSO = _foodData[Random.Range(0, _foodData.Length)];

            (objectToSpawn as FoodController).ResetObject(interactable.position, isPlayerOwner: false, foodSO, wasChangeable: true);
        }

        // Copy some stats here
    }

    private void OnCustomerThrowProjectile(Transform customer)
    {
        WeaponController currentWeapon = (WeaponController)_weaponPool.GetFromPool();
        if (!currentWeapon)
            return;

        ThroweableWeapon weponSO = _weaponData[Random.Range(0, _weaponData.Length)];
        currentWeapon.ResetObject(customer.position, weponSO, wasChangeable: true);

        currentWeapon.UpdateTargetPosition(_player.transform.position);
    }
}
