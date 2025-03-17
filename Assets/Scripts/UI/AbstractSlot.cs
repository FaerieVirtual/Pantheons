using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbstractSlot 
{
    public ItemBase Item;
    public int Quantity;
    public bool IsEmpty => Item == null || Quantity == 0;

    public void AddItem(ItemBase item, int amount = 1)
    {
        if (Item == null)
        {
            Item = item;
            Quantity = amount;
        }
        else if (Item.Name == item.Name)
        {
            Quantity += amount;
        };
    }
    public void RemoveItem(int amount)
    {
        if (Item != null)
        {
            Quantity -= amount;
            if (Quantity <= 0)
            {
                Item = null;
            }
        }
    }
}