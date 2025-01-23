using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GamePausedState : GameState
{
    private PauseMenu pauseMenu;
    public UnityEvent pause;
    public UnityEvent resume;
    public GamePausedState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        pause.AddListener(AudioManager.Instance.OnPause);
        resume.AddListener(AudioManager.Instance.OnResume);
        pause.Invoke();
        Time.timeScale = 0;
        pauseMenu = Object.FindObjectOfType<PauseMenu>(true);
        pauseMenu.gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        resume.Invoke();
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
    }

    public override void Update()
    {
    }
}
