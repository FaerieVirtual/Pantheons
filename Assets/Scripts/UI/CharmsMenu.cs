
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharmsMenu : MonoBehaviour
{
    public Dictionary<int, Charm> charms;
    public Charm[] slots;
    public int fullSlots = 0;
    private void Start()
    {
        slots = new Charm[PlayerManager.instance.maxCharms];
        charms = new Dictionary<int, Charm> 
        {
            {1, new Charm() }
        };
    }
    void EquipCharm(int charmIndex)
    {
        Charm charm = charms[charmIndex];

        if (!charm.hasCharm) return;
        if (charm.equipped) return;
        if (fullSlots + charm.slotsRequired > slots.Length) return;

        int freeSlot = 0;

        for (int i = 0; i < slots.Length; i++) 
        {
            if (slots[i] == null) 
            { 
                freeSlot = i; 
                break;
            }
        }
        for (int j = 0; j < charm.slotsRequired; j++)
        {
            slots[freeSlot + j] = charm;
        }

        fullSlots += charm.slotsRequired;
        PlayerManager.instance.equippedCharms.Add(charm);
    }

    void UnequipCharm(int charmIndex)
    {
        Charm charm = charms[charmIndex];
        if (!charm.hasCharm) return;
        if (!charm.equipped) return;

        for (int i = 0; i < slots.Length; i++) 
        {
            if (slots[i] == charm) slots[i] = null;
        }
        charms[charmIndex].equipped = false;
        PlayerManager.instance.equippedCharms.Remove(charm);
        OrganizeCharms();
    }

    void OrganizeCharms() 
    {
        List<Charm> tmpCharms = PlayerManager.instance.equippedCharms;
        
        foreach (Charm charm in charms) 
        {
            if (!tmpCharms.Contains(charm)) tmpCharms.Add(charm);
        }

        int freeSlot = 0;
        foreach (Charm charm in tmpCharms) 
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    freeSlot = i;
                    break;
                }
            }
            for (int j = 0; j < charm.slotsRequired; j++)
            {
                slots[freeSlot + j] = charm;
            }

        }
    }
    void ClearEquippedCharm() 
    {
        fullSlots = 0;
    }
}

