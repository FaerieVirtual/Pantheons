using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string RespawnSceneID;

    public static GameManager Instance;
    public DataManager DataManager;
    public LevelManager LevelManager;

    public GameStatemachine Machine = new();
    private GameMainMenuState menuState;

    #region General
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        if (Instance == null) Instance = this;

        DontDestroyOnLoad(gameObject);

        DataManager = gameObject.AddComponent<DataManager>();
        LevelManager = gameObject.AddComponent<LevelManager>();
        menuState = new GameMainMenuState(Machine);
        Machine.Init(menuState);
    }

    private void Update()
    {
        Machine.CurrentState.Update();
    }
    #endregion


}
