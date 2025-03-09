using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicalSlot : MonoBehaviour
{
    public ItemBase Item;
    public int Quantity;
    public bool IsEmpty => Item == null || Quantity == 0;
    public bool isLocked;
    public Button Button;
    private ItemManagingMenu menu;
    public TextMeshProUGUI quantityText;

    private void OnEnable()
    {
        //Button = GetComponentInChildren<Button>(true);
        GameObject parent = transform.parent.gameObject;
        while (menu == null) 
        {
            if (!parent.TryGetComponent(out menu)) parent = parent.transform.parent.gameObject;
        }
        if (isLocked) { Button.interactable = false; }
        else
        { Button.onClick.AddListener(() => menu.SelectSlot(this)); }
    }
    private void Update()
    {
        if (Quantity > 1) { quantityText.text = Quantity.ToString(); }
        else { quantityText.text = ""; }
        if (IsEmpty) { Button.gameObject.GetComponent<Image>().color = Color.clear; }
        else
        {
            Button.gameObject.GetComponent<Image>().color = Color.white;
            Button.gameObject.GetComponent<Image>().sprite = Item.ItemSprite;
        }
        if (IsEmpty) isLocked = true;
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
    public ItemBase GetItem() => Item;

    //public void Clear()
    //{
    //    isLocked = false;
    //    Item = null;
    //    Quantity = 0;
    //}

    //public void OnItemDisplay()
    //{
    //    Button.onClick.RemoveAllListeners();
    //    if (SlotID == "Inventory")
    //    {
    //        Button.onClick.AddListener(menu.EquipItem);
    //    }
    //    else
    //    {
    //        Button.onClick.AddListener(menu.UnequipItem);
    //    }
    //}

    //public void StopItemDisplay()
    //{
    //    Button.onClick.RemoveAllListeners();
    //    Button.onClick.AddListener(() => menu.SelectSlot(gameObject.GetComponent<Slot>()));
    //}
}