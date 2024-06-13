using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatemachine
{
    public GameState currentState {  get; set; }
    public void Init(GameState initialState) 
    { 
        currentState = initialState;
        currentState.EnterState();
    }
    public void ChangeState(GameState newState) 
    { 
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
