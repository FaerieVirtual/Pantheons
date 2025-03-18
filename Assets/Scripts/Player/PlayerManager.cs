using Assets.Scripts.Player;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IDamageable
{
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
        SetInput();

        transform.position = new Vector2(GameManager.Instance.DataManager.GameSave.lastX, GameManager.Instance.DataManager.GameSave.lastY + 2);
        UpdateHealth();
        UpdateAbilities();
        UpdateAttack();
        if (Hp <= 0) Die();
    }
    private void FixedUpdate()
    {
        HandleCollisions();
        Move();
        HandleGravity();
        ApplyMovement();

        UpdateMana();
    }
    private void Update()
    {
        UpdateInput();
    }
    #endregion

    #region Player Management
    [Header("INPUT")]
    public float VerticalDeadZoneThreshold = .1f;
    public float HorizontalDeadZoneThreshold = .1f;

    private PlayerInput JumpInput;
    private PlayerInput InteractInput;
    private PlayerInput PauseInput;
    private PlayerInput DebugInput;
    private PlayerInput InventoryInput;
    private PlayerInput ConsumableInput;
    private PlayerInput Amulet1Input;
    private PlayerInput Amulet2Input;
    private PlayerInput Amulet3Input;
    private PlayerInput AttackInput;

    public void ResetPlayer()
    {
        Hp = MaxHp;
        Mana = MaxMana;
        invincible = false;
        movementDisable = false;
    }
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

        AttackInput = new(KeyCode.C, 0.1f);
        AttackInput.OnDown.AddListener(HandleAttack);

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
        AttackInput.Update();
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

    public int DamagePushbackForce;
    public void Die()
    {
        movementDisable = true;
        Gold = 0;

        GameRespawningState respawningState = new(GameManager.Instance.Machine);
        GameManager.Instance.Machine.ChangeState(respawningState);
    }
    public async void TakeDamage(int damage)
    {
        if (invincible) return;

        Hp -= damage;
        if (damage > 0)
        {
            SetInvincibility(invincibleDuration);
            Time.timeScale = 0.4f;
            await Task.Delay((int)(invincibleDuration * 1000));
            Time.timeScale = 1;
        }
        if (Hp <= 0) { Die(); }
    }
    public async void SetInvincibility(float duration)
    {
        invincible = true;
        GetComponent<SpriteRenderer>().color = Color.cyan;

        await Task.Delay((int)(duration * 1000));

        GetComponent<SpriteRenderer>().color = Color.white;
        invincible = false;
    }
    public void UpdateHealth()
    {
        Image[] HpImages = UIManager.Instance.PlayerUI.GetComponent<PlayerGUI>().HpImages;

        for (int i = 0; i < HpImages.Length; i++)
        {
            if (i < Hp) { HpImages[i].sprite = Resources.Load<Sprite>("Sprites/heartfull"); }
            else { HpImages[i].sprite = Resources.Load<Sprite>("Sprites/heartempty"); }

            if (i < MaxHp) { HpImages[i].enabled = true; }
            else { HpImages[i].enabled = false; }
        }
        if (Hp > MaxHp) { Hp = MaxHp; }

    }
    public async void Heal(int healAmount)
    {
        if (Hp >= MaxHp) return;
        Hp += healAmount;
        GetComponent<SpriteRenderer>().color = Color.green;
        UpdateHealth();
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
    public float MaxJumpTime;
    public float MaxFallSpeed;
    public float FallAcceleration;
    public float JumpEndGModifier;
    public float CoyoteTime;
    public float ApexSpeedModifier;

    private int MoveDirection;
    private bool groundHit, ceilingHit;
    private bool IsStanding, IsJumping, FacingRight;
    private Vector2 tempVelocity;
    private LayerMask Ground => LayerMask.GetMask("Ground");
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D>();
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
        if (tempVelocity.y <= JumpPower) tempVelocity.y = JumpPower;
    }
    void Move()
    {
        if (MoveDirection == 1) { FacingRight = true; }
        else if (MoveDirection == -1) { FacingRight = false; }


        if (MoveDirection == 0)
        {
            var deceleration = IsStanding ? GroundDeceleration : AirDeceleration;
            tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            if ((MoveDirection == 1 && transform.localRotation.y != 0) || (MoveDirection == -1 && transform.localRotation.y != 180))
            {
                int angle = MoveDirection == 1 ? 0 : 180;
                transform.localRotation = Quaternion.Euler(transform.localRotation.x, angle, transform.rotation.z);
            }
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
            if (endedJump && tempVelocity.y < -5) inAirGravity *= JumpEndGModifier;

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

    public void Pushback(Transform origin, int Force)
    {
        Vector2 bounce = (transform.position - origin.position).normalized;
        tempVelocity += bounce * Force;
    }
    #endregion

    #region Interact
    [HideInInspector] public UnityEvent Interact = new();

    private void Pause()
    {
        if (UIManager.Instance.InventoryUI.activeSelf) { UIManager.Instance.InventoryUI.SetActive(false); }
        else if (UIManager.Instance.TradeMenuUI.activeSelf) { UIManager.Instance.TradeMenuUI.SetActive(false); }

        GameStatemachine machine = GameManager.Instance.Machine;
        GamePausedState pausedState = new(machine);
        if (machine.CurrentState is Level) machine.ChangeState(pausedState);
        else machine.ChangeState(machine.PreviousState);
    }

    #endregion

    #region Attacking
    [Header("ATTACK")]
    public int AttackPushbackForce;
    public bool attackDisable;

    public int Damage => baseDamage + boostedDamage;
    [HideInInspector] public int baseDamage;
    [HideInInspector] public int boostedDamage;
    [HideInInspector] public float attackReach;

    [HideInInspector] public int GatherGoldBoost = 0;
    [HideInInspector] public int GatherManaBoost = 0;

    public void UpdateAttack()
    {
        if (equippedWeapon != null && equippedWeapon.IsEmpty)
        {
            baseDamage = 0;
            attackReach = 0;
            attackDisable = true;
        }
        if (equippedWeapon != null && !equippedWeapon.IsEmpty)
        {
            WeaponItem tmp = (WeaponItem)equippedWeapon.Item;
            baseDamage = tmp.damage;
            attackReach = tmp.reach;
            attackDisable = false;
        }
    }

    private Vector2 GetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow)) return Vector2.up;
        else if (Input.GetKey(KeyCode.DownArrow)) return Vector2.down;
        else if (FacingRight) return Vector2.right;
        else return Vector2.left;
    }

    public Vector2 GetSize(Vector2 direction)
    {
        if (direction == Vector2.down || direction == Vector2.up) return new Vector2(1, attackReach);
        if (direction == Vector2.right || direction == Vector2.left) return new Vector2(attackReach, 1);

        return Vector2.zero;
    }
    private void HandleAttack()
    {
        if (attackDisable) return;
        attackDisable = true;
        Vector2 Direction = GetDirection();
        Vector2 Size = GetSize(Direction);
        Vector2 Center = (Vector2)transform.position + Direction * (attackReach / 2);

        foreach (Collider2D hit in Physics2D.OverlapBoxAll(Center, Size, 0))
        {
            if (hit.GetComponent<PlayerManager>() != null) continue;

            if (hit.transform.parent != null && hit.transform.parent.TryGetComponent(out IDamageable damagedParent))
            {
                damagedParent.TakeDamage(Damage);
                if (hit.transform.parent.GetComponent<EnemyBase>() != null || hit.GetComponent<Spikes>() != null)
                {
                    Pushback(hit.transform, AttackPushbackForce);
                }
            }
            if (hit.TryGetComponent(out IDamageable damaged))
            {
                damaged.TakeDamage(Damage);
                if (hit.GetComponent<EnemyBase>() != null || hit.GetComponent<Spikes>() != null)
                {
                    Pushback(hit.transform, AttackPushbackForce);
                }
            }
        }
        attackDisable = false;
    }

    #endregion

    #region Abilities
    [Header("Abilities")]
    public int Mana = 100;
    public int MaxMana => baseMaxMana + boostedMaxMana;
    public int baseMaxMana = 100;
    [HideInInspector] public int boostedMaxMana;
    [HideInInspector] public float amulet1ChargeTime;
    [HideInInspector] public float amulet2ChargeTime;
    [HideInInspector] public float amulet3ChargeTime;
    private TextMeshProUGUI manaCounter => UIManager.Instance.PlayerUI.GetComponent<PlayerGUI>().ManaCounter;

    public void UpdateMana()
    {
        if (Mana > MaxMana) Mana = MaxMana;
        manaCounter.text = Mana.ToString();
    }
    public void UpdateAbilities()
    {
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
            UpdateMana();
        }
    }

    public void AddMana(int amount)
    {
        Mana += amount;
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
    public int Gold;

    public void TriggerInventory()
    {
        GameObject playerUI = UIManager.Instance.PlayerUI;

        InventoryMenu inventoryMenu = UIManager.Instance.InventoryUI.GetComponent<InventoryMenu>();
        TradeMenu tradeMenu = UIManager.Instance.TradeMenuUI.GetComponent<TradeMenu>();
        if (inventoryMenu == null)
        {
            return;
        }

        if (tradeMenu.gameObject.activeSelf)
        {
            tradeMenu.gameObject.SetActive(false);
            foreach (VendorNPC npc in FindObjectsByType<VendorNPC>(FindObjectsSortMode.None)) 
            {
                npc.UpdateVendorNPC();
            }
            if (playerUI != null && !playerUI.activeSelf) { playerUI.SetActive(true); }
        }
        else if (inventoryMenu.gameObject.activeSelf)
        {
            inventoryMenu.gameObject.SetActive(false);

            if (playerUI != null && !playerUI.activeSelf) { playerUI.SetActive(true); }
        }
        else
        {
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

