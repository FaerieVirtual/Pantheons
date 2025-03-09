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
    public ItemBase GetItem() => Item;

    //public void Clear()
    //{
    //    isLocked = false;
    //    Item = null;
    //    Quantity = 0;
    //}

    //public void OnItemDisplay()
    //{
    //    Button.onClick.RemoveAllListeners();
    //    if (SlotID == "Inventory")
    //    {
    //        Button.onClick.AddListener(menu.EquipItem);
    //    }
    //    else
    //    {
    //        Button.onClick.AddListener(menu.UnequipItem);
    //    }
    //}

    //public void StopItemDisplay()
    //{
    //    Button.onClick.RemoveAllListeners();
    //    Button.onClick.AddListener(() => menu.SelectSlot(gameObject.GetComponent<Slot>()));
    //}
}