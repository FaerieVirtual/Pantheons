using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();
    public Animator Animator => GetComponent<Animator>();
    private SceneLoader Loader => gameObject.AddComponent<SceneLoader>();
    public static PlayerManager instance;
    public AudioManager Audio;

    PlayerStatemachine Machine { get; set; } = new PlayerStatemachine();

    #region Events
    public UnityEvent playerRespawn;
    public UnityEvent playerHitDef;
    public UnityEvent playerAttack;
    #endregion
    #region States
    PlayerRunningState runningState;
    PlayerFallState fallState;
    PlayerIdleState idleState;
    PlayerJumpState jumpState;
    PlayerHurtState hurtState;
    PlayerDeathState deathState;
    #endregion

    #region General
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Audio = AudioManager.instance;
        #region States
        runningState = new PlayerRunningState(this, Machine);
        fallState = new PlayerFallState(this, Machine);
        jumpState = new PlayerJumpState(this, Machine);
        idleState = new PlayerIdleState(this, Machine);
        deathState = new PlayerDeathState(this, Machine);
        hurtState = new PlayerHurtState(this, Machine);
        #endregion

        ResetPlayer();
        RigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        Machine.Init(idleState);
        #region  listeners
        UI ui = UI.instance;
        GameManager game = GameManager.instance;

        playerRespawn.AddListener(ui.OnPlayerRespawn);

        playerRespawn.AddListener(game.OnPlayerRespawn);
        playerHitDef.AddListener(game.OnPlayerHitDef);

        playerRespawn.AddListener(Loader.OnPlayerRespawn);

        //playerAttack.AddListener(Audio.OnPlayerAttack);
        #endregion
    }
    private void Update()
    {
        Machine.currentState.Update();
        if (invincible) { Invincibility(); }
        if (!alive) { movementDisable = true; }
        CheckForAttack();
    }
    private void FixedUpdate()
    {
        Machine.currentState.PhysicsUpdate();
        if (!movementDisable)
        {
            Move();
            Jump();
            Fall();
        } 
        HealthChecks();
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

    public bool invincible;
    private float invincibleTimer;
    public float invincibleDuration;

    private int hpDisplayed;
    public Image[] hpImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int respawnPointScene = 2;

    public void Die()
    {
        Machine.ChangeState(deathState);    
    }
    public void TakeDamage(int damage)
    {
        switch (invincible)
        {
            case true:
                Animator.Play("Hurt NoEffect");
                break;
            case false:
                if (def == 0)
                {
                    hp -= damage;
                    Machine.ChangeState(hurtState);
                }
                if (def != 0)
                {
                    def -= damage;
                    playerHitDef.Invoke();
                    Animator.Play("Hurt NoEffect");
                }
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
    private void HealthChecks()
    {
        CheckForHpGraphics();
        CheckForMaxValues();

        void CheckForHpGraphics()
        {
            for (int i = 0; i < hpImages.Length; i++)
            {
                if (i < hp) { hpImages[i].sprite = fullHeart; }
                else { hpImages[i].sprite = emptyHeart; }

                if (i < hpDisplayed) { hpImages[i].enabled = true; }
                else { hpImages[i].enabled = false; }
            }
        }
        void CheckForMaxValues()
        {
            if (hp > maxHp) { hp = maxHp; }
            hpDisplayed = maxHp;
            if (def > maxDef) { def = maxDef; }
        }
    }
    public void Respawn()
    {
        ResetPlayer();
        playerRespawn.Invoke();
        if (FindObjectOfType<Respawn>() != null) { FindObjectOfType<Respawn>().activated = true; }
    }

    #endregion

    #region Movement
    private LayerMask ground => LayerMask.GetMask("Ground");
    private Transform standCheck => transform.GetChild(0);
    public float speed;
    public float jumpForce;
    public bool movementDisable = false;

    public bool IsStanding => Physics2D.OverlapCircle(standCheck.position, 0.5f, ground);

    public void Move()
    {
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && IsStanding && Machine.currentState != runningState)
        {
            Machine.ChangeState(runningState);
        }
        if (!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && Machine.currentState == runningState) { Machine.ChangeState(idleState);  }
    }
    public void Jump()
    {
        if ((IsStanding || RigidBody.velocity.y > 0) && Input.GetKey(KeyCode.Space) && Machine.currentState != jumpState)
        {
            Machine.ChangeState(jumpState);
        }
        if (IsStanding && Machine.currentState == jumpState && !Input.GetKey(KeyCode.Space)) { Machine.ChangeState(idleState); }
    }
    public void Fall()
    {
        if (RigidBody.velocity.y < 1 && Machine.currentState != fallState && !IsStanding)
        {
            Machine.ChangeState(fallState);
        }
        if (IsStanding && Machine.currentState == fallState) { Machine.ChangeState(idleState); }
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
            attacking = true;
            Attack();
        }
        if (attacking)
        {
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
        Animator.Play("Attack");
        //Audio.Play("Attack");
        transform.GetChild(1).gameObject.SetActive(attacking);
    }


    #endregion

    #region Magic
    public float favor;
    public float mAtk;
    #endregion
}
