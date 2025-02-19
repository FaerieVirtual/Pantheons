using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Demo1 : Level
{
    public Demo1(GameStatemachine machine, string LevelID = "A1", string LevelScene = "Demo1") : base(machine, LevelID, LevelScene)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }
}

