using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string RespawnSceneID;

    public static GameManager Instance;
    public DataManager DataManager;
    public LevelManager LevelManager;
    //public int gameIndex;
    public string Area;

    public GameStatemachine machine = new();
    private GameMainMenuState menuState;

    #region General
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        if (Instance == null) Instance = this;

        DontDestroyOnLoad(gameObject);

        LevelManager = this.AddComponent<LevelManager>();
        DataManager = this.AddComponent<DataManager>();
        menuState = new GameMainMenuState(machine);

        machine.Init(menuState);
    }

    private void Update()
    {
        machine.CurrentState.Update();
    }
    #endregion


}
