using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
    void Die();
}
