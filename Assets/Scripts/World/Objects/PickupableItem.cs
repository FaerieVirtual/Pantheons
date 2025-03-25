public class PickupableItem : InteractiveObject
{
    public ItemBase Item;
    public int Quantity;
    public string ObjectID;

    public PickupableItem(ItemBase item, int quantity, string objectID)
    {
        Item = item;
        Quantity = quantity;
        ObjectID = objectID;
    }
    public override void Interaction()
    {
        base.Interaction();
        PlayerManager.Instance.Inventory.AddItem(Item, Quantity);
        Destroy(gameObject);
    }
    public override void OnLevelHasFlag()
    {
        Destroy(gameObject);
    }
}
