using System;
using UnityEngine;


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
public struct RangeFloat
{
    public float Min;
    public float Max;
}
