using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : GameState
{
    public string LevelID { get; private set; }
    public string LevelScene { get; private set; }
    public virtual HashSet<string> Flags { get; set; } = new HashSet<string>();

    public Level(GameStatemachine machine, string LevelID, string LevelScene) : base(machine)
    {
        this.machine = machine;
        this.LevelID = LevelID;
        this.LevelScene = LevelScene;
    }

    public override async void EnterState()
    {
        if (SceneManager.GetActiveScene().name != LevelScene)
        {
            LevelConnector connector = Object.FindObjectsOfType<LevelConnector>()
                .FirstOrDefault(connector => connector.Triggered);
            if (connector != null)
            {
                connector.Triggered = false;
                await LevelManager.LoadScene(LevelScene, connector);
            }
            else await LevelManager.LoadScene(LevelScene);
        }
        if (machine.PreviousState is GameRespawningState)
        {
            SceneManager.MoveGameObjectToScene(PlayerManager.Instance.gameObject, SceneManager.GetSceneByName(LevelScene));
            Respawn respawn = Object.FindFirstObjectByType<Respawn>();

            PlayerManager.Instance.transform.position = respawn.GetRespawnpoint();
        }

        foreach (var item in GameManager.Instance.LevelManager.Levels.Values)
        {
            if (item.HasFlag("LastLevel")) item.RemoveFlag("LastLevel");
        }
        SetFlag("LastLevel");
    }
    public void SetFlag(string flag) => Flags.Add(flag);
    public void RemoveFlag(string flag) => Flags.Remove(flag);
    public bool HasFlag(string flag) { return Flags.Contains(flag); }
    public void ResetFlags() => Flags.Clear();
    public SaveLevel ToSaveLevel()
    {
        SaveLevel level = new()
        {
            LevelID = LevelID,
            LevelScene = LevelScene,
            flags = Flags
        };
        return level;
    }
}

public class SaveLevel
{
    public string LevelID;
    public string LevelScene;
    public HashSet<string> flags = new();

    public Level ToLevel()
    {
        Level level = new(GameManager.Instance.Machine, LevelID, LevelScene)
        {
            Flags = flags
        };
        return level;
    }
}
