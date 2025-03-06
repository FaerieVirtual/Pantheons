using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;

public class Nitril : VendorNPC
{
    private void Start()
    {
        Name = "Nitril";
    }
    public override void Interaction()
    {
        if (CanInteract)
        {
            if (!HasFlag("Met"))
            {
                CanInteract = false;
                TextBox.text = $"Greetings, player, \nmy name is {Name}.";
                WaitForInput();
                TextBox.text = "Currently I am talking to you.\n But otherwise I'm a trader.";
                WaitForInput();
                TextBox.text = "If you interact with me again after this,\nI'll offer you my wares.";
                WaitForInput();
                SetFlag("Met");
                CanInteract = true;
                return;
            } else
            if (!HasFlag("ItemBought")) 
            { 
                CanInteract = false;
                OpenInventory();
                CanInteract = true;
                return;
            } else
            
            if (!HasFlag("EmptyInventory")) 
            {
                CanInteract = false;
                TextBox.text = "Hello. Back to buy more items?";
                WaitForInput();
                OpenInventory();
                CanInteract = true;
                return;
            } else
            {
                CanInteract = false;
                TextBox.text = "Apologies. I have no more stock.";
                WaitForInput();
                TextBox.text = "Perhaps we will meet again later\nwhen I get some more.";
                WaitForInput();
                CanInteract = true;
                return;
            }
        }
    }
    public override void Trade(IItem item)
    {
        base.Trade(item);
        SetFlag("ItemBought");
    }
    public override void CloseInventory()
    {
        base.CloseInventory();
        if (Inventory.GetAllItems().Count == 0) { SetFlag("EmptyInventory"); }
    }
}

