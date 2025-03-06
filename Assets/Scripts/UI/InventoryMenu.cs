using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public List<Slot> slots;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI CoinCount;

    public Slot DisplaySlot;
    private Slot selectedSlot;
    public Slot swordSlot;
    public Slot[] equipmentSlots = new Slot[3];
    public Slot consumableSlot;

    private void Start()
    {
        GetComponent<Slot>().enabled = false;
    }

    private void Update()
    {
        if (CoinCount != null) { CoinCount.text = PlayerManager.Instance.Gold.ToString(); }
    }

    public void UpdateMenu()
    {
        foreach (Slot slot in slots)
        {
            slot.RemoveItem(slot.Quantity);
        }

        if (!PlayerManager.Instance.Inventory.IsEmpty)
        {
            List<Slot> playerInventory = PlayerManager.Instance.Inventory.GetAllItems();
            for (int i = 0; i < playerInventory.Count; i++)
            {
                slots[i].AddItem(playerInventory[i].GetItem());
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = slots[i].Item.ItemSprite;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = slots[i].Quantity.ToString();
            }
        }
        swordSlot.RemoveItem(swordSlot.Quantity);
        swordSlot.AddItem(PlayerManager.Instance.equippedWeapon, 1);

        consumableSlot.RemoveItem(consumableSlot.Quantity);
        consumableSlot.AddItem(PlayerManager.Instance.equippedConsumable, PlayerManager.Instance.ConsumableItemQuantity);

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].RemoveItem(equipmentSlots[i].Quantity);
            equipmentSlots[i].AddItem(PlayerManager.Instance.amulets[i], 1);
        }
        if (selectedSlot == null) 
        {
            DisplaySlot.Button.image.color = Color.clear;
            itemName.text = "";
            description.text = "";
        } 

        itemName.ForceMeshUpdate();
        description.ForceMeshUpdate();
    }

    public void SelectSlot(Slot slot)
    {
        //Debug.Log($"slot: {selectedSlot}, empty: {selectedSlot.IsEmpty}, item: {selectedSlot.Item}, q: {selectedSlot.Quantity}");
        if (selectedSlot != null)
        {
            selectedSlot.Button.onClick.RemoveAllListeners();
            Slot tmp = selectedSlot;
            selectedSlot.Button.onClick.AddListener(() => SelectSlot(tmp));
        }
        DisplaySlot.RemoveItem(DisplaySlot.Quantity);

        description.text = slot.Item.Description;
        itemName.text = slot.Item.Name;
        DisplaySlot.AddItem(slot.Item);

        selectedSlot = slot;

        //slot.Button.onClick.RemoveAllListeners();
        //if (slot.SlotID == "Inventory")
        //{
        //    slot.Button.onClick.AddListener(EquipItem);
        //}
        //else
        //{
        //    slot.Button.onClick.AddListener(UnequipItem);
        //}
    }

    public void EquipItem()
    {
        if (selectedSlot == null || selectedSlot.IsEmpty) return;
        Slot slotToEquipTo = null;
        switch (selectedSlot.Item.Type)
        {
            case ItemType.Sword:
                slotToEquipTo = swordSlot;
                PlayerManager.Instance.equippedWeapon = (WeaponItem)selectedSlot.Item;
                break;
            case ItemType.Consumable:
                slotToEquipTo = consumableSlot;
                PlayerManager.Instance.equippedConsumable = (ConsumableItem)selectedSlot.Item;
                PlayerManager.Instance.ConsumableItemQuantity = selectedSlot.Quantity;
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
        Debug.Log($"slot: {selectedSlot}, item: {selectedSlot.Item}, q: {selectedSlot.Quantity}");
        PlayerManager.Instance.Inventory.RemoveItem(selectedSlot.Item, selectedSlot.Quantity);
        selectedSlot.RemoveItem(selectedSlot.Quantity);
        Debug.Log($"followup check: slot: {selectedSlot}, item: {selectedSlot.Item}, q: {selectedSlot.Quantity}");

        UpdateMenu();
    }
    //public void UnequipItem()
    //{
    //    Slot slot = selectedSlot;
    //    Debug.Log(slot);
    //    PlayerManager.Instance.Inventory.AddItem(slot.Item, slot.Quantity);

    //    switch (slot.SlotID)
    //    {
    //        case "SwordSlot": swordSlot.RemoveItem(swordSlot.Quantity); break;
    //        case "ConsumableSlot": consumableSlot.RemoveItem(consumableSlot.Quantity); break;
    //        case "AmuletSlot1": equipmentSlots[0].RemoveItem(equipmentSlots[0].Quantity); break;
    //        case "AmuletSlot2": equipmentSlots[1].RemoveItem(equipmentSlots[1].Quantity); break;
    //        case "AmuletSlot3": equipmentSlots[2].RemoveItem(equipmentSlots[2].Quantity); break;
    //    }
    //    slot.RemoveItem(slot.Quantity);
    //    Debug.Log($"removing {slot.Item} from {slot} {slot.Quantity} times");
    //    UpdateMenu();
    //}
}

