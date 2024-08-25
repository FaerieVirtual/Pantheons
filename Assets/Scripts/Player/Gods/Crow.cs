using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : GodBase
{
    public Crow()
    {
        goldMultiplier = 1.4f;
        speedMultiplier = 1.2f;
        atkMultiplier = 0.9f;
        mAtkMultiplier = 0.8f;
    }
    private void Start()
    {
        stat1 = "+ GOLD";
        stat2 = "+ SPEED";
        stat3 = "- ATK";
        stat4 = "- MATK";
    }
}
