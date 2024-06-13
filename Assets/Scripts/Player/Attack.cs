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
            Debug.Log("detekuji nepøítele");
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            Debug.Log("hledám damageable" + damageable);
            Rigidbody2D otherRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("hledám rigidbody" + otherRigidBody);

            damageable.TakeDamage(Player.damage);
            Debug.Log("dávám damage" + Player.damage);
            Vector2 bounce = (otherRigidBody.transform.position - transform.position).normalized;
            Debug.Log("dávám bounce" + bounce);
            otherRigidBody.AddForce(bounce * 12, ForceMode2D.Impulse);
        }
    }
}
