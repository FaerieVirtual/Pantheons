using UnityEngine;

public class PickupableItem : InteractibleObject
{
    [SerializeField]
    public ItemBase item;
    [SerializeField]
    public int quantity;
    [SerializeField]
    public Sprite itemsprite;

    public PickupableItem(ItemBase item, int quantity, Sprite itemsprite)
    {
        this.item = item;
        this.quantity = quantity;
        this.itemsprite = itemsprite;
    }
    public override void Interaction()
    {
        item.ItemSprite = itemsprite;
        Debug.Log($"pickup interaction, player inventory: {PlayerManager.Instance.Inventory}, item: {item}, q: {quantity}");
        PlayerManager.Instance.Inventory.AddItem(item, quantity);
        Destroy(gameObject);
    }
}
