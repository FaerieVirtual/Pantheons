using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon")]
public class WeaponItem : ItemBase
{

    public int damage;
    public float reach;

    public WeaponItem()
    {
        Type = ItemType.Sword;
    }

    public void OnEquip()
    {
        PlayerManager.Instance.baseDamage = damage;
        PlayerManager.Instance.attackReach = reach;
        Debug.Log($"equipping {Name}, damage: {PlayerManager.Instance.baseDamage}, reach: {PlayerManager.Instance.attackReach}");
    }
}

