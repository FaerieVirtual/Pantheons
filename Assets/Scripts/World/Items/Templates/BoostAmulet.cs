using UnityEngine;

[CreateAssetMenu(fileName = "Amulet (B)", menuName = "Items/Amulets/Amulet (B)")]
public class BoostAmulet : Amulet
{
    public int maxHpBoost;
    public int maxManaBoost;
    public int gatherGoldBoost;
    public int gatherManaBoost;
    public int damageBoost;

    public override void OnEquip()
    {
        Debug.Log($"equipping: {this}");

        if (maxHpBoost > 0) PlayerManager.Instance.boostedMaxHp += maxHpBoost; Debug.Log($"adding hp boost: {maxHpBoost}, boosted hp {PlayerManager.Instance.boostedMaxHp}");
        if (maxManaBoost > 0) PlayerManager.Instance.boostedMaxMana += maxManaBoost; Debug.Log($"adding mana boost: {maxManaBoost}, boosted mana {PlayerManager.Instance.boostedMaxMana}");
        if (damageBoost > 0) PlayerManager.Instance.boostedDamage += damageBoost; Debug.Log($"adding damage boost: {damageBoost}, boosted dmaage {PlayerManager.Instance.boostedDamage}");
    }
    public override void OnUnequip()
    {
        Debug.Log($"unequipping: {this}");

        if (maxHpBoost > 0) PlayerManager.Instance.boostedMaxHp -= maxHpBoost; Debug.Log($"removing hp boost: {maxHpBoost}, boosted hp {PlayerManager.Instance.boostedMaxHp}");
        if (maxManaBoost > 0) PlayerManager.Instance.boostedMaxMana -= maxManaBoost; Debug.Log($"removing mana boost: {maxManaBoost}, boosted mana {PlayerManager.Instance.boostedMaxMana}");
        if (damageBoost > 0) PlayerManager.Instance.boostedDamage -= damageBoost; Debug.Log($"removing damage boost: {damageBoost}, boosted dmaagae {PlayerManager.Instance.boostedDamage}");
    }
}
