using System.Collections.Generic;

public class Demo2 : Level
{
    public Demo2(GameStatemachine machine, string LevelID = "A1", string LevelScene = "Demo2") : base(machine, LevelID, LevelScene)
    {
        this.machine = machine;
        //Flags.Add("IsRespawnZone");
    }

    public override HashSet<string> Flags { get => base.Flags; set => base.Flags = value;}

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

