using System.Linq.Expressions;

public class GameState
{
    protected GameStatemachine machine;
    public LevelManager levelManager = GameManager.Instance.levelManager;
    public GameState(GameStatemachine machine)
    {
        this.machine = machine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
}
