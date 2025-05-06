using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private SpawnRate _spawnRateRange;

    [SerializeField]
    private float _spinSpeed;

    private float _currentFoodTime;
    private float _spawnFoodEveryThisSeconds;
    private InteractablePool _foodPool;

    public void SetUp(InteractablePool foodPool)
    {
        _foodPool = foodPool;
        _spawnFoodEveryThisSeconds = UnityEngine.Random.Range(_spawnRateRange.MinRate, _spawnRateRange.MaxRate);
    }

    private void Update()
    {
        if (!_foodPool)
            return;

        SpawnFood();

        transform.Rotate(Vector3.forward, _spinSpeed * Time.deltaTime);
    }

    private void SpawnFood()
    {
        _currentFoodTime += Time.deltaTime;

        if (_currentFoodTime < _spawnFoodEveryThisSeconds)
            return;

        _currentFoodTime = 0;

        InteractableController food = _foodPool.GetFromPool();

        if (!food)
            return;

        food.ResetObject(transform.position);
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        food.UpdateTargetPosition(Vector2.zero);
    }
}
