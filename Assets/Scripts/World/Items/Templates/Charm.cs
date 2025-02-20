using UnityEngine;

public class Charm : IItem, ICharm
{
    public Charm()
    {
        Quantity = 1;
        Price = 0;
        equipped = false;
        equippable = false;
        slotsRequired = 1;

        hpAdd = 0;
        defAdd = 0;
        manaAdd = 0;
        speedAdd = 0;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public bool equipped { get; set; }
    public bool equippable { get; set; }
    public int slotsRequired { get; set; }
    public int hpAdd { get; set; }
    public int defAdd { get; set; }
    public int manaAdd { get; set; }
    public int speedAdd { get; set; }
    public Sprite ItemSprite { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool Consumable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public ItemType Type { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool Equip()
    {
        if (equipped || !equippable) return false; //visual => fail to equip

        PlayerManager player = PlayerManager.Instance;
        //player.hpAdd += hpAdd;
        //player.defAdd += defAdd;
        //player.manaAdd += manaAdd;
        //player.speedAdd += speedAdd;
        equipped = true;

        //player.ResetBoosts();
        return true;
    }
    public bool Unequip() 
    {
        if (!equipped || !equippable) return false; //should not happen, but better safe than sorry
        PlayerManager player = PlayerManager.Instance;
        //player.hpAdd -= hpAdd;
        //player.defAdd -= defAdd;
        //player.manaAdd -= manaAdd;
        //player.speedAdd -= speedAdd;
        equipped = false;

        //player.ResetBoosts();
        return true;
    }
}

