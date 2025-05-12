using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GamePausedState : GameState
{
    public GamePausedState(GameStateMachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        Time.timeScale = 0;
        UIManager.Instance.PauseMenuUI.SetActive(true);
    }

    public override void ExitState()
    {
        Time.timeScale = 1;
        UIManager.Instance.PauseMenuUI.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
    }
}
