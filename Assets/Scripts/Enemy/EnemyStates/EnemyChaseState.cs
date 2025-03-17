using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyBase enemy, EnemyStatemachine machine) : base(enemy, machine)
    {
    }

    private EnemyPatrolState patrolState;
    //public override void EnterState()
    //{
    //    enemy.Animator.StopPlayback();
    //    enemy.Animator.Play("Attack");
    //}
    public override void PhysicsUpdate()
    {
        bool groundDetect = enemy.GroundCheck.IsTouchingLayers(enemy.Ground);
        bool wallDetect = enemy.WallCheck.IsTouchingLayers(enemy.Ground);
        if (!groundDetect) { enemy.Flip(); }
        if (wallDetect) { enemy.Flip(); }

        enemy.RigidBody.velocity = new(enemy.MoveDirection.x * enemy.Speed * 2, enemy.RigidBody.velocity.y);

        if (!enemy.ChaseRadius.IsTouching(PlayerManager.Instance.GetComponent<Collider2D>()))
        {
            patrolState = new(enemy, machine);
            machine.ChangeState(patrolState);
        }
    }
}

