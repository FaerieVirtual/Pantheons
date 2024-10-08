using UnityEngine;
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


    #region General
    //TODO
    //simple statemachine based on simple bools, determining what action is the player currently executing and what actions cannot be executed at the same moment
    //queue for buffered actions¨limited to one consequent action (could be timed, so the queue works only a moment before the current action is to end)
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
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        GetInput();

        //Movement
        HandleCollisions();
        HandleY();
        HandleX();
        HandleGravity();
        ApplyMovement();

        //Attack
        HandleAttack();

        //Health
        HealthChecks();
        HandleInvincibility();
        HandleRespawnPoint();
    }
    #endregion

    #region PlayerManagement
    [Header("INPUT")]
    public float VerticalDeadZoneThreshold = .1f; //Determines the minimum velocity for the character to move vertically
    public float HorizontalDeadZoneThreshold = .1f; //Determines the minimum velocity for the character to move horizontaly
    public float TapThreshold = .275f; //Determines the difference between a tap and a hold of a button in time
    public float HoldThreshold = 1; //Determines after what point does the hold cut off and button resets

    private float timeInteractDown;
    private float timeInteractUp;

    private void OnCollisionStay(Collision collision) //Handles interactible object detection
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


        if (collision.transform.TryGetComponent<IInteractible>(out var tmp))
        {
            if ((timeInteractUp - timeInteractDown > TapThreshold) || (!tmp.CanInteract)) return;
            tmp.HandleInteraction();
            //checks whether an object can be interacted with and then executes its interact function if the button was tapped
        }
    }

    public void ResetPlayer()
    {
        alive = true;
        hp = maxHp;
        def = maxDef;
        mana = maxMana;
        invincible = false;
        movementDisable = false;
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.O)) TakeDamage(1); //DEBUG COMMAND to take damage (for healing tests)
        MoveInput();
        InteractInput();
        AttackInput();

        void MoveInput()
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
        void InteractInput()
        {
            bool Held = false; //Rules out pressing buttons while the button is already pressed
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("InteractInput") && !Held)
            {
                timeInteractDown = time;
                Held = true;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetButtonUp("InteractInput") && Held)
            {
                timeInteractUp = time;
                Held = false;
            }
        }
        void AttackInput()
        {
            if (Input.GetKeyDown(KeyCode.X) && !attackDisable)
            {
                switch (attackUsable) //checks whether attack action can be activated. If not, it puts it in the queue
                {
                    case true:
                        //attacking = true;
                        HandleAttack();
                        break;
                    case false: attackBuffer = true; break; //buffers the attack action
                }
            }
            if (time > attackTime + attackDelay)
            {
                attackUsable = true;
                if (attackBuffer) //executes the attack action whenever attack is usable if it's buffered
                {
                    //attacking = true;
                    HandleAttack();
                }
            }
        }

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
    [HideInInspector] public Scene respawnPointScene;
    private bool respawnPointOverlap;
    private bool IsSitting;
    [HideInInspector] public bool canChangeScenes;

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
            for (int j = maxHp + 1; j < hpImages.Length; j++)
            {
                if (j < def) { hpImages[j].sprite = fullShield; }
                else { hpImages[j].sprite = emptyShield; }

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
        if (!canChangeScenes)
        {
            Debug.LogError("Cannot change scenes. Respawn is impossible.");
            return;
        }

        Loader.LoadScene(respawnPointScene);
        GameObject[] objects = respawnPointScene.GetRootGameObjects();
        foreach (GameObject obj in objects)
        {
            if (obj != null && obj.CompareTag("Respawn")) respawnPoint = obj;
        }
        transform.position = respawnPoint.transform.position;
        ResetPlayer();
        IsSitting = true;
    }

    #endregion

    #region Movement
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
    public float attackDelay = .2f; //determines the time interval before attack can be used again
    public float attackReach; //determines how far the attack reaches (in case of adding other weapons)
    public int damage = 1;
    public float pushbackForce = 10; //by what force will the character be pushed back from the direction of the attack

    private bool attackUsable; //checks whether attack isn't blocked by other actions
    private bool attackDisable; //debug switch to disable attacks 

    private float attackTime; //the time when attack action happened
    private bool attackBuffer; //queues another attack if the button was pressed, but the player is doing another action
    //private bool attacking; //This is never used, but I'm keeping it here in the case I need it when blocking other actions and abilites during attack

    private void HandleAttack()
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



    #endregion

    #region Abilities
    [Header("HEAL")]
    public float healChargeTime = .7f;
    public float healManaCost = 30;

    public void Heal()
    {
        if (!(timeInteractUp - timeInteractDown < healChargeTime)) return;

        if (hp == maxHp) return;

        hp += 1;
        mana -= healManaCost;
    }

    #endregion

    #region Magic
    [Header("MAGIC")]
    public float mana = 100;
    public float maxMana = 100;
    public float mAtk;

    public void tmpRegainMana()
    {
        if (time >= timeInteractUp + 1 && mana != maxMana)
        {
            mana += 10;
        }
    }

    #endregion
}

