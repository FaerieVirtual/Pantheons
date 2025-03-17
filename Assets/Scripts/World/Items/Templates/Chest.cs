using System.Collections.Generic;

public class Chest : InteractibleObject
{
    public List<ItemBase> Items;
    public List<int> Quantities;
    public string ObjectID;

    public Chest(List<ItemBase> items, List<int> quantities, string objectID)
    {
        Items = items;
        Quantities = quantities;
        ObjectID = objectID;
    }
    public override void Interaction()
    {
        base.Interaction();
        for (int i = 0; i < Items.Count; i++)
        {
            PlayerManager.Instance.Inventory.AddItem(Items[i], Quantities[i]);
        }
        Destroy(gameObject);
    }

}
