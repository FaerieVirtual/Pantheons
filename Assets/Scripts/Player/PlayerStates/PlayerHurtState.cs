using UnityEngine;
using UnityEngine.Events;

public class PlayerHurtState : PlayerState
{
    private bool invincible;
    private Animator animator;

    public UnityEvent playerHit = new UnityEvent();
    public PlayerHurtState(PlayerManager player, PlayerStatemachine machine) : base(player, machine)
    {
        this.player = player;
        this.machine = machine;
        invincible = player.invincible;
        animator = player.Animator;

        playerHit.AddListener(AudioManager.instance.OnPlayerHit);
        playerHit.AddListener(GameManager.instance.OnPlayerHit);
    }

    public override void EnterState()
    {
        playerHit.Invoke();
        animator.Play("Hurt");
        invincible = true; 

    }

    public override void ExitState()
    {
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
