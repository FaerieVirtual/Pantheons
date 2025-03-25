// This is a copied class cited in documentation in source [3]

public class EnemyStateMachine
{
    public EnemyState CurrentState {  get; set; }
    public void Init(EnemyState initialState) 
    { 
        CurrentState = initialState;
        CurrentState.EnterState();
    }
    public void ChangeState(EnemyState newState) 
    { 
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
