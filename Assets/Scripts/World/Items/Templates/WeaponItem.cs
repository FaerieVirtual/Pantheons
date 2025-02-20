using UnityEngine;

public class WeaponItem : ItemBase
{
    public int damage;
    public float reach;

    public new ItemType Type = ItemType.Sword;

    public WeaponItem(string name, ItemType type, string description, Sprite itemSprite, int price) : base(name, type, description, itemSprite, price)
    {
    }
}

