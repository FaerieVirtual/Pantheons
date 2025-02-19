using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public List<Slot> slots;
    public TextMeshPro description;
    private void Start()
    {
        slots = new List<Slot>();
        Transform slotsChild = transform.GetChild(0);
        foreach (Transform slot in slotsChild.GetComponentInChildren<Transform>()) 
        {
            slots.Add(slot.GetComponent<Slot>());
        }
    }
}

