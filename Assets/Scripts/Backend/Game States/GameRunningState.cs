using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRunningState : GameState
{
    public GameRunningState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
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
