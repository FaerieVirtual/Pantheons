using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : GameState
{
    public RunningState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }
}
