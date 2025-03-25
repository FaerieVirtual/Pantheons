using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractiveObject
{
    public List<ItemBase> Items;
    public List<int> Quantities;
    public string ObjectID;
    public bool Opened;

    public Chest(List<ItemBase> items, List<int> quantities, string objectID)
    {
        Items = items;
        Quantities = quantities;
        ObjectID = objectID;
    }
    private void Start()
    {
        if (Opened) 
        {
            CanInteract = false;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/OpenChest");
        }
    }
    public override void Interaction()
    {
        base.Interaction();
        for (int i = 0; i < Items.Count; i++)
        {
            PlayerManager.Instance.Inventory.AddItem(Items[i], Quantities[i]);
        }
        Opened = true;
        CanInteract = false;
    }

    public override void OnLevelHasFlag()
    {
        Opened = true;
    }
}
