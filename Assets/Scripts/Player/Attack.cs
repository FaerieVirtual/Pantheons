using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            Rigidbody2D otherRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            damageable.TakeDamage(PlayerManager.damage);
            Vector2 bounce = (otherRigidBody.transform.position - transform.position).normalized;
            otherRigidBody.AddForce(bounce * 12, ForceMode2D.Impulse);
        }
    }
}
