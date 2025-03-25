using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TradeMenu : ItemManagingMenu
{
    public List<GraphicalSlot> slots;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public TextMeshProUGUI CoinCount;

    public GraphicalSlot DisplaySlot;
    private GraphicalSlot selectedSlot;

    public NPCData traderData;

    private void OnEnable()
    {
        if (TryGetComponent(out Canvas canvas) && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
    public override void UpdateMenu()
    {
        foreach (GraphicalSlot slot in slots)
        {
            slot.RemoveItem(slot.Quantity);
        }

        if (!traderData.Inventory.IsEmpty)
        {
            List<AbstractSlot> traderInventory = traderData.Inventory.GetAllItems();
            for (int i = 0; i < traderInventory.Count; i++)
            {
                slots[i].AddItem(traderInventory[i].Item, traderInventory[i].Quantity);
            }
        }

        CoinCount.text = $"Gold: {PlayerManager.Instance.Gold}";

        if (selectedSlot == null)
        {
            DisplaySlot.Button.image.color = Color.clear;
            itemName.text = "";
            description.text = "";
            price.text = "Price:";
        }
        CoinCount.ForceMeshUpdate();
        itemName.ForceMeshUpdate();
        description.ForceMeshUpdate();
    }

    public override void SelectSlot(GraphicalSlot slot)
    {
        if (selectedSlot != null)
        {
            selectedSlot.Button.onClick.RemoveAllListeners();
            GraphicalSlot tmp = selectedSlot;
            selectedSlot.Button.onClick.AddListener(() => SelectSlot(tmp));
        }
        DisplaySlot.RemoveItem(DisplaySlot.Quantity);

        description.text = slot.Item.Description;
        itemName.text = slot.Item.Name;
        DisplaySlot.AddItem(slot.Item);
        price.text = $"Price: {slot.Item.Price}";

        selectedSlot = slot;
    }

    public void Trade()
    {
        if (PlayerManager.Instance.Gold < selectedSlot.Item.Price) return;

        PlayerManager.Instance.Inventory.AddItem(selectedSlot.Item, 1);
        traderData.Inventory.RemoveItem(selectedSlot.Item, 1);

        PlayerManager.Instance.Gold -= selectedSlot.Item.Price;

        if (!traderData.Flags.Contains("ItemBought")) traderData.Flags.Add("ItemBought");
        UpdateMenu();
    }
}

