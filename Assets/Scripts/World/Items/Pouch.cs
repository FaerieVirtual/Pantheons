
using UnityEngine;

public class Pouch : ConsumableItem
{
    private void OnEnable()
    {
        Name = "Coin Pouch";
        Description = "Filled with jingles of aged metal pieces. Grants 50 coins on opening. Is not lost in the case of death (unlike other money).";
        //ItemSprite = 
        Price = 60;
    }
    //public Pouch(Sprite itemSprite) : base("Coin Pouch", "Filled with jingles of aged metal pieces. Grants 50 coins on opening. Is not lost in the case of death (unlike other money).", itemSprite, 60)
    //{
    //}
    public override void Consume()
    {
        PlayerManager.Instance.Gold += 50;
    }
}

