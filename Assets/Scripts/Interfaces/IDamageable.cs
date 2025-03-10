using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IDamageable
{
    public int Hp { get;  set; }
    public int MaxHp { get;  set; }
    public bool Alive => Hp > 0;
    public void TakeDamage(int damage);
    public void Die();
}
