using System.ComponentModel;
using UnityEngine;

public class PauseMenu : MonoBehaviour, IMenuBase
{

    Game game;
    PausedState paused;
    RunningState running;

    public static PauseMenu instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        if (instance != null) { Destroy(gameObject); }
    }
    private void Start()
    {
        game = FindAnyObjectByType<Game>();
        paused = new PausedState(game.machine);
        running = new RunningState(game.machine);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && game.machine.currentState == paused)
        {
            Resume();
        }
    }
    public void Resume()
    {
        game.machine.ChangeState(running);
    }

    public void Options()
    {
        throw new System.NotImplementedException();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
