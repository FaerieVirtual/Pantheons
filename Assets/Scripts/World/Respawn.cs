using UnityEngine;

public class Respawn : MonoBehaviour, IInteractible
{
    public bool CanInteract { get; set; }

    public void Interaction()
    {
        Level CurrentLevel = (Level)GameManager.Instance.machine.CurrentState;
        GameManager.Instance.RespawnSceneID = CurrentLevel.LevelID;
        foreach (var item in GameManager.Instance.levelManager.levels.Values)
        {
            if (item.HasFlag("RespawnLevel")) item.RemoveFlag("RespawnLevel");
        }
        CurrentLevel.SetFlag("RespawnLevel");

        PlayerManager.Instance.ResetPlayer();
    }
}

