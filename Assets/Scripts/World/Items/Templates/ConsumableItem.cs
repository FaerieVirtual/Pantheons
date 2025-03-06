using UnityEngine;

public class ConsumableItem : ItemBase
{
    //public ConsumableItem(string name, string description, Sprite itemSprite, int price) : base(name, ItemType.Consumable, description, itemSprite, price)
    //{
    //}

    public virtual void Consume() 
    {
        PlayerManager.Instance.Inventory.RemoveItem(this, 1);
    }
}

