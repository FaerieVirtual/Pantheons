public class Coin : CollidableObject
{
    public int CoinAmount;

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
