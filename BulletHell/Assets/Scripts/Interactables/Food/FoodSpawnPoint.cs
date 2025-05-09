using UnityEngine;

public class FoodSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private RangeFloat _spawnRateRange;

    private float _currentFoodTime;
    private float _spawnFoodEveryThisSeconds;
    private InteractablePool _foodPool;
    private ThroweableFood[] _foodData;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetUp(InteractablePool foodPool, ThroweableFood[] foodData)
    {
        _foodData = foodData;
        _foodPool = foodPool;
        _spawnFoodEveryThisSeconds = UnityEngine.Random.Range(_spawnRateRange.Min, _spawnRateRange.Max);
    }

    private void Update()
    {
        if (!_foodPool)
            return;

        SpawnFood();
    }

    private void SpawnFood()
    {
        _currentFoodTime += Time.deltaTime;

        if (_currentFoodTime < _spawnFoodEveryThisSeconds)
            return;

        _currentFoodTime = 0;

        FoodController food = (FoodController)_foodPool.GetFromPool();

        if (!food || _foodData.Length == 0)
            return;

        _animator.SetTrigger("Spawn");
        ThroweableFood foodSO = _foodData[Random.Range(0, LevelSceneManager.Instance.GetCurrentLevel()._maxFoodsToSpawn)];
        food.ResetObject(transform.position, isPlayerOwner: false, foodSO);
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        food.UpdateTargetPosition(randomDir);
    }
}
