using System.Collections.Generic;
using UnityEngine;

public class Demo2 : Level
{
    public Demo2(GameStatemachine machine, string LevelID = "A2", string LevelScene = "Demo2") : base(machine, LevelID, LevelScene)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        GameManager.Instance.DataManager.NPCs["Nitril"] = Object.FindObjectOfType<VendorNPC>().Data;
    }

    public override void Update()
    {
        base.Update();
    }
}

