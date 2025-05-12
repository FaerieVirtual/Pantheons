using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory 
{
    public List<AbstractSlot> Items = new();
    public bool IsEmpty => Items.Count < 1;
    public void AddItem(ItemBase item, int amount)
    {
        AbstractSlot slot = Items.FirstOrDefault(slot => slot.Item == item);
        if (slot != null)
        {

            slot.AddItem(item, amount);
        }
        else
        {
            slot = new AbstractSlot();
            slot.AddItem(item, amount);
            Items.Add(new AbstractSlot());
        }
    }
    public void RemoveItem(ItemBase item, int amount)
    {
        AbstractSlot slot = Items.FirstOrDefault(slot => slot.Item == item);

        if (slot != null)
        {
            slot.RemoveItem(amount);
            if (slot.IsEmpty)
            {
                Items.Remove(slot);
            }
        }
    }

    public bool HasItem(ItemBase item)
    {
        AbstractSlot slot = Items.FirstOrDefault(slot => slot.Item == item);
        if (slot != null)
        {
            return true;
        }
        else return false;
    }

    public SaveInventory ToSaveInventory()
    {
        SaveInventory inventory = new();
        foreach (var slot in Items)
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

