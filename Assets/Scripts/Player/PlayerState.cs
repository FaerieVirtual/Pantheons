using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerState
{
    protected PlayerManager player;
    protected PlayerStatemachine machine;

    public PlayerState(PlayerManager player, PlayerStatemachine machine)
    {
        this.player = player;
        this.machine = machine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void PhysicsUpdate() { }
}
