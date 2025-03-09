using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Data/NPCData")]
public class NPCData : ScriptableObject
{
    [SerializeField] public HashSet<string> Flags = new();
    [SerializeField] public Inventory Inventory = new();
    [SerializeField] public List<NPCResponse> NPCResponses = new();

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
}
