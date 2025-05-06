using System;
using UnityEngine;


[Serializable]
public struct SpawnRate
{
    public float MinRate;
    public float MaxRate;
}

[Serializable]
public struct Resolution
{
    public float width;
    public float height;
}

[Serializable]
public struct CustomerRenderer
{
    public Sprite NormalState;
    public Sprite UnstableState;
}

[Serializable]
public struct LifeTimeRange
{
    public float MinLifeTimeSec;
    public float MaxLifeTimeSec;
}

[Serializable]
public struct PatienceRange 
{
    public float MinPatience;
    public float MaxPatience;
}
