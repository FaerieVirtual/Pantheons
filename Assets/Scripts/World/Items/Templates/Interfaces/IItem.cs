using UnityEngine;

public interface IItem 
{
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int quantity { get; set; }
    public void ActivatedAbility() { }
    public void PassiveAbility() { }
}

