public class NPCIdleState : NPCState
{
    public NPCIdleState(NPC npc, NPCStateMachine machine) : base(npc, machine)
    {
    }

    public override void EnterState()
    {
        npc.TextBox.text = "";
    }
}

