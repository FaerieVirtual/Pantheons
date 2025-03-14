using UnityEngine;

public class Wolf : EnemyBase
{
    private void FixedUpdate()
    {
        if (RigidBody.velocity.y > -1) RigidBody.velocity = new(RigidBody.velocity.x, Mathf.MoveTowards(RigidBody.velocity.y, -1, 4));
    }

}
