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
    public GraphicalSlot amuletSlot1;
    public GraphicalSlot amuletSlot2;
    public GraphicalSlot amuletSlot3;
    public GraphicalSlot consumableSlot;

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
        if (!PlayerManager.Instance.equippedWeapon.IsEmpty) swordSlot.AddItem(PlayerManager.Instance.equippedWeapon.Item);

        consumableSlot.RemoveItem(consumableSlot.Quantity);
        if (!PlayerManager.Instance.equippedConsumable.IsEmpty) consumableSlot.AddItem(PlayerManager.Instance.equippedConsumable.Item, PlayerManager.Instance.equippedConsumable.Quantity);

        amuletSlot1.RemoveItem(amuletSlot1.Quantity);
        if (!PlayerManager.Instance.equippedAmulet1.IsEmpty) amuletSlot1.AddItem(PlayerManager.Instance.equippedAmulet1.Item);

        amuletSlot2.RemoveItem(amuletSlot2.Quantity);
        if (!PlayerManager.Instance.equippedAmulet2.IsEmpty) amuletSlot2.AddItem(PlayerManager.Instance.equippedAmulet2.Item);

        amuletSlot3.RemoveItem(amuletSlot3.Quantity);
        if (!PlayerManager.Instance.equippedAmulet3.IsEmpty) amuletSlot3.AddItem(PlayerManager.Instance.equippedAmulet3.Item);
        if (selectedSlot == null)
        {
            DisplaySlot.Button.image.color = Color.clear;
            itemName.text = "";
            description.text = "";
        }

        CoinCount.text = $"Gold: {PlayerManager.Instance.Gold}";

        foreach (GraphicalSlot slot in slots) 
        { 
            slot.isLocked = false;
            slot.Button.interactable = true;
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
                PlayerManager.Instance.equippedWeapon.RemoveItem(1);
                PlayerManager.Instance.equippedWeapon.AddItem(selectedSlot.Item);
                
                break;

            case ItemType.Consumable:
                slotToEquipTo = consumableSlot;
                PlayerManager.Instance.equippedConsumable.RemoveItem(PlayerManager.Instance.equippedConsumable.Quantity);
                PlayerManager.Instance.equippedConsumable.AddItem(selectedSlot.Item, selectedSlot.Quantity);
                break;

            case ItemType.Equipment:
                if (amuletSlot1.IsEmpty)
                {
                    slotToEquipTo = amuletSlot1;
                    PlayerManager.Instance.equippedAmulet1.RemoveItem(1);
                    PlayerManager.Instance.equippedAmulet1.AddItem(selectedSlot.Item);
                }
                else if (amuletSlot2.IsEmpty)
                {
                    slotToEquipTo = amuletSlot2;
                    PlayerManager.Instance.equippedAmulet2.RemoveItem(1);
                    PlayerManager.Instance.equippedAmulet2.AddItem(selectedSlot.Item);
                }
                else if (amuletSlot3.IsEmpty)
                {
                    slotToEquipTo = amuletSlot3;
                    PlayerManager.Instance.equippedAmulet3.RemoveItem(1);
                    PlayerManager.Instance.equippedAmulet3.AddItem(selectedSlot.Item);
                }
                else
                {
                    System.Random r = new();
                    switch (r.Next(1, 4))
                    {
                        case 1:
                            slotToEquipTo = amuletSlot1;
                            PlayerManager.Instance.equippedAmulet1.RemoveItem(1);
                            PlayerManager.Instance.equippedAmulet1.AddItem(selectedSlot.Item);
                            break;
                        case 2:
                            slotToEquipTo = amuletSlot2;
                            PlayerManager.Instance.equippedAmulet2.RemoveItem(1);
                            PlayerManager.Instance.equippedAmulet2.AddItem(selectedSlot.Item);
                            break;
                        case 3:
                            slotToEquipTo = amuletSlot3;
                            PlayerManager.Instance.equippedAmulet3.RemoveItem(1);
                            PlayerManager.Instance.equippedAmulet3.AddItem(selectedSlot.Item);
                            break;
                    }
                }
                if (!slotToEquipTo.IsEmpty && slotToEquipTo.Item is Amulet am) { am.OnUnequip(); }
                break;
            default: Debug.Log("Attempt to assign to searched slot failed. Unknown item type."); break;
        }

        if (!slotToEquipTo.IsEmpty && slotToEquipTo.Item != selectedSlot.Item)
        {
            PlayerManager.Instance.Inventory.AddItem(slotToEquipTo.Item, slotToEquipTo.Quantity);

            slotToEquipTo.RemoveItem(slotToEquipTo.Quantity);
        }

        slotToEquipTo.AddItem(selectedSlot.Item, selectedSlot.Quantity);

        if (slotToEquipTo.Item is Amulet tmp1) { tmp1.OnEquip(); }

        PlayerManager.Instance.Inventory.RemoveItem(selectedSlot.Item, selectedSlot.Quantity);
        selectedSlot.RemoveItem(selectedSlot.Quantity);
        PlayerManager.Instance.ResetStats();
        UpdateMenu();
    }
}

