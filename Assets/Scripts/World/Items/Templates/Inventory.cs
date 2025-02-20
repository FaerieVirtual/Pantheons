using System.Collections.Generic;

public class Inventory
{
    public Dictionary<IItem, Slot> Items = new();

    public void AddItem(IItem item, int amount)
    {
        if (Items.TryGetValue(item, out var slot))
        {
            slot.AddItem(item);
        }
        else
        {
            Items[item] = new Slot();
            Items[item].AddItem(item, amount);
        }
    }
    public void RemoveItem(IItem item, int amount)
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

    public bool HasItem(IItem item)
    {
        if (Items.TryGetValue(item, out var slot))
        {
            return true;
        }
        else return false;
    }
    public List<Slot> GetAllItems()
    {
        return new List<Slot>(Items.Values);
    }
}

