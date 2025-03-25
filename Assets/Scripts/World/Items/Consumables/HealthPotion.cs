using UnityEngine;
[CreateAssetMenu(fileName = "Health Potion", menuName = "Items/Consumable/Health Potion")]
public class HealthPotion : ConsumableItem
{
    public int HealAmount; 
    public override void Consume()
    {
        PlayerManager.Instance.Heal(HealAmount);
        base.Consume();
    }
}
