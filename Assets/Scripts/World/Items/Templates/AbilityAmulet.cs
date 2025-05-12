using UnityEngine;

public class AbilityAmulet : Amulet
{
    public float chargeTime;

    public override void OnEquip()
    {
        if (PlayerManager.Instance.equippedAmulet1.Item == this) PlayerManager.Instance.amulet1ChargeTime = chargeTime; 
        else if (PlayerManager.Instance.equippedAmulet2.Item == this) PlayerManager.Instance.amulet2ChargeTime = chargeTime;
        else if (PlayerManager.Instance.equippedAmulet3.Item == this) PlayerManager.Instance.amulet3ChargeTime = chargeTime;
    }

    public override void OnUnequip()
    {
        if (PlayerManager.Instance.equippedAmulet1.Item == this) PlayerManager.Instance.amulet1ChargeTime = 0;
        else if (PlayerManager.Instance.equippedAmulet2.Item == this) PlayerManager.Instance.amulet2ChargeTime = 0;
        else if (PlayerManager.Instance.equippedAmulet3.Item == this) PlayerManager.Instance.amulet3ChargeTime = 0;

    }
}

