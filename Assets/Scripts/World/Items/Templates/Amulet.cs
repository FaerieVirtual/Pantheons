public class Amulet : ItemBase
{
    public Amulet()
    {
        Type = ItemType.Equipment;
    }
    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }
}

