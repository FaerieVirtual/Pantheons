using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyBase enemy, EnemyStatemachine machine) : base(enemy, machine)
    {
        this.machine = machine;
        this.enemy = enemy;
    }
    private EnemyPatrolState patrolState;

    public override void EnterState()
    {
        enemy.Animator.StopPlayback();
        enemy.Animator.Play("Idle");
    }
    public override void Update()
    {
        if (PlayerManager.Instance.Alive)
        {
            patrolState = new(enemy, machine);
            enemy.Machine.ChangeState(patrolState);
        }
    }
}
