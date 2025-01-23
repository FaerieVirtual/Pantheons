using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatemachine
{
    public GameState CurrentState {  get; set; }
    public void Init(GameState initialState) 
    { 
        CurrentState = initialState;
        CurrentState.EnterState();
    }
    public void ChangeState(GameState newState) 
    { 
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
