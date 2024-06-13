using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable, IMoveable
{
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();
    private Animator animator => GetComponent<Animator>();
    private PausedState paused;
    public static Player instance;

    #region General
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        if (instance != null) 
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        hp = maxHp;
        DontDestroyOnLoad(gameObject);
        RigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        standCheck = transform.GetChild(0);
    }
    private void Update()
    {
        if (invincible) { Invincibility(); }
        if (!alive) { movementDisable = true; }
        if (slowdown) { Slowdown(); }
        Game game = FindAnyObjectByType<Game>();
        CheckForAttack();
    }
    private void FixedUpdate()
    {
        if (!movementDisable)
        {
            Move();
            Jump();
            Fall();
        }
        CheckForHpGraphics();
        CheckForMaxValues();
    }

    public void ResetPlayer()
    {
        alive = true;
        hp = maxHp;
        invincible = false;
        RigidBody.velocity = Vector3.zero;
    }
    #endregion

    #region Health
    public int maxHp;
    public int hp;
    public int def;
    public int maxDef;
    public bool alive = true;

    private bool invincible;
    private float invincibleTimer;
    public float invincibleDuration;

    private float slowdownTimer;
    private float slowdownDuration = 1f;
    private bool slowdown = false;

    private int hpDisplayed;
    public Image[] hpImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int respawnPointScene = -1;
    private RespawnMenu respawnMenu;

    public void Die()
    {
        IEnumerator AnimationAndTimestop()
        {
            animator.Play("Death");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Time.timeScale = 0;
        }
        RigidBody.velocity = Vector3.zero;
        StartCoroutine(AnimationAndTimestop());
        respawnMenu = Object.FindObjectOfType<RespawnMenu>(true);
        respawnMenu.gameObject.SetActive(true);
        alive = false;
    }
    public void TakeDamage(int damage)
    {
        switch (invincible)
        {
            case true:
                animator.Play("Hurt NoEffect");
                break;
            case false:
                if (def == 0) { hp -= damage; }
                if (def != 0) { def -= damage; }
                invincible = true;
                animator.Play("Hurt");
                slowdown = true;
                break;
        }
        if (hp <= 0) { Die(); }
    }
    public void Invincibility()
    {
        invincibleTimer += Time.deltaTime;
        if (invincibleTimer >= invincibleDuration)
        {
            invincible = false;
            invincibleTimer = 0f;
        }

    }
    private void Slowdown()
    {
        Time.timeScale = 0.3f;

        slowdownTimer += Time.unscaledDeltaTime;
        if (slowdownTimer >= slowdownDuration)
        {
            slowdown = false;
            Time.timeScale = 1.0f;
            slowdownTimer = 0f;
        }
    }
    private void CheckForHpGraphics()
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            if (i < hp) { hpImages[i].sprite = fullHeart; }
            else { hpImages[i].sprite = emptyHeart; }

            if (i < hpDisplayed) { hpImages[i].enabled = true; }
            else { hpImages[i].enabled = false; }
        }
    }
    private void CheckForMaxValues()
    {
        if (hp > maxHp) { hp = maxHp; }
        hpDisplayed = maxHp;
        if (def > maxDef) { def = maxDef; }
    }
    public void Respawn()
    {
        SceneLoader loader = gameObject.AddComponent<SceneLoader>();
        if (respawnPointScene != -1)
        {
            loader.nextSceneIndex = respawnPointScene;
            loader.mode = "respawn";
            loader.LoadScene();
        }

        ResetPlayer();
        if (FindObjectOfType<Respawn>() != null) { FindObjectOfType<Respawn>().activated = true; }
        Time.timeScale = 1f;
        respawnMenu.gameObject.SetActive(false);
    }

    #endregion

    #region Movement
    public LayerMask ground;
    private Transform standCheck;
    public float speed;

    public float jumpForce;
    private float maxJumpTime = 0.5f;
    private float jumpTimer = 0f;

    private float fallTimer = 0f;
    private float rampupTime = 3f;

    private bool movementDisable = false;
    bool IsStanding => Physics2D.OverlapCircle(standCheck.position, 0.5f, ground);

    public void Flip() { }
    public void Move()
    {
        bool isRunning = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180f, transform.rotation.z);
            RigidBody.velocity = new Vector2(-speed, RigidBody.velocity.y);
            isRunning = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0f, transform.rotation.z);
            RigidBody.velocity = new Vector2(speed, RigidBody.velocity.y);
            isRunning = true;
        }
        if (IsStanding && isRunning) { animator.Play("Run"); }
        if (RigidBody.velocity.x == 0 && RigidBody.velocity.y == 0 && !attacking) { animator.Play("Idle"); }
    }

    public void Jump()
    {
        bool canJump = false;

        if (IsStanding) { canJump = true; }

        if ((canJump || RigidBody.velocity.y > 0) && Input.GetKey(KeyCode.Space))
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > maxJumpTime) { jumpTimer = maxJumpTime; }
            if (jumpTimer < maxJumpTime)
            {
                RigidBody.velocity = new Vector2(RigidBody.velocity.x, jumpForce - (jumpTimer / maxJumpTime * 10f));
            }
            animator.Play("jump");
        }

        if (IsStanding) { jumpTimer = 0; }
        if (RigidBody.velocity.y < 0 && fallTimer < 0) { animator.Play("JumptoFall"); }
    }
    public void Fall()
    {
        if (RigidBody.velocity.y < 1)
        {
            fallTimer += Time.deltaTime;
            if (fallTimer > rampupTime && RigidBody.gravityScale < 50f)
            {
                RigidBody.gravityScale += 2;
                fallTimer = rampupTime - 0.1f;
            }
            if (fallTimer >= 1) { animator.Play("Fall"); }
        }
        if (RigidBody.velocity.y >= 0)
        {
            fallTimer = 0;
            RigidBody.gravityScale = 10;
        }
    }
    #endregion

    #region Attacking
    public GameObject AttackHitboxSide;
    private float attackTime = 0.25f;
    private float attackTimer;
    public static int damage = 1;
    private bool attacking;

    private void CheckForAttack()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("útoèím");
            attacking = true;
            Attack();
        }
        if (attacking)
        {
            Debug.Log("timer" + attackTimer);
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime)
            {
                attackTimer = 0;
                attacking = false;
                AttackHitboxSide.SetActive(attacking);
            }
        }
    }
    private void Attack()
    {
        animator.Play("Attack");
        AttackHitboxSide = transform.GetChild(1).gameObject;
        Debug.Log("´beru hitbox:" + AttackHitboxSide);
        AttackHitboxSide.SetActive(attacking);
        Debug.Log("zapínám");
    }


    #endregion

    #region Magic
    public float favor;
    public float mAtk;
    #endregion
}
