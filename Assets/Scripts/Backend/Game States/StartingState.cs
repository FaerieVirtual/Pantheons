using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingState : GameState
{
    public StartingState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }
    public override void EnterState()
    {
        //Scene main = SceneManager.GetSceneByName("MainMenu");
        //SceneManager.LoadScene(main.name);
    }
    public override void ExitState() 
    { 
        
    }
    public override void Update() 
    { 
        
    }
}
