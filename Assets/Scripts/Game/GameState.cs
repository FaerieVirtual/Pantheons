public class GameState
{
    protected GameStateMachine machine;
    public GameState(GameStateMachine machine)
    {
        this.machine = machine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
}
