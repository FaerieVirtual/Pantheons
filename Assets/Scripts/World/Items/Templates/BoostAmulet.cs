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
        if (maxHpBoost > 0) PlayerManager.Instance.boostedMaxHp += maxHpBoost;
        if (maxManaBoost > 0) PlayerManager.Instance.boostedMaxMana += maxManaBoost;
        if (damageBoost > 0) PlayerManager.Instance.boostedDamage += damageBoost; 
    }
    public override void OnUnequip()
    {
        if (maxHpBoost > 0) PlayerManager.Instance.boostedMaxHp -= maxHpBoost;
        if (maxManaBoost > 0) PlayerManager.Instance.boostedMaxMana -= maxManaBoost;
        if (damageBoost > 0) PlayerManager.Instance.boostedDamage -= damageBoost;
    }
}
