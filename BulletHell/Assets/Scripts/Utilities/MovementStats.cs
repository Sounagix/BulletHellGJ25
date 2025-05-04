using System;
using UnityEngine;

[Serializable]
public struct MovementStats
{
    [SerializeField]
    private float _movementForce;
    public float MovementForce { get { return _movementForce; } }

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
