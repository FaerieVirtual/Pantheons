using UnityEngine;
public class DamageableObject : MonoBehaviour, IDamageable
{
    public int Hp { get; set; }
    public int MaxHp { get; set; }

    public void ResetObject() 
    {
        Hp = MaxHp;
        gameObject.SetActive(true);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {

    }
}
