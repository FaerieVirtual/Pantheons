

using UnityEngine;

public class ItemBase : IItem
    {
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public string Description { get; set; }
        public Sprite ItemSprite { get; set; }
        public int Price { get; set; }
        public bool Consumable { get; set; }
    }

