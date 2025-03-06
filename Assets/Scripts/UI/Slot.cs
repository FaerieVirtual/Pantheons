using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public IItem Item;
    public int Quantity;
    public bool IsEmpty => Item == null || Quantity == 0;
    public bool isLocked;
    public string SlotID;
    public Button Button;
    private InventoryMenu menu;
    private TextMeshProUGUI quantityText;

    private void OnEnable()
    {
        Button = GetComponentInChildren<Button>(true);
        menu = FindObjectOfType<InventoryMenu>(true);
        quantityText = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();//GetComponentInChildren<TextMeshProUGUI>(true);
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

    public void AddItem(IItem item, int amount = 1)
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
    public IItem GetItem() => Item;

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