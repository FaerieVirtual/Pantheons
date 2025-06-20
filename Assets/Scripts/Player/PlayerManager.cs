using Assets.Scripts.Player;
using JetBrains.Annotations;
using System.Collections.Generic;
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
        UpdateHealth();
    }
    private void Update()
    {
        UpdateInput();
    }
    #endregion

    #region Player Management
    [Header("INPUT")]
    public float VelocityForJumpThreshold = .1f;
    public List<PlayerInput> Inputs = new();

    public void ResetPlayer()
    {
        Hp = MaxHp;
        Mana = MaxMana;
        invincible = false;
        movementDisable = false;
    }
    private void SetInput()
    {
        AddInput(KeyCode.Space, MaxJumpTime, Jump, JumpSustain, () => { jumpSustainable = false; endedJump = true; });
        AddInput(KeyCode.DownArrow, 0.2f, Interact.Invoke);
        AddInput(KeyCode.Escape, 0.1f, Pause);
        AddInput(KeyCode.Tab, 0.1f, TriggerInventory);
        AddInput(KeyCode.A, 0.1f, ConsumeConsumableItem);
        AddInput(KeyCode.S, amulet1ChargeTime, OnMax: () => AmuletAbility(0));
        AddInput(KeyCode.D, amulet2ChargeTime, OnMax: () => AmuletAbility(1));
        AddInput(KeyCode.F, amulet3ChargeTime, OnMax: () => AmuletAbility(2));
        AddInput(KeyCode.C, 0.1f, HandleAttack);
    }

    private void UpdateInput()
    {
       foreach (PlayerInput input in Inputs) { input.Update(); }
       MoveDirection = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
    }

    private void AddInput(KeyCode code, float maxtime, UnityAction OnDown = null, UnityAction OnHold = null, UnityAction OnUp = null, UnityAction OnMax = null) 
    {
        PlayerInput input = new(code, maxtime);
        if (OnDown != null) input.OnDown.AddListener(OnDown);
        if (OnHold != null) input.OnHold.AddListener(OnHold);
        if (OnUp != null) input.OnUp.AddListener(OnUp);
        if (OnMax != null) input.OnMaxReached.AddListener(OnMax);
        Inputs.Add(input);
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
    // Here begins the modified version of code cited in documentation in source [1]

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
    // Here ends the modified version of code cited in documentation in source [1]

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
    // Here begins the modified version of code cited in documentation in source [2]
    public float MaxSpeed;
    public float Acceleration;
    public float GroundDeceleration;
    public float AirDeceleration;
    public float GroundingForce;

    [Header("JUMP")]
    public int JumpPower;
    public float MaxJumpTime;
    public float StartJumpTime;
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
    private BoxCollider2D Groundcheck => transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
    private BoxCollider2D Ceilingcheck => transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>();

    public bool movementDisable;

    private bool endedJump;
    private bool CoyoteUsable;
    private bool jumpSustainable;
    private bool ApexHit;
    private float timeGroundLeft;
    private bool HasCoyoteTime => CoyoteUsable && !IsStanding && Time.time <= timeGroundLeft + CoyoteTime;

    void HandleCollisions()
    {
        groundHit = Groundcheck.IsTouchingLayers(Ground);
        ceilingHit = Ceilingcheck.IsTouchingLayers(Ground);

        if (!IsStanding && groundHit)
        {
            IsStanding = true;
            CoyoteUsable = true;
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
        StartJumpTime = Time.time;
        if (IsStanding || HasCoyoteTime)
        {
            CoyoteUsable = false;
            IsJumping = true;
            tempVelocity.y = JumpPower;
            endedJump = false;
            jumpSustainable = true;
        }
    }
    private void JumpSustain()
    {
        if (!jumpSustainable || StartJumpTime + MaxJumpTime < Time.time) return;
        CoyoteUsable = false;
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
        if (IsStanding && tempVelocity.y <= VelocityForJumpThreshold)
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
    // Here ends the modified version of code cited in documentation in source [2]

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

    #region Attack
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
            if (hit.TryGetComponent(out IDamageable damaged) && hit.GetComponent<PlayerManager>() == null)
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

    public async void BoostDamage(float duration, int amount)
    {
        boostedDamage += amount;
        GetComponent<SpriteRenderer>().color = Color.red;

        await Task.Delay((int)(duration * 1000));

        boostedDamage -= amount;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Abilities
    [Header("Abilities")]
    public int Mana;
    public int MaxMana => baseMaxMana + boostedMaxMana;
    public int baseMaxMana;
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
            ActivateEffect(tmp.Effect);
        }
    }

    public void ActivateEffect(PlayerEffect effect) 
    {
        if (Mana < effect.ManaCost) return;

        switch (effect.type) 
        {
            case PlayerEffectType.DamageBoost:
                BoostDamage(effect.Duration, (int)effect.Value);
                break;
            case PlayerEffectType.Healing:
                if (Hp >= MaxHp) return;

                Heal((int)effect.Value);
                break;
            case PlayerEffectType.Invincibility:
                SetInvincibility(effect.Duration);
                break;
            case PlayerEffectType.ManaRegeneration:
                if (Mana >= MaxMana) return;

                AddMana((int)effect.Value);
                break;
            case PlayerEffectType.RaiseMaxHp:
                boostedMaxHp += (int)effect.Value;
                break;
            case PlayerEffectType.RaiseMaxMana:
                boostedMaxMana += (int)effect.Value;
                break;
            case PlayerEffectType.RaiseDamage:
                boostedDamage += (int)effect.Value;
                break;
            case PlayerEffectType.RaiseGatheredGold:
                GatherGoldBoost += (int)effect.Value;
                break;
            case PlayerEffectType.RaiseGatheredMana:
                GatherManaBoost += (int)effect.Value;
                break;
        }
        Mana -= effect.ManaCost;

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
        if (equippedConsumable.Item is ConsumableItem tmp)
        ActivateEffect(tmp.Effect);       
        equippedConsumable.RemoveItem(1);
    }
    #endregion
}

