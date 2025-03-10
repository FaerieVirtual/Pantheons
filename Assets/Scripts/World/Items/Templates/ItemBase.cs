using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    public string Name;
    public ItemType Type;
    public string Description;
    public Sprite ItemSprite;
    public int Price;
}

