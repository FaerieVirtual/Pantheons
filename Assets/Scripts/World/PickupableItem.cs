
using UnityEngine;

public class PickupableItem : MonoBehaviour, IInteractible
{
    public bool CanInteract { get; set; } = true;
    public IItem item;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            collision.gameObject.GetComponent<PlayerManager>().Interact.AddListener(Interaction);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            collision.gameObject.GetComponent<PlayerManager>().Interact.RemoveListener(Interaction);
        }
    }

    public void Interaction()
    {
        if (CanInteract) 
        { 
            
        }
    }
}

