using UnityEngine;

public class PickupableItem : InteractibleObject
{
    public ItemBase item;
    public int quantity;

    public PickupableItem(ItemBase item, int quantity, Sprite itemsprite)
    {
        this.item = item;
        this.quantity = quantity;
    }
    public override void Interaction()
    {
        base.Interaction();
        PlayerManager.Instance.Inventory.AddItem(item, quantity);
        Destroy(gameObject);
    }
}
