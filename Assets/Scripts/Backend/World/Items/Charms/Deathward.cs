
using System;

internal class Deathward : Charm
{
    public Deathward()
    {
        name = "Deathward";
        description = "Forestall a premature meeting with your god.";
        slotsRequired = 2;

        hpAdd = 1;
    }

    public int DeathSave(int damage) 
    {
        Random r = new();
        int tmp = r.Next(0, 11);
        if (tmp <= 3) { return 0; }
        else { return damage; }
    }
}




