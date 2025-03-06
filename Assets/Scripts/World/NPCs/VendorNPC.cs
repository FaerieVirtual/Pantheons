
using System.Collections;
using UnityEngine;

public class VendorNPC : NPC
{
    public Inventory Inventory;
    public GameObject inventoryObject;
    public override void Interaction()
    {
        base.Interaction();
    }
    public virtual void OpenInventory() 
    {
        inventoryObject.SetActive(true);
        WaitForInventoryClose();
    }
    public virtual void Trade(IItem item) 
    {
        if (PlayerManager.Instance.Gold < item.Price) return;
        if (Inventory.HasItem(item)) Inventory.RemoveItem(item, 1);
        PlayerManager.Instance.Trade(item);
    }
    public virtual void CloseInventory() 
    {
        inventoryObject.SetActive(false);
    }
    public void WaitForInventoryClose() 
    {
        StartCoroutine(WaitForInventoryCloseCoroutine());
        IEnumerator WaitForInventoryCloseCoroutine() 
        { 
            yield return new WaitUntil(() => !inventoryObject.activeInHierarchy);
        }
    }
}

