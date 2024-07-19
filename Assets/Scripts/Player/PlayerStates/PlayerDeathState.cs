using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathState : PlayerState
{
    private bool alive;
    private Rigidbody2D RigidBody;
    private Collider2D Collider;
    private Animator animator;

    public UnityEvent playerDeath = new UnityEvent();

    public PlayerDeathState(PlayerManager player, PlayerStatemachine machine) : base(player, machine)
    {
        this.player = player;
        this.machine = machine;
        RigidBody = player.RigidBody;
        animator = player.Animator;
        Collider = player.GetComponent<Collider2D>();
        alive = player.alive;

        playerDeath.AddListener(AudioManager.instance.OnPlayerDeath);
        playerDeath.AddListener(GameManager.instance.OnPlayerDeath);
        playerDeath.AddListener(UI.instance.OnPlayerDeath);
    }

    public override void EnterState()
    {
        animator.Play("Death");
        playerDeath.Invoke();
        RigidBody.gravityScale = 0f;
        RigidBody.velocity = Vector3.zero;
        Collider.enabled = false;
        alive = false;
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
}
