using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(EnemyBase enemy, EnemyStatemachine machine) : base(enemy, machine)
    {
        //Ground = enemy.Ground;
        //GroundCheck = enemy.GroundCheck;
        //WallCheck = enemy.WallCheck;
        //Speed = enemy.Speed;
        //ChaseRadius = enemy.ChaseRadius;
    }
    //LayerMask Ground;
    //BoxCollider2D GroundCheck;
    //BoxCollider2D WallCheck;
    //Collider2D ChaseRadius;
    //int Speed;

    private EnemyIdleState IdleState;
    private EnemyChaseState chaseState;

    //public override void EnterState()
    //{
    //    enemy.Animator.StopPlayback();
    //    enemy.Animator.Play("Run");
    //}
    public override void PhysicsUpdate()
    {
        bool groundDetect = enemy.GroundCheck.IsTouchingLayers(enemy.Ground);
        bool wallDetect = enemy.WallCheck.IsTouchingLayers(enemy.Ground);
        if (!groundDetect) { enemy.Flip(); }
        if (wallDetect) { enemy.Flip(); }

        enemy.RigidBody.velocity = new(enemy.MoveDirection.x * enemy.Speed, enemy.RigidBody.velocity.y);
        if (enemy.ChaseRadius != null && enemy.ChaseRadius.IsTouching(PlayerManager.Instance.GetComponent<Collider2D>()))
        {
            chaseState = new(enemy, machine);
            machine.ChangeState(chaseState);
        }
    }
    public override void Update()
    {
        if (!PlayerManager.Instance.Alive) 
        {
            IdleState = new(enemy, machine);
            enemy.Machine.ChangeState(IdleState); 
        }
    }

}
