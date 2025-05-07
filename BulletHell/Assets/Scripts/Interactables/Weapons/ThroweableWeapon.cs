using UnityEngine;

[CreateAssetMenu(fileName = "ThroweableWeapon", menuName = "Scriptable Objects/ThroweableWeapon")]
public class ThroweableWeapon : ThroweableObject
{
    [SerializeField]
    public WeaponType _wEAPON_tYPE;
}
