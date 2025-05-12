public class NPCState
{
    protected NPCStateMachine machine;
    public NPC npc;
    public NPCState(NPC npc, NPCStateMachine machine)
    {
        this.machine = machine;
        this.npc = npc;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }

}

