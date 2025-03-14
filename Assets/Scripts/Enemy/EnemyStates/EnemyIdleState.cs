using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyBase enemy, EnemyStatemachine machine) : base(enemy, machine)
    {
        this.machine = machine;
        this.enemy = enemy;
    }
    private EnemyPatrolState patrolState;

    //public override void EnterState()
    //{
    //    enemy.Animator.StopPlayback();
    //    enemy.Animator.Play("Idle");
    //}
    public override void Update()
    {
        if (PlayerManager.Instance.Alive)
        {
            Debug.Log($"switching to patrol on {enemy.name}");
            patrolState = new(enemy, machine);
            enemy.Machine.ChangeState(patrolState);
        }
    }
}
