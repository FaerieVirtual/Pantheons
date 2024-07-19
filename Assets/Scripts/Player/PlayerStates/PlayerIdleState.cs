using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    private Animator animator;
    public PlayerIdleState(PlayerManager player, PlayerStatemachine machine) : base(player, machine)
    {
        this.player = player;
        this.machine = machine;
        animator = player.Animator;
    }

    public override void EnterState()
    {
        animator.Play("Idle");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void PhysicsUpdate()
    {
    }

    public override void Update()
    {
    }
}
