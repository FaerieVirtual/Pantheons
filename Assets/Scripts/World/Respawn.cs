using System.Linq;
using UnityEngine;

public class Respawn : InteractibleObject
{
    public Collider2D Collider { get; set; }
    private void Update()
    {
        if (GameManager.Instance.machine.CurrentState == GameManager.Instance.LevelManager.levels.Values.FirstOrDefault(level => level.HasFlag("RespawnLevel")) && !GetFire().activeSelf)
        {
            GetFire().SetActive(true);
        }
        if (GameManager.Instance.machine.CurrentState != GameManager.Instance.LevelManager.levels.Values.FirstOrDefault(level => level.HasFlag("RespawnLevel")) && GetFire().activeSelf)
        {
            GetFire().SetActive(false);
        }
    }

    public override void Interaction()
    {
        if (CanInteract)
        {
            Level CurrentLevel = (Level)GameManager.Instance.machine.CurrentState;
            foreach (var item in GameManager.Instance.LevelManager.levels.Values)
            {
                if (item.HasFlag("RespawnLevel")) item.RemoveFlag("RespawnLevel");
            }
            CurrentLevel.SetFlag("RespawnLevel");

            PlayerManager.Instance.ResetPlayer();

            DataManager manager = GameManager.Instance.DataManager;
            manager.SaveFile(manager.Save(), manager.SaveIndex);

        }
    }

    public GameObject GetFire()
    {
        return transform.GetChild(1).gameObject;
    }
    public Vector2 GetRespawnpoint()
    {
        return transform.GetChild(0).position;
    }
}

