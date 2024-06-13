using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyState
{
    protected EnemyBase enemy;
    protected EnemyStatemachine machine;

    public EnemyState(EnemyBase enemy, EnemyStatemachine machine)
    {
        this.enemy = enemy;
        this.machine = machine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void PhysicsUpdate() { }
}
