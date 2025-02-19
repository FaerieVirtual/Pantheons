
using UnityEngine;

internal interface IInteractible
{
    void Interaction();
    bool CanInteract { get; set; }
    //Collider2D Collider { get; set; }

}

