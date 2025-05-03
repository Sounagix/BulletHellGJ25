using System;
using UnityEngine;

public interface IDamageable
{
    public void OnReceiveDamage(float damage);
    public void OnReceiveHealth(float health);
    public void OnDeath();
    public float HPPercentage();
}
