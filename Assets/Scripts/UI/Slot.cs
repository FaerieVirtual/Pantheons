using UnityEngine;

public class Slot : MonoBehaviour
{
    public IItem Item;
    public int Quantity;
    public bool IsEmpty => Item == null || Quantity == 0;
    public bool isLocked;
    public string SlotID;
    //public SpriteRenderer itemRenderer;

    public void AddItem(IItem item, int amount = 1)
    {
        if (isLocked) return;
        if (Item == null)
        {
            Item = item;
            //itemRenderer.enabled = true;
            //itemRenderer.sprite = Item.ItemSprite;
        }
        else if (Item.Name == item.Name)
        {
            Quantity += amount;
        };
    }
    public void RemoveItem(int amount)
    {
        if (isLocked) return;
        if (Item != null)
        {
            Quantity -= amount;
            if (Quantity <= 0)
            {
                Item = null;
                //itemRenderer.sprite = null;
                //itemRenderer.enabled = false;
            }
        }
    }
    public IItem GetItem() => Item;


}

