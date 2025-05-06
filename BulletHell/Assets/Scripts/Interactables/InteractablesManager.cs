using System.Collections.Generic;
using UnityEngine;

public class InteractableSpawnManager : Manager
{
    [Header("Weapons")]
    [SerializeField]
    private InteractablePool _weaponPool;

    [SerializeField]
    private List<WeaponSpawnPoint> _weaponSpawnPoints;

    [Header("Food")]
    [SerializeField]
    private InteractablePool _foodPool;

    [SerializeField]
    private List<FoodSpawnPoint> _foodSpawnPoints;

    private PlayerManager _player;
    public PlayerManager Player { set { _player = value; } }

    private GameSceneManager _gameSceneManager;
    public GameSceneManager GameSceneManager { set { _gameSceneManager = value; } }

    public override void Initialize()
    {
        _weaponPool.CreatePool();
        _foodPool.CreatePool();

        foreach(WeaponSpawnPoint weaponPoint in _weaponSpawnPoints) 
        {
            weaponPoint.SetUp(_weaponPool, _player);
        }

        foreach (FoodSpawnPoint foodPoint in _foodSpawnPoints)
        {
            foodPoint.SetUp(_foodPool);
        }
    }

    public override void Shutdown()
    {
    }

    #region Unity Callbacks

    private void OnEnable()
    {
        InteractableController.OnBackToThePool += OnBackToThePool;
    }

    private void OnDisable()
    {
        InteractableController.OnBackToThePool -= OnBackToThePool;
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
    private void OnInteractableChange(InteractableController interactable) 
    {
        
    }
}
