using System;
using UnityEngine;

public class CharmMenu : MonoBehaviour
{
    public Slot<Charm>[] slots;
    public Charm[] charms;
    public int fullSlots = 0;
    private void Start()
    {
        slots = new Slot<Charm>[PlayerManager.instance.maxCharms];
        charms[0] = new Deathward();
    }
    private void Update()
    {
        if (slots.Length < PlayerManager.instance.maxCharms - 1) { Array.Resize(ref slots, PlayerManager.instance.maxCharms); }
    }
    void EquipCharm(Charm charm)
    {

        if (fullSlots + charm.slotsRequired > slots.Length) return; //Put some visual here to indicate failed try to equip
        bool success = charm.Equip();
        if (!success)
        {
            charm.Unequip();
            return;
        }
        int freeSlot = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isEmpty)
            {
                if (charm.slotsRequired == 1)
                {
                    slots[i].AddItem(charm);
                    break;
                }
                else
                {
                    int freeSlots = 0;
                    for (int k = i; k == i + charm.slotsRequired - 1; k++)
                    {
                        if (slots[k].isEmpty) freeSlots++;
                    }

                    if (freeSlots == charm.slotsRequired)
                    {
                        slots[i].AddItem(charm);
                        for (int j = i + 1; j == i + charm.slotsRequired; j++)
                        {
                            slots[freeSlot + j].isEmpty = false;
                        }
                        break;
                    }
                    else continue;
                }
            }
        }

        fullSlots += charm.slotsRequired;
    }

    void UnequipCharm(Charm charm)
    {
        bool success = charm.Unequip();
        if (!success) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetItem() == charm) slots[i].RemoveItem();
            if (charm.slotsRequired > 1)
            {
                for (int j = i + 1; j < i + charm.slotsRequired; j++)
                {
                    if (slots[j].GetItem() == null) { slots[j].isEmpty = true; }
                }
            }
        }
        //OrganizeCharms();
    }

    //void OrganizeCharms() 
    //{
    //    List<ICharm> tmpCharms = PlayerManager.Instance.equippedCharms;

    //    for (int k = 0; k < charms.Count; k++) 
    //    {
    //        if (!PlayerManager.Instance.equippedCharms.Contains(charms[k])) tmpCharms.Add(charms[k]);
    //    }

    //    int freeSlot = 0;
    //    foreach (ICharm charm in tmpCharms) 
    //    {
    //        for (int i = 0; i < slots.Length; i++)
    //        {
    //            if (slots[i] == null)
    //            {
    //                freeSlot = i;
    //                break;
    //            }
    //        }
    //        for (int j = 0; j < charm.slotsRequired; j++)
    //        {
    //            slots[freeSlot + j] = charm;
    //        }

    //    }
    //}
    void ClearEquippedCharm()
    {
        foreach (Slot<Charm> slot in slots)
        {
            UnequipCharm(slot.GetItem());
        }
    }
}

