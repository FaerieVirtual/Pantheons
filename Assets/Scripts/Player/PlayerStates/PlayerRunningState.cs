using UnityEngine;
using UnityEngine.Events;

public class PlayerRunningState : PlayerState
{
    private Transform transform;
    private Rigidbody2D RigidBody;
    private Animator animator;
    private AudioManager audio;
    private float speed;

    private float timer = 0;
    private float animTriggerTime = 0.1f;

    public UnityEvent playerRun = new();
    public UnityEvent playerRunStop = new();

    public PlayerRunningState(PlayerManager player, PlayerStatemachine machine) : base(player, machine)
    {
        transform = player.transform;
        speed = player.speed;
        animator = player.Animator;
        RigidBody = player.RigidBody;
        audio = AudioManager.instance;

        playerRun.AddListener(audio.OnPlayerRun);
        playerRunStop.AddListener(audio.OnPlayerRunStop);
    }


    public override void EnterState()
    {
        animator.Play("Run");
    }
    public override void ExitState()
    {
        playerRunStop.Invoke();
        timer = 0;
    }

    public override void PhysicsUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180f, transform.rotation.z);
            RigidBody.velocity = new Vector2(-speed, RigidBody.velocity.y);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0f, transform.rotation.z);
            RigidBody.velocity = new Vector2(speed, RigidBody.velocity.y);
        }
        timer += Time.deltaTime;
        if (timer > animTriggerTime)
        {
            playerRun.Invoke(); 
        }

    }

    public override void Update()
    {
        base.Update();
    }
}
