using UnityEngine;

[CreateAssetMenu(fileName = "Amulet (B)", menuName = "Items/Amulets/Amulet (B)")]
public class BoostAmulet : Amulet
{
    public override void OnEquip()
    {
        PlayerManager.Instance.ActivateEffect(Effect);
        
    }
    public override void OnUnequip()
    {
        PlayerEffect tmp = new(Effect.type, -Effect.Value);
        PlayerManager.Instance.ActivateEffect(tmp);
    }
}
