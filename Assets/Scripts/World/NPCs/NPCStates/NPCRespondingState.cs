using System.Linq;

public class NPCRespondingState : NPCState
{
    public NPCRespondingState(NPC npc, NPCStateMachine machine) : base(npc, machine)
    {
    }

    private NPCResponse response;
    public override void EnterState()
    {
        if (npc.CurrentResponse == null)
        {
            npc.CurrentResponse = GetResponse();
            npc.ResponseIndex = 0;
        }

        response = npc.CurrentResponse;
        string responseString = response.SplitResponse[npc.ResponseIndex];

        npc.TextBox.text = responseString;
        npc.ResponseIndex++;
        if (responseString == response.SplitResponse.Last())
        {
            if (responseString.Contains("#")) npc.TextBox.text = "";
            if (response.ExclusionFlag != null) npc.Data.Flags.Add(response.ExclusionFlag);
            npc.CurrentResponse = null;
        }

    }
    public NPCResponse GetResponse()
    {
        foreach (NPCResponse r in npc.Data.NPCResponses)
        {
            if ((r.TriggerFlag == null || npc.Data.Flags.Contains(r.TriggerFlag)) && (r.ExclusionFlag == null || !npc.Data.Flags.Contains(r.ExclusionFlag)))
            {
                return r;
            }
        }
        return null;
    }

}

