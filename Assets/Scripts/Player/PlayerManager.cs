using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();
    public CapsuleCollider2D Collider => GetComponent<CapsuleCollider2D>();
    public Animator Animator => GetComponent<Animator>();
    private LevelManager Loader => gameObject.AddComponent<LevelManager>();
    public static PlayerManager instance;
    public AudioManager Audio;

    #region Events
    public UnityEvent playerRespawn;
    public UnityEvent playerHitDef;
    public UnityEvent playerAttack;
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

        ResetPlayer();
        RigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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
        GetInput();
        if (invincible) { Invincibility(); }
        CheckForAttack();
        time += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        HandleCollisions();
        HandleY();
        HandleX();
        HandleGravity();

        ApplyMovement();
        HealthChecks();
        AnimationParams();
    }

    public void ResetPlayer()
    {
        alive = true;
        hp = maxHp;
        invincible = false;
        RigidBody.velocity = Vector3.zero;
    }
    void AnimationParams()
    {
        Animator.SetFloat("Vertical", RigidBody.velocity.y);
        //Animator.SetFloat("Forward", RigidBody.velocity.x);
        Animator.SetBool("isStanding", IsStanding);
        Animator.SetInteger("Hp", hp);
        //Animator.SetBool("isRunning", RigidBody.velocity.x != 0);
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

    //FIX
    public void Die()
    {
    }
    //FIX
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
    //BROKEN -> TAKEDAMAGE
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
    [Header("INPUT")]
    public float VerticalDeadZoneThreshold = .1f;
    public float HorizontalDeadZoneThreshold = .1f;

    [Header("MOVEMENT")]
    public float MaxSpeed = 14;
    public float Acceleration = 140;
    public float GroundDeceleration = 80;
    public float AirDeceleration = 60;
    public float GroundingForce = -1f;
    public float GrounderDistance = .1f;

    [Header("JUMP")]
    public float JumpPower = 24;
    public float MaxJumpTime = 1.2f;
    public float MaxFallSpeed = 30;
    public float FallAcceleration = 40;
    public float JumpEndEarlyGravityModifier = 5f;
    public float CoyoteTime = .15f;
    public float JumpBuffer = .2f;
    public float ApexSpeedBonus = 2f;

    private bool JumpDown, JumpHeld;
    private int MoveDirection;

    private bool IsStanding, groundHit, ceilingHit;
    private bool IsJumping;
    private float time;
    private Vector2 tempVelocity;
    private LayerMask ground => LayerMask.GetMask("Ground");
    public bool movementDisable;

    private bool jumpToConsume;
    private bool bufferedJumpUsable;
    private bool endedJumpEarly;
    private bool coyoteUsable;
    private bool apexHit;
    private float timeJumpPressed;
    private float timeGroundLeft;
    private Vector2 previousVelocity;

    private bool HasBufferedJump => bufferedJumpUsable && time < timeJumpPressed + JumpBuffer;
    private bool HasCoyoteTime => coyoteUsable && !IsStanding && time < timeGroundLeft + CoyoteTime;

    void GetInput()
    {
        JumpDown = Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump");
        JumpHeld = Input.GetKey(KeyCode.Space) || Input.GetButton("Jump");

        if (JumpDown)
        {
            jumpToConsume = true;
            timeJumpPressed = time;
        }

        MoveDirection = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
    }
    void HandleCollisions()
    {
        Physics2D.queriesStartInColliders = false;
        Vector2 ColLeftDown = new(Collider.bounds.min.x, Collider.bounds.min.y);
        Vector2 ColRightDown = new(Collider.bounds.max.x, Collider.bounds.min.y);
        Vector2 ColLeftUp = new(Collider.bounds.min.x, Collider.bounds.max.y);
        Vector2 ColRightUp = new(Collider.bounds.max.x, Collider.bounds.max.y);

        groundHit = Physics2D.Raycast(ColLeftDown, Vector2.down, GrounderDistance, ground) || Physics2D.Raycast(ColRightDown, Vector2.down, GrounderDistance, ground);
        ceilingHit = Physics2D.Raycast(ColLeftUp, Vector2.up, GrounderDistance, ground) || Physics2D.Raycast(ColRightUp, Vector2.up, GrounderDistance, ground);

        if (ceilingHit) tempVelocity.y = Mathf.Min(0, tempVelocity.y);

        if (!IsStanding && groundHit)
        {
            IsStanding = true;
            coyoteUsable = true;
            bufferedJumpUsable = true;
            endedJumpEarly = false;
        }

        if (IsStanding && !groundHit)
        {
            IsStanding = false;
            timeGroundLeft = time;
        }
    }
    void HandleY()
    {
        if (!endedJumpEarly && !IsStanding && !JumpHeld && RigidBody.velocity.y > 0) { endedJumpEarly = true; }

        if (!jumpToConsume && !HasBufferedJump) { return; }

        if (IsStanding || HasCoyoteTime && !movementDisable) { DoJump(); }

        jumpToConsume = false;

        void DoJump()
        {
            endedJumpEarly = false;
            timeJumpPressed = 0;
            bufferedJumpUsable = false;
            coyoteUsable = false;
            IsJumping = true;
            tempVelocity.y = JumpPower;
        }
    }

    void HandleX()
    {
        if (MoveDirection != 0 && (MoveDirection == 1 && transform.localRotation.y != 0) || (MoveDirection == -1 && transform.localRotation.y != 180))
        {
            int angle = MoveDirection == 1 ? 0 : 180;
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, angle, transform.rotation.z);
        }

        if (MoveDirection == 0 && RigidBody.velocity.x > HorizontalDeadZoneThreshold)
        {
            var deceleration = IsStanding ? GroundDeceleration : AirDeceleration;

            tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, MoveDirection * MaxSpeed, Acceleration * Time.fixedDeltaTime);
        }
        tempVelocity.x = Mathf.Clamp(tempVelocity.x, -MaxSpeed, MaxSpeed);
    }

    private void HandleGravity()
    {
        if (IsStanding && tempVelocity.y <= VerticalDeadZoneThreshold)
        {
            tempVelocity.y = GroundingForce;
        }
        else
        {
            var inAirGravity = FallAcceleration;

            if (endedJumpEarly && tempVelocity.y > 0)
            {
                inAirGravity *= JumpEndEarlyGravityModifier;
            }
            tempVelocity.y = Mathf.MoveTowards(tempVelocity.y, -MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }

        if (previousVelocity.y >= 0 && tempVelocity.y <= 0 && IsJumping)
        { apexHit = true; }
        if (apexHit)
        {
            Debug.Log("trying to apex");
            tempVelocity.x *= ApexSpeedBonus;
            apexHit = false;
            IsJumping = false;
        }
    }

    void ApplyMovement()
    {
        previousVelocity = tempVelocity;
        RigidBody.velocity = tempVelocity;
        Debug.Log(tempVelocity);
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

