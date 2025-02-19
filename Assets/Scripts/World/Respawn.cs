using System.Linq;
using UnityEngine;

public class Respawn : MonoBehaviour, IInteractible
{
    public bool CanInteract { get; set; } = true;
    public Collider2D Collider { get; set; }
    private void Update()
    {
        if (GameManager.Instance.machine.CurrentState == GameManager.Instance.levelManager.levels.Values.FirstOrDefault(level => level.HasFlag("RespawnLevel")))
        {
            GetFire().SetActive(true);
        }
        else
        {
            GetFire().SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            collision.gameObject.GetComponent<PlayerManager>().Interact.AddListener(Interaction);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            collision.gameObject.GetComponent<PlayerManager>().Interact.RemoveListener(Interaction);
        }
    }
    public void Interaction()
    {
        if (CanInteract)
        {
            Level CurrentLevel = (Level)GameManager.Instance.machine.CurrentState;
            foreach (var item in GameManager.Instance.levelManager.levels.Values)
            {
                if (item.HasFlag("RespawnLevel")) item.RemoveFlag("RespawnLevel");
            }
            CurrentLevel.SetFlag("RespawnLevel");

            PlayerManager.Instance.ResetPlayer();
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

