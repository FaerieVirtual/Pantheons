using UnityEngine;

public class ConsumableItem : ItemBase
{
    public new ItemType Type = ItemType.Consumable;

    public ConsumableItem(string name, ItemType type, string description, Sprite itemSprite, int price) : base(name, type, description, itemSprite, price)
    {
    }

    public virtual void Consume() 
    {
        PlayerManager.Instance.Inventory.RemoveItem(this, 1);
    }
}

