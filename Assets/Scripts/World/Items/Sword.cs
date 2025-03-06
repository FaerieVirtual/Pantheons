using UnityEngine;

public class Sword : WeaponItem
{
    private void OnEnable()
    {
        Name = "Ancient Sword";
        Description = "Rusted and overgrown. This sword is perhaps more powerful than is apparent.";
        Price = -1;
    }
    //public Sword(Sprite itemSprite) : base("Ancient Sword", "Rusted and overgrown. This sword is perhaps more powerful than is apparent.", itemSprite, -1)
    //{
    //}
}