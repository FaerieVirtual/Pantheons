using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable, IMoveable
{
    EnemyStatemachine EnemyStatemachine { get; set; } = new EnemyStatemachine();
    IdleState idleState;
    DeathState deathState;

    #region Start/Updates
    void Start()
    {
        idleState = new IdleState(this, EnemyStatemachine);
        deathState = new DeathState(this, EnemyStatemachine);
        EnemyStatemachine.Init(idleState);

        Hp = MaxHp;
        ground = LayerMask.GetMask("Ground");
        groundCheck = transform.GetChild(2);
        wallCheck = transform.GetChild(3);
    }
    void Update()
    {
        EnemyStatemachine.currentState.Update();
    }
    void FixedUpdate()
    {
        EnemyStatemachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Health
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    //public bool Alive => hp > 0;

    public void Die()
    {
        EnemyStatemachine.ChangeState(deathState);
    }
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0) { Die(); }
    }
    #endregion

    #region Movement 
    public int Speed { get; set; }
    private Transform groundCheck;
    private Transform wallCheck;
    private LayerMask ground;
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();

    private bool facingRight = true;

    public void Move()
    {
        Vector2 forward = Vector2.zero;
        bool groundDetect = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        bool wallDetect = Physics2D.OverlapCircle(wallCheck.position, 0.1f, ground);

        if (!groundDetect) { Flip(); }
        if (wallDetect) { Flip(); }
        if (facingRight) { forward = new(Speed, RigidBody.velocity.y); }
        if (!facingRight) { forward = new(-Speed, RigidBody.velocity.y); }

        Animator animator = GetComponent<Animator>();
        animator.Play("Run");
        RigidBody.velocity = forward;
    }
    public void Flip()
    {
        switch (facingRight)
        {
            case true:
                transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180f, transform.rotation.z);
                facingRight = false;
                break;
            case false:
                transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0f, transform.rotation.z);
                facingRight = true;
                break;
        }
    }
    #endregion

    #region Collision
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
            Rigidbody2D otherRigidBody = playerObject.GetComponent<Rigidbody2D>();

            player.TakeDamage(1);
            Vector2 bounce = (otherRigidBody.transform.position - transform.position).normalized;
            otherRigidBody.AddForce(bounce * 12, ForceMode2D.Impulse);
        }
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            TakeDamage(1);
            Vector2 bounce = (collision.transform.position - transform.position).normalized;
            RigidBody.AddForce(bounce * 4, ForceMode2D.Impulse);

        }
    }
    #endregion
}
