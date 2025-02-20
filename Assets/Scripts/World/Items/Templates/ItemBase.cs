using UnityEngine;

public abstract class ItemBase : IItem
{
    public ItemBase(string name, ItemType type, string description, Sprite itemSprite, int price)
    {
        Name = name;
        Type = type;
        Description = description;
        ItemSprite = itemSprite;
        Price = price;
    }

    public string Name { get; set; }
    public ItemType Type { get; set; }
    public string Description { get; set; }
    public Sprite ItemSprite { get; set; }
    public int Price { get; set; }
}

