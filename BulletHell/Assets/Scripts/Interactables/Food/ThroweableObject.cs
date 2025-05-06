using UnityEngine;

[CreateAssetMenu(fileName = "ThroweableObject", menuName = "Scriptable Objects/ThroweableObject")]
public abstract class ThroweableObject : ScriptableObject
{
    [SerializeField]
    public string _objectName;

    [SerializeField]
    public Sprite _sprite;
}
