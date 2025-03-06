using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractible
{
    public string Name;
    public HashSet<string> Flags = new();
    public bool CanInteract { get; set; }


    public TextMeshPro TextBox;
    public virtual void Interaction() { }
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
    public void WaitForInput()
    {
        IEnumerator WaitForInputCoroutine()
        {
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.DownArrow));
        }
        StartCoroutine(WaitForInputCoroutine());
    }

}

