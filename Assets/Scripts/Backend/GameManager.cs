using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int goldMultiplier;

    public GodBase god;
    public Transform lastRespawnPoint;

    public static GameManager instance;
    public static int Area;
    // -2 - RespawnMenu -1 - GodMenu 0 - MainMenu 1 - AP

    public GameStatemachine machine = new GameStatemachine();
    GameMenuState mainState;
    GamePausedState pausedState;
    GameRunningState runningState;

    #region General
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        mainState = new GameMenuState(machine);
        pausedState = new GamePausedState(machine);
        runningState = new GameRunningState(machine);

        machine.Init(mainState);
    }
    private void Start()
    {
    }
    private void Update()
    {
        machine.currentState.Update();
        if (Input.GetKey(KeyCode.Escape) && machine.currentState != pausedState)
        {
            machine.ChangeState(pausedState);
        }
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: Area = 0; break;
            case 1: Area = 1; break;
            case 2: Area = 2; break;
            case 3: Area = 2; break;
        }
    }
    private void FixedUpdate()
    {
        if (slowdown) { Slowdown(); }
    }
    #endregion

    #region StateSwitches
    public void NewGame()
    {
        MainMenu main = FindObjectOfType<MainMenu>(true);
        main.gameObject.SetActive(false);
        GodMenu god = FindObjectOfType<GodMenu>(true);
        god.gameObject.SetActive(true);
<<<<<<< HEAD
        god.enabled = false;
=======
>>>>>>> parent of 7a2af25 (13.8.24)
    }
    public void ChooseGod()
    {
        machine.ChangeState(runningState);
    }
    #endregion

    #region Player Events
    private bool slowdown;
    private float slowdownTimer;
    private float slowdownDuration = 1f;
    public void OnPlayerDeath()
    {
        Time.timeScale = 0;
    }
    public void OnPlayerRespawn()
    {
        Time.timeScale = 1;
    }
    public void OnPlayerHit()
    {
        slowdown = true;
    }
    public void OnPlayerHitDef()
    {
        slowdown = true;
    }
    private void Slowdown()
    {
        Time.timeScale = 0.3f;

        slowdownTimer += Time.unscaledDeltaTime;
        if (slowdownTimer >= slowdownDuration)
        {
            slowdown = false;
            Time.timeScale = 1.0f;
            slowdownTimer = 0f;
        }
    }
    #endregion
}
