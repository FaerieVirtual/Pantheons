using Assets.Scripts.Player;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();
    public static PlayerManager Instance;

    #region General
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        if (Instance == null) { Instance = this; }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Interact = new();

        ResetPlayer();
        RigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        SetInput();
    }
    private void FixedUpdate() //Executing physics simulation
    {
        //Movement
        HandleCollisions();
        Move();
        HandleGravity();
        ApplyMovement();

        //Attack
        //HandleAttack();

        HealthCheck();
        AttackCheck();
        AbilityCheck();
    }
    private void Update() //Getting Player Input
    {
        UpdateInput();
    }
    #endregion

    #region PlayerManagement
    [Header("INPUT")]
    public float VerticalDeadZoneThreshold = .1f; //Determines the minimum velocity for the character to move vertically
    public float HorizontalDeadZoneThreshold = .1f; //Determines the minimum velocity for the character to move horizontaly
    public float TapThreshold = .35f; //Determines the difference between a tap and a hold of a button in time

    private PlayerInput JumpInput;
    private PlayerInput InteractInput;
    private PlayerInput PauseInput;
    private PlayerInput DebugInput;
    private PlayerInput InventoryInput;
    private PlayerInput ConsumableInput;
    private PlayerInput Amulet1Input;
    private PlayerInput Amulet2Input;
    private PlayerInput Amulet3Input;
    //private PlayerInput AttackInput;

    public void ResetPlayer()
    {
        //ResetBoosts();
        Hp = MaxHp;
        Mana = MaxMana;
        invincible = false;
        movementDisable = false;
        Gold = 0;
    }
    //public void ResetBoosts()
    //{
    //    MaxHp = baseHp + hpAdd;
    //    maxMana = baseMana + manaAdd;
    //}
    private void SetInput()
    {
        JumpInput = new(KeyCode.Space, MaxJumpTime);
        JumpInput.OnDown.AddListener(Jump);
        JumpInput.OnHold.AddListener(JumpSustain);
        JumpInput.OnUp.AddListener(() => jumpSustainable = false);
        JumpInput.OnUp.AddListener(() => endedJump = true);

        InteractInput = new(KeyCode.DownArrow, MaxJumpTime);
        InteractInput.OnDown.AddListener(Interact.Invoke);

        PauseInput = new(KeyCode.Escape, 0.1f);
        PauseInput.OnDown.AddListener(Pause);

        DebugInput = new(KeyCode.O, 0.1f);
        DebugInput.OnDown.AddListener(() => TakeDamage(1));

        InventoryInput = new(KeyCode.Tab, 0.1f);
        InventoryInput.OnDown.AddListener(TriggerInventory);

        ConsumableInput = new(KeyCode.A, 0.1f);
        ConsumableInput.OnDown.AddListener(ConsumeConsumableItem);

        Amulet1Input = new(KeyCode.S, amulet1ChargeTime);
        Amulet1Input.OnMaxReached.AddListener(() => AmuletAbility(0));

        Amulet2Input = new(KeyCode.D, amulet2ChargeTime);
        Amulet2Input.OnMaxReached.AddListener(() => AmuletAbility(1));

        Amulet3Input = new(KeyCode.F, amulet3ChargeTime);
        Amulet3Input.OnMaxReached.AddListener(() => AmuletAbility(2));

    }
    private void UpdateInput()
    {
        JumpInput.Update();
        InteractInput.Update();
        PauseInput.Update();
        DebugInput.Update();
        InventoryInput.Update();
        ConsumableInput.Update();
        Amulet1Input.Update();
        Amulet2Input.Update();
        Amulet3Input.Update();
        MoveDirection = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
    }
    #endregion

    #region Health
    [Header("HEALTH")]
    public int baseMaxHp;
    [HideInInspector] public int boostedMaxHp;
    [HideInInspector] public int MaxHp => baseMaxHp + boostedMaxHp;
    public int Hp;
    public bool Alive => Hp > 0;

    public float invincibleDuration;
    private bool invincible;

    [Header("HEALTH GRAPHICS")]
    private int hpDisplayed;
    public Image[] hpImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void Die()
    {
        movementDisable = true;

        GameRespawningState respawningState = new(GameManager.Instance.machine);
        GameManager.Instance.machine.ChangeState(respawningState);
    }
    public async void TakeDamage(int damage)
    {
        if (invincible) return; //Eventually add visual marker for user

        Hp -= damage;
        if (damage > 0)
        {
            SetInvincibility(invincibleDuration);
            Time.timeScale = 0.4f;
            await Task.Delay((int)(invincibleDuration * 1000));
            Time.timeScale = 1;
        }
    }
    public async void SetInvincibility(float duration)
    {
        invincible = true;
        GetComponent<SpriteRenderer>().color = Color.cyan;

        await Task.Delay((int)(duration * 1000));

        GetComponent<SpriteRenderer>().color = Color.white;
        invincible = false;
    }
    private void HealthCheck()
    {
        CheckForHpGraphics();
        CheckForMaxValues();
        if (Hp <= 0) { Die(); }
        void CheckForHpGraphics()
        {
            for (int i = 0; i < hpImages.Length; i++)
            {
                if (i < Hp) { hpImages[i].sprite = fullHeart; }
                else { hpImages[i].sprite = emptyHeart; }

                if (i < hpDisplayed) { hpImages[i].enabled = true; }
                else { hpImages[i].enabled = false; }
            }
        }
        void CheckForMaxValues()
        {
            if (Hp > MaxHp) { Hp = MaxHp; }
            hpDisplayed = MaxHp;
        }

    }
    public async void Heal(int healAmount)
    {
        if (Hp >= MaxHp) return;
        Hp += healAmount;
        GetComponent<SpriteRenderer>().color = Color.green;
        await Task.Delay(500);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Movement
    [Header("MOVEMENT")]
    public float MaxSpeed;
    public float Acceleration;
    public float GroundDeceleration;
    public float AirDeceleration;
    public float GroundingForce;

    [Header("JUMP")]
    public int JumpPower;
    public int JumpSustainPower;
    public float MaxJumpTime;
    public float MaxFallSpeed;
    public float FallAcceleration;
    public float JumpEndGModifier;
    public float CoyoteTime;
    public float JumpBuffer;
    public float ApexSpeedModifier;

    private int MoveDirection;
    private bool groundHit, ceilingHit;
    private bool IsStanding, IsJumping;
    private Vector2 tempVelocity;
    private LayerMask Ground => LayerMask.GetMask("Ground");
    public bool movementDisable;

    private bool endedJump;
    private bool coyoteUsable;
    private bool jumpSustainable;
    private bool ApexHit;
    private float timeGroundLeft;

    private bool HasCoyoteTime => coyoteUsable && !IsStanding && Time.time <= timeGroundLeft + CoyoteTime;

    void HandleCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        BoxCollider2D groundcheck = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        BoxCollider2D ceilingcheck = transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>();
        groundHit = groundcheck.IsTouchingLayers(Ground);
        ceilingHit = ceilingcheck.IsTouchingLayers(Ground);

        if (!IsStanding && groundHit)
        {
            IsStanding = true;
            coyoteUsable = true;
        }
        if (IsStanding && !groundHit)
        {
            IsStanding = false;
            timeGroundLeft = Time.time;
        }
        if (ceilingHit)
        {
            tempVelocity.y = Mathf.Min(0, tempVelocity.y);
            jumpSustainable = false;
        }
    }

    private void Jump()
    {
        if (IsStanding || HasCoyoteTime)
        {
            coyoteUsable = false;
            IsJumping = true;
            tempVelocity.y = JumpPower;
            endedJump = false;
            jumpSustainable = true;
        }
    }
    private void JumpSustain()
    {
        if (!jumpSustainable || JumpInput.timePressed + MaxJumpTime < Time.time) return;
        coyoteUsable = false;
        IsJumping = true;
        if (tempVelocity.y <= JumpSustainPower) tempVelocity.y = JumpSustainPower;
    }
    void Move()
    {
        if (MoveDirection != 0 && (MoveDirection == 1 && transform.localRotation.y != 0) || (MoveDirection == -1 && transform.localRotation.y != 180))
        {
            int angle = MoveDirection == 1 ? 0 : 180;
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, angle, transform.rotation.z);
        }

        if (MoveDirection == 0)
        {
            var deceleration = IsStanding ? GroundDeceleration : AirDeceleration;
            tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, MoveDirection * MaxSpeed, Acceleration * Time.fixedDeltaTime);
        }

        if (IsJumping && tempVelocity.y < 1)
        {
            tempVelocity.x *= ApexSpeedModifier;
            IsJumping = false;
            endedJump = true;
            ApexHit = true;
        }
        if (!(ApexHit && (tempVelocity.x > MaxSpeed || tempVelocity.x < -MaxSpeed)))
        {
            tempVelocity.x = Mathf.Clamp(tempVelocity.x, -MaxSpeed, MaxSpeed);
            ApexHit = false;
        }
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
            if (endedJump) inAirGravity *= JumpEndGModifier;

            tempVelocity.y = Mathf.MoveTowards(tempVelocity.y, -MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }
    private void ApplyMovement()
    {
        RigidBody.velocity = tempVelocity;
    }

    public void Stop()
    {
        tempVelocity = Vector2.zero;
    }

    #endregion

    #region Interact
    public UnityEvent Interact;

    private void Pause()
    {
        GameStatemachine machine = GameManager.Instance.machine;
        GamePausedState pausedState = new(machine);
        if (machine.CurrentState is Level) machine.ChangeState(pausedState);
        else machine.ChangeState(machine.PreviousState);
    }

    #endregion

    #region Attacking
    [Header("ATTACK")]
    public float attackDelay = .2f; //determines the time interval before attack can be used again
    public float attackReach; //determines how far the attack reaches (in case of adding other weapons)
    public int Damage => baseDamage + boostedDamage;
    public int baseDamage;
    [HideInInspector] public int boostedDamage;
    public float pushbackForce = 10; //by what force will the character be pushed back from the direction of the attack

    private bool attackUsable; //checks whether attack isn't blocked by other actions
    private bool attackDisable; //debug switch to disable attacks 

    private float attackTime; //the time when attack action happened
    private bool attackBuffer; //queues another attack if the button was pressed, but the player is doing another action
    //private bool attacking; //This is never used, but I'm keeping it here in the case I need it when blocking other actions and abilites during attack

    private void AttackCheck() 
    { 
        if (equippedWeapon != null && equippedWeapon.IsEmpty) 
        { 
            baseDamage = 0; 
            attackReach = 0; 
        }
    }

    //private void HandleAttack()
    //{
    //    attackTime = Time.time;
    //    attackUsable = false;

    //    Vector2 boxOrigin = new(Collider.bounds.center.x + (Collider.bounds.max.x + attackReach) / 2 * MoveDirection, Collider.bounds.center.y);
    //    Vector2 boxSize = new(attackReach / 2, Collider.bounds.max.y / 2);
    //    Vector2 boxDirection = new(MoveDirection, 0);
    //    LayerMask player = LayerMask.GetMask("Player");
    //    Transform enemyTransform = null;

    //    RaycastHit2D[] hitObjects = Physics2D.BoxCastAll(boxOrigin, boxSize, 0f, boxDirection, 0.1f, ~player);
    //    foreach (RaycastHit2D hit in hitObjects)
    //    {
    //        if (hit.transform.gameObject.GetComponent<IDamageable>() != null)
    //        {
    //            IDamageable damaged = hit.transform.gameObject.GetComponent<IDamageable>();
    //            damaged.TakeDamage(damage);
    //            if (hit.transform.CompareTag("Enemy")) enemyTransform = hit.transform;
    //        }
    //    }
    //    if (enemyTransform != null)
    //    {
    //        Vector2 bounce = (transform.position - enemyTransform.position).normalized;
    //        RigidBody.AddForce(bounce * pushbackForce, ForceMode2D.Impulse);
    //    }

    //}



    #endregion

    #region Abilities
    [Header("Abilities")]
    public float Mana = 100;
    public float MaxMana => baseMaxMana + boostedMaxMana;
    public int baseMaxMana = 100;
    [HideInInspector] public int boostedMaxMana;
    [HideInInspector] public float amulet1ChargeTime;
    [HideInInspector] public float amulet2ChargeTime;
    [HideInInspector] public float amulet3ChargeTime;

    public TextMeshProUGUI ManaCounter;

    public void AbilityCheck()
    {
        ManaCounter.text = Mana.ToString();
        if (equippedAmulet1 != null && equippedAmulet1.Item is AbilityAmulet tmp1) 
        { 
            amulet1ChargeTime = tmp1.chargeTime;
        }
        if (equippedAmulet2 != null && equippedAmulet2.Item is AbilityAmulet tmp2)
        {
            amulet2ChargeTime = tmp2.chargeTime;
        }
        if (equippedAmulet3 != null && equippedAmulet3.Item is AbilityAmulet tmp3)
        {
            amulet3ChargeTime = tmp3.chargeTime;
        }

    }

    public void AmuletAbility(int amuletIndex)
    {
        AbstractSlot amulet = null;
        switch (amuletIndex)
        {
            case 0: amulet = equippedAmulet1; break;
            case 1: amulet = equippedAmulet2; break;
            case 2: amulet = equippedAmulet3; break;

        }
        if (amulet != null && amulet.Item is AbilityAmulet tmp)
        {
            tmp.ActivateAbility();
        }
    }
    #endregion

    #region Inventory
    [Header("Inventory")]
    [HideInInspector] public Inventory Inventory = new();
    [HideInInspector] public AbstractSlot equippedConsumable = new();
    [HideInInspector] public AbstractSlot equippedWeapon = new();
    [HideInInspector] public AbstractSlot equippedAmulet1 = new();
    [HideInInspector] public AbstractSlot equippedAmulet2 = new();
    [HideInInspector] public AbstractSlot equippedAmulet3 = new();
    public int Gold = 0;

    public void TriggerInventory()
    {
        InventoryMenu inventoryMenu = FindObjectOfType<InventoryMenu>(true);
        TradeMenu tradeMenu = FindObjectOfType<TradeMenu>(true);
        if (inventoryMenu == null)
        {
            return;
        }

        if (tradeMenu.gameObject.activeSelf)
        {
            tradeMenu.gameObject.SetActive(false);

            GameObject playerUI = FindObjectOfType<UI>(true).transform.GetChild(0).gameObject;
            if (playerUI != null && !playerUI.activeSelf) { playerUI.SetActive(true); }
        }
        else if (inventoryMenu.gameObject.activeSelf)
        {
            inventoryMenu.gameObject.SetActive(false);

            GameObject playerUI = FindObjectOfType<UI>(true).transform.GetChild(0).gameObject;
            if (playerUI != null && !playerUI.activeSelf) { playerUI.SetActive(true); }
        }
        else
        {
            GameObject playerUI = FindObjectOfType<UI>(true).transform.GetChild(0).gameObject;
            if (playerUI != null && playerUI.activeSelf) { playerUI.SetActive(false); }

            inventoryMenu.gameObject.SetActive(true);
            inventoryMenu.UpdateMenu();
        }
    }

    public void ConsumeConsumableItem()
    {
        ConsumableItem consumable = (ConsumableItem)equippedConsumable.Item;
        consumable.Consume();
        equippedConsumable.RemoveItem(1);
    }
    #endregion
}

