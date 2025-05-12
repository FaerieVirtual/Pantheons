public class GameStateMachine
{
    public GameState CurrentState { get; set; }
    public GameState PreviousState { get; set; }
    public void Init(GameState initialState)
    {
        CurrentState = initialState;
        CurrentState.EnterState();
    }
    public void ChangeState(GameState newState)
    {
        CurrentState.ExitState();
        PreviousState = CurrentState;
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
