using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyBase enemy, EnemyStatemachine machine) : base(enemy, machine)
    {
        Speed = enemy.Speed;
    }
    int Speed;

    private EnemyPatrolState patrolState;
    //public override void EnterState()
    //{
    //    enemy.Animator.StopPlayback();
    //    enemy.Animator.Play("Attack");
    //}
    public override void PhysicsUpdate()
    {
        Vector2 direction = new Vector2(PlayerManager.Instance.transform.position.x - enemy.transform.position.x, 0).normalized;

        if (direction == Vector2.left && enemy.MoveDirection == Vector2.right) { enemy.Flip(); }
        else if (direction == Vector2.right && enemy.MoveDirection == Vector2.left) { enemy.Flip(); }

        enemy.RigidBody.velocity = new(direction.x * enemy.Speed * 2, enemy.RigidBody.velocity.y);

        if (!enemy.ChaseRadius.IsTouching(PlayerManager.Instance.GetComponent<Collider2D>()))
        {
            patrolState = new(enemy, machine);
            machine.ChangeState(patrolState);
        }
    }
}

