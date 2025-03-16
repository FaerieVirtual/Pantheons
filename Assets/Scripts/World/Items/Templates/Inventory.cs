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

    public SaveInventory ToSaveInventory()
    {
        SaveInventory inventory = new();
        foreach (var slot in Items.Values)
        {
            SaveSlot saveslot = new()
            {
                ItemPath = $"Items/{slot.Item.Name}",
                Quantity = slot.Quantity
            };
            inventory.items.Add(saveslot);
        }
        return inventory;
    }
}
public class SaveInventory
{
    public List<SaveSlot> items = new();

    public Inventory ToInventory() 
    {
        Inventory inventory = new();
        foreach (var saveslot in items)
        {
            inventory.AddItem(Resources.Load<ItemBase>(saveslot.ItemPath), saveslot.Quantity);
        }
        return inventory;
    }
}
public class SaveSlot 
{
    public string ItemPath;
    public int Quantity;
}

