using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : Manager
{
    [Header("Weapons")]
    [SerializeField]
    private InteractablePool _weaponPool;

    [SerializeField]
    private AnimationCurve _weaponSpawnRateCurve;

    [SerializeField]
    private List<Transform> _weaponSpawnPoints;

    [Header("Food")]
    [SerializeField]
    private InteractablePool _foodPool;

    [SerializeField]
    private AnimationCurve _foodSpawnRateCurve;

    [SerializeField]
    private List<Transform> _foodSpawnPoints;

    private float _currentWeaponTime;
    private float _currentFoodTime;
    private PlayerManager _player;
    public PlayerManager Player { set { _player = value; } }

    private GameSceneManager _gameSceneManager;
    public GameSceneManager GameSceneManager { set { _gameSceneManager = value; } }

    public override void Initialize()
    {
        _weaponPool.CreatePool();
        _foodPool.CreatePool();
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

    private void Update()
    {
        if (!_gameSceneManager || !_player)
            return;

        SpawnWeapon();
        SpawnFood();
    }

    #endregion

    private void SpawnWeapon() 
    {
        _currentWeaponTime += Time.deltaTime;

        float elapsedTime = Time.timeSinceLevelLoad;
        float spawnRate = _weaponSpawnRateCurve.Evaluate(elapsedTime);

        if (_currentWeaponTime < spawnRate)
            return;

        _currentWeaponTime = 0;

        InteractableController weapon = _weaponPool.GetFromPool();

        if (!weapon)
            return;

        Transform spawnPoint = _weaponSpawnPoints[Random.Range(0, _weaponSpawnPoints.Count)];
        weapon.ResetObject(spawnPoint.position);
        weapon.UpdateMoveDir(_player.transform.position);
    }

    private void SpawnFood() 
    {
        _currentFoodTime += Time.deltaTime;

        float elapsedTime = Time.timeSinceLevelLoad;
        float spawnRate = _foodSpawnRateCurve.Evaluate(elapsedTime);

        if (_currentFoodTime < spawnRate)
            return;

        _currentFoodTime = 0;

        InteractableController food = _foodPool.GetFromPool();

        if (!food)
            return;

        Transform spawnPoint = _foodSpawnPoints[Random.Range(0, _foodSpawnPoints.Count)];
        food.ResetObject(spawnPoint.position);
        food.UpdateMoveDir(_player.transform.position);
    }

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
