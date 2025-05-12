using UnityEngine;
public class Demo3 : Level
{
    public Demo3(GameStateMachine machine, string LevelID = "A3", string LevelScene = "Demo3") : base(machine, LevelID, LevelScene)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        base.EnterState();
    }
}

