using UnityEngine;
public class Demo3 : Level
{
    public Demo3(GameStatemachine machine, string LevelID = "A3", string LevelScene = "Demo3") : base(machine, LevelID, LevelScene)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        if (HasFlag("SwordTaken"))
        {            
            foreach (PickupableItem obj in Object.FindObjectsOfType<PickupableItem>())
            {
                if (obj.ObjectID == "Sword") { Object.Destroy(obj.gameObject); break; }
            }
        }
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

