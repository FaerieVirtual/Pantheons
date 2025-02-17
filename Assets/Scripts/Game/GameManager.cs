using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string RespawnSceneID;

    public static GameManager Instance;
    public static EventSystem EventSystemInstance;
    public LevelManager levelManager;
    //public int gameIndex;
    public string Area;

    public GameStatemachine machine = new();
    GameMainMenuState menuState;
    GamePausedState pausedState;

    #region General
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        if (Instance == null) Instance = this;

        DontDestroyOnLoad(gameObject);

        levelManager = this.AddComponent<LevelManager>();
        menuState = new GameMainMenuState(machine);
        pausedState = new GamePausedState(machine);

        machine.Init(menuState);
    }

    //private void Start()
    //{
    //    //SaveDicRefresh();
    //}
    private void Update()
    {
        machine.CurrentState.Update();
        //if (Input.GetKey(KeyCode.Escape) && machine.CurrentState != pausedState)
        //{
        //    machine.ChangeState(pausedState);
        //}
        //Area = SceneManager.GetActiveScene().name;

    }
    private void FixedUpdate()
    {
        if (slowdown) { Slowdown(); }
    }
    #endregion

    #region Save/Load & File management
    GameStats gameStats;
    PlayerStats playerStats;
    PreviewStats previewStats;

    private Dictionary<ScriptableObject, string> saveFileDic;
    //private void SaveDicRefresh() 
    //{ 
    //    saveFileDic = new Dictionary<ScriptableObject, string>()
    //    {
    //        {gameStats, $@"..\..\..\Saves\Save {gameIndex}\game"},
    //        {playerStats, $@"..\..\..\Saves\Save {gameIndex}\player" },
    //        {previewStats, $@"..\..\..\Saves\Save {gameIndex}\preview" }           
    //    };
    //}

    //public void Save()
    //{
    //    string folderPath = @"..\..\..\Saves";
    //    string savePath = Path.Combine(folderPath, $"Save {gameIndex}");
    //    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath); 
    //    if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

    //    //string GameJSON = JsonUtility.ToJson(gameStats);
    //    //string PlayerJSON = JsonUtility.ToJson(playerStats);
    //    //string PreviewJSON = JsonUtility.ToJson(previewStats);
    //    foreach (var entry in saveFileDic) 
    //    { 
    //        string JSON = JsonUtility.ToJson(entry.Key);
    //        File.WriteAllText(entry.Value, JSON);
    //    }
    //    //File.WriteAllText(Path.Combine(savePath, "game"), GameJSON);
    //    //File.WriteAllText(Path.Combine(savePath, "player"), PlayerJSON);
    //    //File.WriteAllText(Path.Combine(savePath, "preview"), PreviewJSON);
    //}

    //public void Load(int saveIndex = 0)
    //{
    //    string folderPath = Path.Combine(@"..\..\..\Saves", $"Save {saveIndex}");
    //    if (saveIndex != gameIndex)
    //    {
    //        gameIndex = saveIndex;
    //        SaveDicRefresh();
    //    }
    //    if (Directory.Exists(folderPath))
    //    {
    //        //foreach (var entry in saveFileDic) 
    //        //{ 
    //        //    if (Directory.Exists(entry.Value)) 
    //        //    { 
    //        //        string JSON = File.ReadAllText(entry.Value);
    //        //        var type = entry.Key.GetType();
    //        //        entry.Key = JsonUtility.FromJson<type>(JSON);
    //        //    }
    //        //}
    //        if (Directory.Exists(Path.Combine(folderPath, "game")))
    //        {
    //            string GameJSON = File.ReadAllText(Path.Combine(folderPath, "game"));
    //            gameStats = JsonUtility.FromJson<GameStats>(GameJSON);
    //        }
    //        else { Console.WriteLine("Error: Save corrupted: game not found"); }
    //        if (Directory.Exists(Path.Combine(folderPath, "player")))
    //        {
    //            string PlayerJSON = File.ReadAllText(Path.Combine(folderPath, "player"));
    //            playerStats = JsonUtility.FromJson<PlayerStats>(PlayerJSON);
    //        }
    //        else { Console.WriteLine("Error: Save corrupted: player not found"); }
    //        if (Directory.Exists(Path.Combine(folderPath, "preview")))
    //        {
    //            string PreviewJSON = File.ReadAllText(Path.Combine(folderPath, "preview"));
    //            previewStats = JsonUtility.FromJson<PreviewStats>(PreviewJSON);
    //        }
    //        else { Console.WriteLine("Error: Save corrupted: preview not found"); }
    //    }
    //    else { Console.WriteLine("Error: Save corrupted: folder not found."); }
    //}

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
