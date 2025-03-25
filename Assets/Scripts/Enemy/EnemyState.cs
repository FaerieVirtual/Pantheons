// This is a copied class cited in documentation in source [3]

public class EnemyState
{
    protected EnemyBase enemy;
    protected EnemyStateMachine machine;

    public EnemyState(EnemyBase enemy, EnemyStateMachine machine)
    {
        this.enemy = enemy;
        this.machine = machine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void PhysicsUpdate() { }
}
