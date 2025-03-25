using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Boost Amulet", menuName = "Items/Amulets/Damage Boost Amulet")]
public class DamageAmulet : AbilityAmulet
{
    public float Duration;
    public int DamageBoost;
    public int ManaCost;
    public override async void ActivateAbility()
    {
        if (PlayerManager.Instance.Mana < ManaCost) return;

        PlayerManager.Instance.boostedDamage += DamageBoost;
        PlayerManager.Instance.Mana -= ManaCost;
        PlayerManager.Instance.GetComponent<SpriteRenderer>().color = Color.red;

        await Task.Delay((int)(Duration * 1000));

        PlayerManager.Instance.GetComponent<SpriteRenderer>().color = Color.white;
        PlayerManager.Instance.boostedDamage -= DamageBoost;
    }
}
