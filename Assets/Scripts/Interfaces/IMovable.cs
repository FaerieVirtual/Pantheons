using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    int Speed { get; set; }
    void Move();
    void Flip();
    Rigidbody2D RigidBody { get; }
}
