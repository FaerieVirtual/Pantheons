using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatemachine
{
    public EnemyState currentState {  get; set; }
    public void Init(EnemyState initialState) 
    { 
        currentState = initialState;
        currentState.EnterState();
    }
    public void ChangeState(EnemyState newState) 
    { 
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
