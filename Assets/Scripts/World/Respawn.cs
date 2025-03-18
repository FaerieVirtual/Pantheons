using System.Linq;
using UnityEngine;

public class Respawn : InteractibleObject
{
    public Collider2D Collider { get; set; }
    private void Start()
    {
        UpdateFire();
    }
    public override void Interaction()
    {
        base.Interaction();
        if (CanInteract)
        {
            Level CurrentLevel = (Level)GameManager.Instance.Machine.CurrentState;
            foreach (var item in GameManager.Instance.LevelManager.levels.Values)
            {
                if (item.HasFlag("RespawnLevel")) item.RemoveFlag("RespawnLevel");
            }
            CurrentLevel.SetFlag("RespawnLevel");

            PlayerManager.Instance.ResetPlayer();

            DataManager manager = GameManager.Instance.DataManager;
            manager.SaveFile(manager.Save(), manager.SaveIndex);
            UpdateFire();
        }
    }

    public Vector2 GetRespawnpoint()
    {
        return transform.GetChild(0).position;
    }
    private void UpdateFire() 
    {
        GameObject fire = transform.GetChild(1).gameObject;

        if (GameManager.Instance.Machine.CurrentState == GameManager.Instance.LevelManager.levels.Values.FirstOrDefault(level => level.HasFlag("RespawnLevel")) && !fire.activeSelf)
        {
            fire.SetActive(true);
        } 
        else
        {
            fire.SetActive(false);
        }
    }
}

