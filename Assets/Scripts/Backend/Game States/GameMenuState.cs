using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuState : GameState
{
    Scene main => UnityEngine.SceneManagement.SceneManager.GetSceneByName("MainMenu");
    public GameMenuState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }
    public override void EnterState()
    {
    }
    public override void ExitState() 
    {
        SceneManager.UnloadSceneAsync(main);
    }
    public override void Update() 
    { 
        
    }
}
