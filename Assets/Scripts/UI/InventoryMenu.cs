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

    public Button EquipButton;
    public Button UnequipButton;
    public Animator PlayerSpriteAnimator;

    private void OnEnable()
    {
        if (TryGetComponent(out Canvas canvas) && canvas.worldCamera == null) 
        { 
            canvas.worldCamera = Camera.main;
        }
        SelectSlot(slots[1]);
        PlayerSpriteAnimator.StopPlayback(); 
        PlayerSpriteAnimator.Play("Idle");
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
        if (!slot.IsEmpty)
        {
            DisplaySlot.RemoveItem(DisplaySlot.Quantity);

            description.text = slot.Item.Description;
            itemName.text = slot.Item.Name;
            DisplaySlot.AddItem(slot.Item);
        }
        else
        {
            description.text = "";
            itemName.text = "";
            if (!DisplaySlot.IsEmpty) DisplaySlot.RemoveItem(DisplaySlot.Quantity);
        }
        selectedSlot = slot;

        if (selectedSlot.Type == SlotType.EquipSlot) 
        { 
            EquipButton.interactable = true; 
            UnequipButton.interactable = false; 
        }
        if (selectedSlot.Type == SlotType.UnequipSlot) 
        {
            EquipButton.interactable = false;
            UnequipButton.interactable = true;
        }
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
                PlayerManager.Instance.UpdateAttack();
                
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
                else return;

                if (!slotToEquipTo.IsEmpty && slotToEquipTo.Item is Amulet am) 
                { 
                    am.OnUnequip(); 
                    if (selectedSlot.Item is Amulet am2) { am2.OnEquip(); }
                }
                PlayerManager.Instance.UpdateAbilities();
                break;
            default: Debug.Log("Attempt to assign to searched slot failed. Unknown Items type."); break;
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

    public void UnequipItem() 
    {
        if (selectedSlot == null || selectedSlot.IsEmpty) return;

        PlayerManager.Instance.Inventory.AddItem(selectedSlot.Item, selectedSlot.Quantity);

        if (selectedSlot == swordSlot) PlayerManager.Instance.equippedWeapon.RemoveItem(PlayerManager.Instance.equippedWeapon.Quantity);
        if (selectedSlot == consumableSlot) PlayerManager.Instance.equippedConsumable.RemoveItem(PlayerManager.Instance.equippedConsumable.Quantity);
        if (selectedSlot == amuletSlot1) PlayerManager.Instance.equippedAmulet1.RemoveItem(PlayerManager.Instance.equippedAmulet1.Quantity);
        if (selectedSlot == amuletSlot2) PlayerManager.Instance.equippedAmulet2.RemoveItem(PlayerManager.Instance.equippedAmulet2.Quantity);
        if (selectedSlot == amuletSlot3) PlayerManager.Instance.equippedAmulet3.RemoveItem(PlayerManager.Instance.equippedAmulet3.Quantity);

        if (selectedSlot.Item is Amulet am) am.OnUnequip();
        selectedSlot.RemoveItem(selectedSlot.Quantity);
        UpdateMenu();
    }
}

