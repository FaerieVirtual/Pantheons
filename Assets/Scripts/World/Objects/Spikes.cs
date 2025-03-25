using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : CollidableObject
{
    public Transform ReturnPoint;
    public override void CollisionInteraction()
    {
        PlayerManager.Instance.TakeDamage(1);
        PlayerManager.Instance.Stop();
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.CompareTag("ReturnPoint")) 
            { 
                float distance = Vector2.Distance(obj.transform.position, PlayerManager.Instance.transform.position);
                if (distance < Vector2.Distance(ReturnPoint.position, PlayerManager.Instance.transform.position)) ReturnPoint = obj.transform;
            }
        }
        PlayerManager.Instance.transform.position = ReturnPoint.position;
    }
}

