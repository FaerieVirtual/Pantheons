using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            CollisionInteraction();
        }
    }
    public virtual void CollisionInteraction() { }
}

