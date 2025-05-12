public class Amulet : ItemBase
{
    public PlayerEffect Effect;

    public Amulet()
    {
        Type = ItemType.Amulet;
    }
    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }
}

