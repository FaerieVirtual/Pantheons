using UnityEngine;

[CreateAssetMenu(fileName = "Invincibility Amulet", menuName = "Items/Amulets/Invincibility Amulet")]
public class InvincibilityAmulet : AbilityAmulet
{
    public int ManaCost;
    public float Duration;
    public override void ActivateAbility()
    {
        if (PlayerManager.Instance.Mana < ManaCost) return;
        
        PlayerManager.Instance.Mana -= ManaCost;
        PlayerManager.Instance.SetInvincibility(Duration);
    }
}
