using UnityEngine;

public class Demo1 : Level
{
    public Demo1(GameStatemachine machine, string LevelID = "A1") : base(LevelID, machine)
    {
        this.machine = machine;
    }

    public override async void EnterState()
    {
        await LevelManager.LoadScene(2, false);
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

