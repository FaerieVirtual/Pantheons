using UnityEngine;

public class EquipmentItem : ItemBase
{
    //public EquipmentItem(string name, string description, Sprite itemSprite, int price) : base(name, ItemType.Equipment, description, itemSprite, price)
    //{
    //}
    public virtual void AddBoost() { }
    public virtual void RemoveBoost() { }

}

