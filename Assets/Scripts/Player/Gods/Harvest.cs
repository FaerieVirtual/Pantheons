using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : GodBase
{
    public Harvest()
    {
        itemMultiplier = 1.2f;
        goldMultiplier = 1.2f;
        manaGainMultiplier = 0.8f;
        atkMultiplier = 0.8f;
    }
    private void Start()
    {
        stat1 = "+ ITEM";
        stat2 = "+ GOLD";
        stat3 = "- FAVOR";
        stat4 = "- ATK";

        //description = "test one, test two, lorem ípsum et dolores. test Harvest, sample text, sample text, sample text.";
    }
}
