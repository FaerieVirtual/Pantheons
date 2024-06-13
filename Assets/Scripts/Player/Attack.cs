using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("kolize");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("detekuji nep��tele");
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            Debug.Log("hled�m damageable" + damageable);
            Rigidbody2D otherRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("hled�m rigidbody" + otherRigidBody);

            damageable.TakeDamage(Player.damage);
            Debug.Log("d�v�m damage" + Player.damage);
            Vector2 bounce = (otherRigidBody.transform.position - transform.position).normalized;
            Debug.Log("d�v�m bounce" + bounce);
            otherRigidBody.AddForce(bounce * 12, ForceMode2D.Impulse);
        }
    }
}
