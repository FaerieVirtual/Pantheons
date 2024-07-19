public class Keeper : GodBase
{
    public Keeper()
    {
        atkMultiplier = 1.2f;
        mAtkMultiplier = 0.7f;
        manaGainMultiplier = 0.7f;
        speedMultiplier = 1.15f;
    }
    private void Start()
    {
        stat1 = "+ ATK";
        stat2 = "+ SPEED";
        stat3 = "- MATK";
        stat4 = "- MANA";
    }
}

