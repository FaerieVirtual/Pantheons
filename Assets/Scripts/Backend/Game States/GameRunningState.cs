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
<<<<<<< Updated upstream
=======
        UnityEngine.SceneManagement.SceneManager.LoadScene("AP1");
>>>>>>> Stashed changes
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
