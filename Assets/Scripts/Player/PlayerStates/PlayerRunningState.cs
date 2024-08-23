//using UnityEngine;
//using UnityEngine.Events;

//public class PlayerRunningState : PlayerState
//{
//    private Transform transform;
//    private Rigidbody2D RigidBody;
//    //private Animator animator;
//    private AudioManager audio;
//    private float speed;

//    private float timer = 0;
//    private float animTriggerTime = 0.1f;

//<<<<<<< Updated upstream
//    private Vector2 moveInput;

//    public UnityEvent playerRun = new();
//    public UnityEvent playerRunStop = new();
//=======
//>>>>>>> Stashed changes

//    public PlayerRunningState(PlayerManager player, PlayerStatemachine machine) : base(player, machine)
//    {
//        transform = player.transform;
//        speed = player.Acceleration;
//        //animator = player.Animator;
//        RigidBody = player.RigidBody;
//        audio = AudioManager.instance;
//    }


//    public override void EnterState()
//    {
//        //animator.Play("Run");
//    }
//    public override void ExitState()
//    {
//        timer = 0;
//    }
    //private Vector2 moveInput;

    //public UnityEvent playerRun = new();
    //public UnityEvent playerRunStop = new();

    //public PlayerRunningState(PlayerManager player, PlayerStatemachine machine) : base(player, machine)
    //{
    //    transform = player.transform;
    //    speed = player.Acceleration;
    //    //animator = player.Animator;
    //    RigidBody = player.RigidBody;
    //    audio = AudioManager.instance;

    //    playerRun.AddListener(audio.OnPlayerRun);
    //    playerRunStop.AddListener(audio.OnPlayerRunStop);
    //}


    //public override void EnterState()
    //{
    //    //animator.Play("Run");
    //}
    //public override void ExitState()
    //{
    //    playerRunStop.Invoke();
    //    timer = 0;
    //}

//    public override void PhysicsUpdate()
//    {
//        if (Input.GetKey(KeyCode.LeftArrow))
//        {
//            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180f, transform.rotation.z);
//            RigidBody.velocity = new Vector2(-speed, RigidBody.velocity.y);
//        }

//        if (Input.GetKey(KeyCode.RightArrow))
//        {
//            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0f, transform.rotation.z);
//            RigidBody.velocity = new Vector2(speed, RigidBody.velocity.y);
//        }
//        timer += Time.deltaTime;
//<<<<<<< Updated upstream
//        if (timer > animTriggerTime)
//        {
//            playerRun.Invoke();
//        }
//        CalculateImpactAngle();
//=======
//>>>>>>> Stashed changes
//    }
    //    if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0f, transform.rotation.z);
    //        RigidBody.velocity = new Vector2(speed, RigidBody.velocity.y);
    //    }
    //    timer += Time.deltaTime;
    //    if (timer > animTriggerTime)
    //    {
    //        playerRun.Invoke();
    //    }
    //    CalculateImpactAngle();
    //}

//    public override void Update()
//    {
//        base.Update();
//        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
//    }

//    void CalculateImpactAngle()
//    {
//        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, 1f, LayerMask.GetMask("Ground"));

//        if (hit.collider != null)
//        {
//            float angle = Vector2.Angle(hit.normal, Vector2.up);

//            if (angle > 45 && angle < 135)
//            {
//                // zeï
//                RigidBody.velocity = new Vector2(RigidBody.velocity.x, -2);
//            }
//            else
//            {
//                // zemì
//                RigidBody.velocity = new Vector2(moveInput.x * speed, RigidBody.velocity.y);
//            }
//        }
//    }
//}
