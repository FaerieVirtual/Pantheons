using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();
    public CapsuleCollider2D Collider => GetComponent<CapsuleCollider2D>();
    public Animator Animator => GetComponent<Animator>();
    private LevelManager Loader = new();
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
        if (instance != null) { Destroy(gameObject); }
        if (instance == null) { instance = this; }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Audio = AudioManager.instance;

        ResetPlayer();
        RigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        #region  listeners
        GameManager game = GameManager.instance;

        playerRespawn.AddListener(game.OnPlayerRespawn);
        playerHitDef.AddListener(game.OnPlayerHitDef);
        #endregion
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;

        //Movement
        GetMovementInput();
        HandleCollisions();
        HandleY();
        HandleX();
        HandleGravity();
        ApplyMovement();

        //Attack
        HandleAttackInput();
        HandleAttack();
        
        //Health
        HealthChecks();
        HandleInvincibility();
        HandleRespawnPoint();


        AnimationParams();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision == null) return;
        if (collision.transform.CompareTag("Respawn"))
        {
            if (respawnPoint != collision.gameObject) 
            { 
                respawnPoint = collision.gameObject; 
                respawnPointOverlap = true;
            }
        }
        else { respawnPointOverlap = false; }
    }

    public void ResetPlayer()
    {
        alive = true;
        hp = maxHp;
        invincible = false;
        movementDisable = false;
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
    [Header("HEALTH")]
    public int maxHp;
    public int maxDef;
    public bool alive = true;
    public float invincibleDuration;

    private int hp;
    private int def;
    private bool invincible;
    private float damageTakenTime;

    [Header("HEALTH GRAPHICS")]
    private int hpDisplayed;
    public Image[] hpImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("DEF GRAPHICS")]
    private int defDisplayed;
    public Sprite fullShield;
    public Sprite emptyShield;

    private GameObject respawnPoint;
    [HideInInspector]
    public Scene respawnPointScene;
    private bool respawnPointOverlap;
    private bool IsSitting;

    public void Die()
    {
        movementDisable = true;
        alive = false;
    }
    public void TakeDamage(int damage)
    {
        if (invincible) return;

        if (def > 0)
        {
            if (def <= damage)
            {
                damage -= def;
                def = 0;
            }
            if (def > damage)
            {
                def -= damage;
                damage = 0;
            }
        }
        hp -= damage;
        if (damage > 0 && hp > 0)
        {
            damageTakenTime = time;
        }
        if (hp <= 0) { Die(); }
    }
    private void HandleInvincibility()
    {
        if (damageTakenTime + invincibleDuration <= time)
        {
            invincible = false;
            damageTakenTime = 0;
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
            for (int j = maxHp + 1;  j < hpImages.Length; j++) 
            { 
                if (j < def) { hpImages[j].sprite = fullShield; }
                else { hpImages[j].sprite= emptyShield; }

                if (j < defDisplayed) { hpImages[j].enabled = true; }
                else { hpImages[j].enabled = false; }
            }
        }
        void CheckForMaxValues()
        {
            if (hp > maxHp) { hp = maxHp; }
            hpDisplayed = maxHp;
            if (def > maxDef) { def = maxDef; }
        }
    }
    private void HandleRespawnPoint()
    {
        if (!Input.GetKeyUp(KeyCode.DownArrow)) return;
        if (respawnPointOverlap && !IsSitting)
        {
            respawnPointScene = SceneManager.GetActiveScene();
            Mathf.MoveTowards(transform.position.x, respawnPoint.transform.position.x, previousVelocity.x);
            IsSitting = true;
            movementDisable = true;
        }
        if (IsSitting) 
        {
            IsSitting = false;
            movementDisable = false;
        }
    }

    public void Respawn()
    {
        if (respawnPointScene == null) return; 

        Loader.LoadScene(respawnPointScene);
        GameObject[] objects = respawnPointScene.GetRootGameObjects();
        foreach (GameObject obj in objects) 
        { 
            if (obj != null && obj.CompareTag("Respawn")) { respawnPoint = obj; }
        }
        transform.position = respawnPoint.transform.position;
        ResetPlayer();
        IsSitting = true;
        //if (FindObjectOfType<Respawn>() != null) { FindObjectOfType<Respawn>().activated = true; }
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

    void GetMovementInput()
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
    [Header("ATTACK")]
    public float attackDelay = .2f;
    public float attackReach;
    public int damage = 1;
    public float pushbackForce = 10;

    private bool attackUsable;
    private bool attackDisable;

    private float attackTime;
    private bool attackBuffer;
    private bool attacking;

    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.X) && !attackDisable)
        {
            switch (attackUsable)
            {
                case true: attacking = true; break;
                case false: attackBuffer = true; break;
            }
        }
        if (time > attackTime + attackDelay)
        {
            attackUsable = true;
            if (attackBuffer)
            {
                attacking = true;
            }
        }
    }
    private void HandleAttack()
    {
        if (attacking)
        {
            attackTime = time;
            attackUsable = false;

            Vector2 boxOrigin = new(Collider.bounds.center.x + (Collider.bounds.max.x + attackReach) / 2 * MoveDirection, Collider.bounds.center.y);
            Vector2 boxSize = new(attackReach / 2, Collider.bounds.max.y / 2);
            Vector2 boxDirection = new(MoveDirection, 0);
            LayerMask player = LayerMask.GetMask("Player");
            Transform enemyTransform = null;

            RaycastHit2D[] hitObjects = Physics2D.BoxCastAll(boxOrigin, boxSize, 0f, boxDirection, 0.1f, ~player);
            foreach (RaycastHit2D hit in hitObjects)
            {
                if (hit.transform.gameObject.GetComponent<IDamageable>() != null)
                {
                    IDamageable damaged = hit.transform.gameObject.GetComponent<IDamageable>();
                    damaged.TakeDamage(damage);
                    if (hit.transform.CompareTag("Enemy")) enemyTransform = hit.transform;
                }
            }
            if (enemyTransform != null)
            {
                Vector2 bounce = (transform.position - enemyTransform.position).normalized;
                RigidBody.AddForce(bounce * pushbackForce, ForceMode2D.Impulse);
            }
        }
    }



    #endregion

    #region Magic
    //public float favor;
    //public float mAtk;
    #endregion
}

