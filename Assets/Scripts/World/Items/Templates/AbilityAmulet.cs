using UnityEngine;

public class AbilityAmulet : Amulet
{
    public float chargeTime;
    public virtual void ActivateAbility() { }
    public override void OnEquip()
    {
        Debug.Log($"equipping {this}");

        if (PlayerManager.Instance.equippedAmulet1.Item == this) { PlayerManager.Instance.amulet1ChargeTime = chargeTime; Debug.Log($"changing amulet1 {PlayerManager.Instance.amulet1ChargeTime} to {chargeTime}"); }
        else if (PlayerManager.Instance.equippedAmulet2.Item == this){ PlayerManager.Instance.amulet2ChargeTime = chargeTime; Debug.Log($"changing amulet2 {PlayerManager.Instance.amulet2ChargeTime} to {chargeTime}"); }
        else if (PlayerManager.Instance.equippedAmulet3.Item == this) {PlayerManager.Instance.amulet3ChargeTime = chargeTime; Debug.Log($"changing amulet3 {PlayerManager.Instance.amulet3ChargeTime} to {chargeTime}");}
    }

    public override void OnUnequip()
    {
        Debug.Log($"equipping {this}");

        if (PlayerManager.Instance.equippedAmulet1.Item == this) {PlayerManager.Instance.amulet1ChargeTime = 0; Debug.Log($"changing amulet1 {PlayerManager.Instance.amulet1ChargeTime} to 0"); }
        else if (PlayerManager.Instance.equippedAmulet2.Item == this) {PlayerManager.Instance.amulet2ChargeTime = 0; Debug.Log($"changing amulet2 {PlayerManager.Instance.amulet1ChargeTime} to 0"); }
        else if (PlayerManager.Instance.equippedAmulet3.Item == this) {PlayerManager.Instance.amulet3ChargeTime = 0; Debug.Log($"changing amulet3 {PlayerManager.Instance.amulet1ChargeTime} to 0"); }

    }
}

