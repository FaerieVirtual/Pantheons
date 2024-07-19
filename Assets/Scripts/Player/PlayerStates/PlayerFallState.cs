using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFallState : PlayerState
{
    private float timer = 0f;
    private float rampupTime = 3f;
    private Rigidbody2D RigidBody;
    private Animator animator;
    private AudioManager audio;

    public UnityEvent playerFall = new UnityEvent();
    public UnityEvent playerFallStop = new UnityEvent();

    public PlayerFallState(PlayerManager player, PlayerStatemachine machine) : base(player, machine)
    {
        this.player = player;
        this.machine = machine;
        RigidBody = player.RigidBody;
        animator = player.Animator;
        audio = player.Audio;
        playerFall.AddListener(audio.OnPlayerFall);
        playerFallStop.AddListener(audio.OnPlayerFallStop);
    }

    public override void EnterState()
    {
        playerFall.Invoke();
        animator.Play("Fall");
    }

    public override void ExitState()
    {
        playerFallStop.Invoke();
        RigidBody.gravityScale = 10;
        timer = 0;
    }

    public override void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer > rampupTime && RigidBody.gravityScale < 50f)
        {
            playerFall.Invoke();
            RigidBody.gravityScale += 2;
            timer = rampupTime - 0.2f;
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
