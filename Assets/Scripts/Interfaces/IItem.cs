using UnityEngine;

public interface IItem 
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public string Description { get; set; }
    public Sprite ItemSprite { get; set; }
    public int Price { get; set; }
}
public enum ItemType
{
    Consumable,
    Sword,
    Equipment
}


