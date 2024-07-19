using System.Collections;
using UnityEngine;

public class DeathState : EnemyState
{
    Animator animator;
    public DeathState(EnemyBase enemy, EnemyStatemachine machine) : base(enemy, machine)
    {
        this.machine = machine;
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        enemy.RigidBody.velocity = Vector3.zero;
        animator = enemy.GetComponent<Animator>();
        enemy.StartCoroutine(AnimateAndDestroy());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
    private IEnumerator AnimateAndDestroy() 
    {
        animator.Play("Death");
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(info.length);
        Object.Destroy(enemy);
    }
}
