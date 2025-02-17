using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : GameState
{
    public string LevelID { get; private set; }
    public string LevelScene { get; private set; }
    public virtual HashSet<string> Flags { get; set; }

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
    }
    public void SetFlag(string flag) => Flags.Add(flag);
    public void RemoveFlag(string flag) => Flags.Remove(flag);
    public bool HasFlag(string flag) { return Flags.Contains(flag); }
    public void ResetFlags() => Flags.Clear();

}

