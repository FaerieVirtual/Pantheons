using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType
{
    EquipSlot,
    UnequipSlot,
    LockedSlot
}
public class GraphicalSlot : MonoBehaviour
{
    public ItemBase Item;
    public int Quantity;
    public SlotType Type;
    public bool IsEmpty => Item == null || Quantity == 0;
    public Button Button;
    private ItemManagingMenu menu;
    public TextMeshProUGUI quantityText;

    private void OnEnable()
    {
        GameObject parent = transform.parent.gameObject;
        while (menu == null) 
        {
            if (!parent.TryGetComponent(out menu)) parent = parent.transform.parent.gameObject;
        }
        if (Type == SlotType.LockedSlot) { Button.interactable = false; }
        else
        {
            Button.interactable = true;
            Button.onClick.AddListener(() => menu.SelectSlot(this)); 
        }
        UpdateSlot();
    }
    public void UpdateSlot()
    {
        if (Quantity > 1) { quantityText.text = Quantity.ToString(); }
        else { quantityText.text = ""; }
        if (IsEmpty) { Button.gameObject.GetComponent<Image>().color = Color.clear; }
        else
        {
            Button.gameObject.GetComponent<Image>().color = Color.white;
            Button.gameObject.GetComponent<Image>().sprite = Item.ItemSprite;
        }
        if (IsEmpty) { Button.interactable = false; }
        else { Button.interactable = true; }
    }

    public void AddItem(ItemBase item, int amount = 1)
    {
        if (Item == null)
        {
            Item = item;
            Quantity = amount;
        }
        else if (Item.Name == item.Name)
        {
            Quantity += amount;
        };
        UpdateSlot();
    }
    public void RemoveItem(int amount)
    {
        if (Item != null)
        {
            Quantity -= amount;
            if (Quantity <= 0)
            {
                Item = null;
            }
        }
        UpdateSlot();
    }
    public ItemBase GetItem() => Item;
}

public class AbstractSlot
{
    public ItemBase Item;
    public int Quantity;
    public bool IsEmpty => Item == null || Quantity == 0;

    public void AddItem(ItemBase item, int amount = 1)
    {
        if (Item == null)
        {
            Item = item;
            Quantity = amount;
        }
        else if (Item.Name == item.Name)
        {
            Quantity += amount;
        };
    }
    public void RemoveItem(int amount)
    {
        if (Item != null)
        {
            Quantity -= amount;
            if (Quantity <= 0)
            {
                Item = null;
            }
        }
    }
}

public class SaveSlot
{
    public string ItemPath;
    public int Quantity;
}
