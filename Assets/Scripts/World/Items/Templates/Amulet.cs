public class Amulet : ItemBase
{
    public Amulet()
    {
        Type = ItemType.Amulet;
    }
    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }
}

