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

}

