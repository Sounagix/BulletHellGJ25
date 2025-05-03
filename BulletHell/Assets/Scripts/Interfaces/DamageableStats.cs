using System;
using UnityEngine;

[Serializable]
public struct DamageableStats
{
    [SerializeField]
    private float _maxHP;
    public float MaxHP { get { return _maxHP; } }

    [HideInInspector]
    public float CurrentHP;

    /// <summary>
    /// In the case there are animations, this variable determine if the death animation is over
    /// </summary>
    [HideInInspector]
    public bool IsDeathFinished;
}
