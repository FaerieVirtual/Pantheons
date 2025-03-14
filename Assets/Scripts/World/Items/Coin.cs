using UnityEngine;

public class Coin : CollidableObject
{
    public int CoinAmount;
    private Rigidbody2D Rb => GetComponent<Rigidbody2D>();
    private void Start()
    {
        System.Random r = new();
        Vector2 vector = new(r.Next(-10, 11), 15);
        Rb.velocity = vector;
    }
    private void FixedUpdate()
    {
        Vector2 vector = new(Mathf.MoveTowards(Rb.velocity.x, 0, 1), Mathf.MoveTowards(Rb.velocity.y, -8, 1));
        Rb.velocity = vector;
    }
    public Coin(int coinAmount)
    {
        CoinAmount = coinAmount;
    }
    public override void CollisionInteraction()
    {
        PlayerManager.Instance.Gold += CoinAmount;
        Destroy(gameObject);
    }
}
