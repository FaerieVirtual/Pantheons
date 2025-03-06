using UnityEngine;
[CreateAssetMenu(fileName = "Health Potion", menuName = "Items/Consumable")]
public class HealthPotion : ConsumableItem
{
    private void OnEnable()
    {
        Name = "Minor Health Potion";
        Description = "This potent mixture mends bone and sinew. Shame it does nothing against soul's wounds.";
        Price = 50;
    }
    [SerializeField] public new Sprite ItemSprite;
    //protected HealthPotion() : base(
    //    "Minor Health Potion",
    //    "This potent mixture mends bone and sinew. Shame it does nothing against soul's wounds.",
    //    (Sprite)Resources.Load(@"..\..\..\Assets\Assets\Blood vial.png"),
    //    50)
    //{ }

    public int HealAmount { get; set; } = 1;
    public override void Consume()
    {
        PlayerManager.Instance.Heal(HealAmount);
        base.Consume();
    }
}
