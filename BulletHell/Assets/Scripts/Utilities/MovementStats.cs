using System;
using UnityEngine;

[Serializable]
public struct MovementStats
{
    [SerializeField]
    public float MovementForce;

    [SerializeField]
    private float _maxSpeed;
    public float MaxSpeed { get { return _maxSpeed; } }

    [HideInInspector]
    public Vector2 MovementDir;

    [HideInInspector]
    public Vector2 CurrentVelocity;

    [HideInInspector]
    public float CurrentMaxSpeed;
}
