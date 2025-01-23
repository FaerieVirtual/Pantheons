using System;
using UnityEngine;

public class AP1 : MonoBehaviour, ILevel
{
    public ILevel.Area CurrentArea { get; set; }
    private void Start()
    {
        CurrentArea = ILevel.Area.Ancient_Path;
    }

}

