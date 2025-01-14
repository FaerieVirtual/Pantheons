
public interface ICharm 
{
    public bool equipped { get; set; }
    public bool equippable { get; set; }
    public int slotsRequired { get; set; }
    //public int slot { get; set; }

    public int hpAdd { get; set; }
    public int defAdd { get; set; }
    public int manaAdd { get; set; }
    public int speedAdd { get; set; }

    public bool Equip();
    public bool Unequip();
}

