using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public int _maxFoodsToSpawn;

    public int _numOfCLientsToServe;
}
