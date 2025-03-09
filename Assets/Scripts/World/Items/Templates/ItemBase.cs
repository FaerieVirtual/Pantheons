using UnityEngine;

public abstract class ItemBase : ScriptableObject//, IItem
{
    [SerializeField] public string Name;// { get; set; }
    public ItemType Type;// { get; set; }
    [SerializeField] public string Description;// { get; set; }
    [SerializeField] public Sprite ItemSprite;// { get; set; }
    [SerializeField] public int Price;// { get; set; }
}

