using UnityEngine;

[CreateAssetMenu(fileName = "ThroweableFood", menuName = "Scriptable Objects/ThroweableFood")]
public class ThroweableFood : ThroweableObject
{
    [SerializeField]
    public FoodType FoodType;
}
