using UnityEngine;

public class HealthPotion : ConsumableItem
{

    protected HealthPotion() : base("Minor Health Potion", ItemType.Consumable, "This potent mixture mends bone and sinew. Shame it does nothing against soul's wounds.", (Sprite)Resources.Load(@"..\..\..\Assets\Assets\Blood vial.png"), 50)
    {
    }

    public int HealAmount { get; set; } = 1;
    public override void Consume() 
    {
        PlayerManager.Instance.Heal(HealAmount);
        base.Consume();
    }
}
