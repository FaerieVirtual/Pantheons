using UnityEngine;

public class InteractibleObject : MonoBehaviour, IInteractible
{
    public bool CanInteract { get; set; } = true;
    public string Flag;
    private Level Level => (Level)GameManager.Instance.Machine.CurrentState;

    private void Awake()
    {
        if (Level.HasFlag(Flag) && Flag != null) Destroy(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            collision.gameObject.GetComponent<PlayerManager>().Interact.AddListener(Interaction);
        }
    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            collision.gameObject.GetComponent<PlayerManager>().Interact.RemoveListener(Interaction);
        }
    }

    public virtual void Interaction()
    {
        if (Flag != null) Level.SetFlag(Flag);
    }
}

