using UnityEngine;

public class Item : MonoBehaviour
{
    public new string name;
    public string description;
    public int price;
    public int quantity;
    public virtual void ActivatedAbility() { }
    public virtual void PassiveAbility() { }
}

