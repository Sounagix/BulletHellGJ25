using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public int _maxFoodsToSpawn;

    public int _numOfCLientsToServe;

    public AnimationCurve _custumerSpawnRate;

    public float _minShootRate;

    public float _maxShootRate;

    public float _minShootForce;

    public float _maxShootForce;

    public float _minSpawnRate;

    public float _maxSpawnRate;
}
