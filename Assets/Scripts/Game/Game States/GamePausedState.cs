using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GamePausedState : GameState
{
    private PauseMenu pauseMenu;
    public GamePausedState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        Time.timeScale = 0;
        pauseMenu = Object.FindObjectOfType<PauseMenu>(true);
        pauseMenu.gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
    }
}
