using UnityEngine;
public class LevelConnector : MonoBehaviour
{
    public string nextLevelID;

    public string ConnectorID;
    public string SisterConnectorID;

    public bool Triggered;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            Triggered = true;
            GameManager.Instance.machine.ChangeState(GameManager.Instance.LevelManager.GetLevelByID(nextLevelID));
        }
    }

    public Vector2 GetSpawnpoint() 
    { 
        return transform.GetChild(0).transform.position;
    }
}

