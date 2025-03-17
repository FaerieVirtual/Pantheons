using System.Threading.Tasks;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    public EnemyStatemachine Machine { get; set; } = new EnemyStatemachine();
    private DeathState deathState;
    public EnemyPatrolState patrolState;

    #region Start/Updates
    void Start()
    {
        deathState = new(this, Machine);
        patrolState = new(this, Machine);
        Machine.Init(patrolState);

        Hp = MaxHp;
        MoveDirection = Vector2.right;
    }
    private void Update()
    {
        Machine.currentState.Update();
    }
    private void FixedUpdate()
    {
        if (RigidBody.velocity.y > -1) RigidBody.velocity = new(RigidBody.velocity.x, Mathf.MoveTowards(RigidBody.velocity.y, -1, 4));
        Machine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Health
    public int MaxHp;
    public int Hp;
    public int CoinCount;

    public void Die()
    {
        Machine.ChangeState(deathState);
    }
    public async void TakeDamage(int damage)
    {
        TryGetComponent(out SpriteRenderer renderer);

        renderer.color = Color.red;
        Hp -= damage;

        await Task.Delay(500);

        renderer.color = Color.white;
        if (Hp <= 0) { Die(); }
        Vector2 bounce = (transform.position - PlayerManager.Instance.transform.position).normalized;
        RigidBody.velocity = bounce * PlayerManager.Instance.AttackPushbackForce;

    }
    #endregion

    #region Movement 
    public int Speed;
    public BoxCollider2D GroundCheck;
    public BoxCollider2D WallCheck;
    public Collider2D ChaseRadius;

    public LayerMask Ground => LayerMask.GetMask("Ground");
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();
    public int DamagePushbackForce;

    [HideInInspector] public Vector2 MoveDirection;
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        MoveDirection = (MoveDirection == Vector2.right) ? Vector3.right : Vector3.left;
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            player.TakeDamage(1);
            player.Pushback(transform, PlayerManager.Instance.DamagePushbackForce);
        }
    }
    #endregion

    public void SpawnCoin(int amount)
    {
        GameObject coinPrefab = Resources.Load<GameObject>("Items/Coin");
        for (int i = 0; i < amount; i++)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}
