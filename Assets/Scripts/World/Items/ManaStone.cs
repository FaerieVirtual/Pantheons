using UnityEngine;

public class ManaStone : ConsumableItem
{
    private void OnEnable()
    {
        Name = "Mana Stone";
        Description = "This glowing rock resonates with power. Consume for 30 mana.";
        ItemSprite = (Sprite)Resources.Load(@"..\..\..\Assets\Assets\Blue crystal.png");
        Price = 20;
    }
    //public ManaStone() : base(
    //    "Mana Stone",
    //    "This glowing rock resonates with power. Consume for 30 mana.",
    //    (Sprite)Resources.Load(@"..\..\..\Assets\Assets\Blue crystal.png"),
    //    20)
    //{
    //}
    public override void Consume()
    {
        PlayerManager.Instance.Mana += 30;
        base.Consume();
    }
}

