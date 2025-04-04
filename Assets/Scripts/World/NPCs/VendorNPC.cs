﻿using System.Linq;
using UnityEngine;

public class VendorNPC : NPC
{
    private GameObject menuObject;
    private void Start()
    {
        Data = GameManager.Instance.DataManager.NPCs[Name];
        menuObject = UIManager.Instance.TradeMenuUI;
    }
    public void UpdateVendorNPC()
    {
        if (Data.Inventory.IsEmpty && !Data.HasFlag("InventoryEmpty")) Data.SetFlag("InventoryEmpty");
        if (menuObject.activeSelf && TextBox.gameObject.activeSelf) { TextBox.gameObject.SetActive(false); }
        else if (!menuObject.activeSelf && !TextBox.gameObject.activeSelf) { TextBox.gameObject.SetActive(true); }
    }
    public override void Interaction()
    {
        if (!CanInteract) return;
        UpdateVendorNPC();
        if (CurrentResponse == null)
        {
            CurrentResponse = GetResponse();
            ResponseIndex = 0;
        }

        string response = CurrentResponse.SplitResponse[ResponseIndex];
        TextBox.text = response;
        ResponseIndex++;
        if (response == CurrentResponse.SplitResponse.Last())
        {

            if (response.Contains("#")) TextBox.text = "";
            if (response.Contains("$")) OpenInventory();
            if (response.Contains("!") && CurrentResponse.ExclusionFlag != null) Data.Flags.Add(CurrentResponse.ExclusionFlag);
            CurrentResponse = null;
        }
        UpdateVendorNPC();
    }
    public virtual void OpenInventory()
    {
        if (UIManager.Instance.InventoryUI.activeSelf) return;
        menuObject.SetActive(true);
        menuObject.GetComponent<TradeMenu>().traderData = Data;
        menuObject.GetComponent<TradeMenu>().UpdateMenu();
        UIManager.Instance.PlayerUI.SetActive(false);
    }
}

