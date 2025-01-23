using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadMenuState : GameState
{
    public GameLoadMenuState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }
    public override void EnterState()
    {
        //GameManager.Area = 0;
    }
    public override void ExitState() 
    {
    }
    public override void Update() 
    { 
        
    }
}
