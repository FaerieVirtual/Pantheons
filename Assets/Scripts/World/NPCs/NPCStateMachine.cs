public class NPCStateMachine
{
    public NPCState CurrentState { get; private set; } 
    public void Init(NPCState initialState)
    {
        CurrentState = initialState;
        CurrentState.EnterState();
    }
    public void ChangeState(NPCState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

}
