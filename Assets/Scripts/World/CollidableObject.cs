using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            CollisionInteraction();
        }
    }
    public virtual void CollisionInteraction() { }
}

