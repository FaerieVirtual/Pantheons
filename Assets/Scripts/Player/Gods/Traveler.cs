using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveler : GodBase
{
    public Traveler()
    {
        defAdditional = 2;
        hpAdditional = 2;
        goldMultiplier = 0.8f;
        manaGainMultiplier = 0.7f;
    }
    private void Start()
    {
        stat1 = "+ DEF";
        stat2 = "+ HP";
        stat3 = "- GOLD";
        stat4 = "- MANA";
    }
}
