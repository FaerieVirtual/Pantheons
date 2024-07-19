using System.Diagnostics;

public class PlayerStatemachine
{
    public PlayerState currentState { get; set; }
    public void Init(PlayerState initialState)
    {
        currentState = initialState;
        currentState.EnterState();
    }
    public void ChangeState(PlayerState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
