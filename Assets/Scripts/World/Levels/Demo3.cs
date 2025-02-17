
using System.Collections.Generic;

public class Demo3 : Level
{
    public Demo3(GameStatemachine machine, string LevelID = "A3", string LevelScene = "Demo3") : base(machine, LevelID, LevelScene)
    {
        this.machine = machine;
    }

    public override HashSet<string> Flags { get => base.Flags; set => base.Flags = value; }

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

