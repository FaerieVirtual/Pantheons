using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Heavens : GodBase
{
    public Heavens()
    {
        manaGainMultiplier = 1.5f;
        mAtkMultiplier = 1.2f;
        hpAdditional = -2;
        atkMultiplier = 0.8f;
    }
    private void Start()
    {
        stat1 = "+ FAVOR";
        stat2 = "+ MATK";
        stat3 = "- HP";
        stat4 = "- ATK";

        //description = "lorem ípsume et dolores. test one, test two, test Heavens, sample text, sample text.";
    }
}
