using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Data/NPCData")]
public class NPCData : ScriptableObject
{
    public HashSet<string> Flags = new();
    public Inventory Inventory = new();
    public List<NPCResponse> NPCResponses = new();

    public bool HasFlag(string flag)
    {
        return Flags.Contains(flag);
    }

    public void SetFlag(string flag)
    {
        Flags.Add(flag);
    }
    public bool RemoveFlag(string flag)
    {
        if (Flags.Contains(flag))
        {
            Flags.Remove(flag);
            return true;
        }
        else return false;
    }
    public void ClearFlags()
    {
        Flags.Clear();
    }

    public SaveNPCData ToSaveNPCData() 
    {
        SaveNPCData data = new()
        {
            Flags = Flags,
            Inventory = Inventory.ToSaveInventory(),
        };

        return data;
    }
}

public class SaveNPCData 
{
    public HashSet<string> Flags = new();
    public SaveInventory Inventory = new();
    public NPCData ToNPCData()
    {
        NPCData tmp = new()
        {
            Flags = Flags,
            Inventory = Inventory.ToInventory()
        };
        return tmp;
    }

}