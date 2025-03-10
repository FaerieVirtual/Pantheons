using UnityEngine;

[CreateAssetMenu(fileName = "Healing Amulet", menuName = "Items/Amulets/Healing Amulet")]
public class HealingAmulet : AbilityAmulet
{
    public int HealAmount;
    public int ManaCost;
    public override void ActivateAbility()
    {
        if (PlayerManager.Instance.Hp >= PlayerManager.Instance.MaxHp || PlayerManager.Instance.Mana < ManaCost) return;

        PlayerManager.Instance.Heal(HealAmount);
        PlayerManager.Instance.Mana -= ManaCost;
    }

}
