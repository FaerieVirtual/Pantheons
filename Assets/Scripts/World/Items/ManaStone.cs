using UnityEngine;
[CreateAssetMenu(fileName = "Mana Stone", menuName = "Items/Consumable/Mana Stone")]
public class ManaStone : ConsumableItem
{
    public int ManaAmount;
    public override void Consume()
    {
        PlayerManager.Instance.AddMana(ManaAmount);
        base.Consume();
    }
}

