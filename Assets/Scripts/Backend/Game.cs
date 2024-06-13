using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour 
{
    public static int goldMultiplier;

    public static God god;
    private Player player;
    public string currentScene;
    public Transform lastRespawnPoint;

    public static Game instance;

    public GameStatemachine machine = new GameStatemachine();
    StartingState starting;
    PausedState paused;
    RunningState running;
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        if (instance != null) 
        {
            Destroy(gameObject);
        }

        starting = new StartingState(machine);
        paused = new PausedState(machine);
        running = new RunningState(machine);

        machine.Init(starting);
        currentScene = SceneManager.GetActiveScene().name;
        player = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        //if (currentScene != SceneManager.GetActiveScene().name) 
        //{ 
        //    SceneManager.UnloadSceneAsync(currentScene);
        //    currentScene = SceneManager.GetActiveScene().name;
        //}
        machine.currentState.Update();
        if (Input.GetKeyDown(KeyCode.Escape) && machine.currentState != paused) { machine.ChangeState(paused); }

    }
}
