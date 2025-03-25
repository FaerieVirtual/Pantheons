using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : InteractiveObject
{
    public string Name;
    [HideInInspector] public NPCData Data;
    [HideInInspector] public NPCResponse CurrentResponse;
    [HideInInspector] public int ResponseIndex;

    public TextMeshProUGUI TextBox;

    private void Start()
    {
        Data = GameManager.Instance.DataManager.NPCs[Name];
    }

    public override void Interaction()
    {
        if (!CanInteract) return;
        if (CurrentResponse == null) 
        { 
            GetResponse(); 
            ResponseIndex = 0;
        }

        string response = CurrentResponse.SplitResponse[ResponseIndex];
        TextBox.text = response;
        ResponseIndex++;
        if (response == CurrentResponse.SplitResponse.Last())
        {
            if (response.Contains("#")) TextBox.text = "";
            if (response.Contains("!") && CurrentResponse.ExclusionFlag != null) Data.Flags.Add(CurrentResponse.ExclusionFlag);
            CurrentResponse = null;
        }
    }
    public async Task WaitForInput()
    {
        while (!Input.GetKeyDown(KeyCode.DownArrow))
        {
            await Task.Yield();
        }
    }
    public void UpdateData()
    {
        GameManager.Instance.DataManager.NPCs[Name] = Data;
    }
    public NPCResponse GetResponse()
    {
        foreach (NPCResponse r in Data.NPCResponses)
        {
            if ((r.TriggerFlag == null || Data.Flags.Contains(r.TriggerFlag)) && (r.ExclusionFlag == null || !Data.Flags.Contains(r.ExclusionFlag)))
            {
                return r;
            }
        }
        return null;
    }

}

