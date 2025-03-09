using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<ItemBase, AbstractSlot> Items = new();
    public bool IsEmpty => Items.Count < 1;
    public void AddItem(ItemBase item, int amount)
    {
        if (Items.TryGetValue(item, out var slot))
        {
            slot.AddItem(item, amount);
        }
        else
        {
            Items[item] = new AbstractSlot();
            Items[item].AddItem(item, amount);
        }
    }
    public void RemoveItem(ItemBase item, int amount)
    {
        if (Items.TryGetValue(item, out var slot))
        {
            slot.RemoveItem(amount);
            if (slot.IsEmpty)
            {
                Items.Remove(item);
            }
        }
    }

    public bool HasItem(ItemBase item)
    {
        if (Items.TryGetValue(item, out var slot))
        {
            return true;
        }
        else return false;
    }
    public List<AbstractSlot> GetAllItems()
    {
        return new List<AbstractSlot>(Items.Values);
    }
}

