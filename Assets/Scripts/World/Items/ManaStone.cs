using UnityEngine;
[CreateAssetMenu(fileName = "Mana Stone", menuName = "Items/Consumable/Mana Stone")]
public class ManaStone : ConsumableItem
{
    [SerializeField] public int ManaAmount;
    public override void Consume()
    {
        PlayerManager.Instance.Mana += ManaAmount;
        base.Consume();
    }
}

