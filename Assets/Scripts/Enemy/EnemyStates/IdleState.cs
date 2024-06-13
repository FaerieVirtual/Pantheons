using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(EnemyBase enemy, EnemyStatemachine machine) : base(enemy, machine)
    {
        this.machine = machine;
        this.enemy = enemy;
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.Move();
    }

    public override void Update()
    {
        base.Update();
    }
}
