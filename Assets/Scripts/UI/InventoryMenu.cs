using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : ItemManagingMenu
{
    public List<GraphicalSlot> slots;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI CoinCount;

    public GraphicalSlot DisplaySlot;
    private GraphicalSlot selectedSlot;
    public GraphicalSlot swordSlot;
    public GraphicalSlot[] equipmentSlots;
    public GraphicalSlot consumableSlot;

    public override void UpdateMenu()
    {
        foreach (GraphicalSlot slot in slots)
        {
            slot.RemoveItem(slot.Quantity);
        }

        if (!PlayerManager.Instance.Inventory.IsEmpty)
        {
            List<AbstractSlot> playerInventory = PlayerManager.Instance.Inventory.GetAllItems();
            for (int i = 0; i < playerInventory.Count; i++)
            {
                slots[i].AddItem(playerInventory[i].Item, playerInventory[i].Quantity);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = slots[i].Item.ItemSprite;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = slots[i].Quantity.ToString();
            }
        }
        swordSlot.RemoveItem(swordSlot.Quantity);
        if (!PlayerManager.Instance.equippedWeapon.IsEmpty) swordSlot.AddItem(PlayerManager.Instance.equippedWeapon.Item, 1);

        consumableSlot.RemoveItem(consumableSlot.Quantity);
        if (!PlayerManager.Instance.equippedConsumable.IsEmpty) consumableSlot.AddItem(PlayerManager.Instance.equippedConsumable.Item, PlayerManager.Instance.equippedConsumable.Quantity);

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].RemoveItem(equipmentSlots[i].Quantity);
            if (equipmentSlots[i].IsEmpty && !PlayerManager.Instance.amulets[i].IsEmpty) equipmentSlots[i].AddItem(PlayerManager.Instance.amulets[i].Item, 1);
        }
        if (selectedSlot == null) 
        {
            DisplaySlot.Button.image.color = Color.clear;
            itemName.text = "";
            description.text = "";
        }

        CoinCount.text = $"Gold: {PlayerManager.Instance.Gold}";

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

        selectedSlot = slot;
    }

    public void EquipItem()
    {
        if (selectedSlot == null || selectedSlot.IsEmpty) return;
        GraphicalSlot slotToEquipTo = null;
        switch (selectedSlot.Item.Type)
        {
            case ItemType.Sword:
                slotToEquipTo = swordSlot;
                PlayerManager.Instance.equippedWeapon.Item = (WeaponItem)selectedSlot.Item;
                break;
            case ItemType.Consumable:
                slotToEquipTo = consumableSlot;
                PlayerManager.Instance.equippedConsumable.Item = (ConsumableItem)selectedSlot.Item;
                PlayerManager.Instance.equippedConsumable.Quantity = selectedSlot.Quantity;

                break;
            case ItemType.Equipment:
                bool assigned = false;
                for (int i = 0; i < equipmentSlots.Length; i++)
                {
                    if (equipmentSlots[i].IsEmpty)
                    {
                        slotToEquipTo = equipmentSlots[i];
                        assigned = true;
                        break;
                    }
                }
                if (!assigned) slotToEquipTo = equipmentSlots[0];
                break;
            default: Debug.Log("Attempt to assign to searched slot failed. Unknown item type."); break;
        }

        if (!slotToEquipTo.IsEmpty && slotToEquipTo.Item != selectedSlot.Item)
        {
            PlayerManager.Instance.Inventory.AddItem(slotToEquipTo.Item, slotToEquipTo.Quantity);

            slotToEquipTo.RemoveItem(slotToEquipTo.Quantity);
        }
        slotToEquipTo.AddItem(selectedSlot.Item, selectedSlot.Quantity);
        PlayerManager.Instance.Inventory.RemoveItem(selectedSlot.Item, selectedSlot.Quantity);
        selectedSlot.RemoveItem(selectedSlot.Quantity);

        UpdateMenu();
    }
}

