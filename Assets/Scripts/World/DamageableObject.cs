using System.Threading.Tasks;
using UnityEngine;
public class DamageableObject : MonoBehaviour, IDamageable
{
    public int Hp;
    public int MaxHp;

    public void ResetObject()
    {
        Hp = MaxHp;
        gameObject.SetActive(true);
    }

    private void Start()
    {
        ResetObject();
    }
    public void Die()
    {
        GameObject CoinPrefab = Resources.Load<GameObject>("Items/Coin");
        Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public async void TakeDamage(int damage)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
        Hp -= damage;
        if ( Hp <= 0 ) { Die(); }
        await Task.Delay(400);
        renderer.color = Color.white;
    }
}
