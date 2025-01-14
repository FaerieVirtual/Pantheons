public class Charm : IItem, ICharm
{
    public Charm()
    {
        quantity = 1;
        price = 0;
        equipped = false;
        equippable = false;
        slotsRequired = 1;

        hpAdd = 0;
        defAdd = 0;
        manaAdd = 0;
        speedAdd = 0;
    }

    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int quantity { get; set; }
    public bool equipped { get; set; }
    public bool equippable { get; set; }
    public int slotsRequired { get; set; }
    public int hpAdd { get; set; }
    public int defAdd { get; set; }
    public int manaAdd { get; set; }
    public int speedAdd { get; set; }
    public bool Equip()
    {
        if (equipped || !equippable) return false; //visual => fail to equip

        PlayerManager player = PlayerManager.instance;
        player.hpAdd += hpAdd;
        player.defAdd += defAdd;
        player.manaAdd += manaAdd;
        player.speedAdd += speedAdd;
        equipped = true;

        player.ResetBoosts();
        return true;
    }
    public bool Unequip() 
    {
        if (!equipped || !equippable) return false; //should not happen, but better safe than sorry
        PlayerManager player = PlayerManager.instance;
        player.hpAdd -= hpAdd;
        player.defAdd -= defAdd;
        player.manaAdd -= manaAdd;
        player.speedAdd -= speedAdd;
        equipped = false;

        player.ResetBoosts();
        return true;
    }
}

