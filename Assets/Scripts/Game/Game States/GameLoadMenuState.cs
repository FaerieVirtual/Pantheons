using System.Threading.Tasks;
using UnityEngine;

public class GameLoadMenuState : GameState
{
    public GameLoadMenuState(GameStateMachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override async void EnterState()
    {
        await LevelManager.LoadScene("LoadMenu");
        LoadMenu menu = Object.FindObjectOfType<LoadMenu>();
        menu.SelectSave(1);
        menu.playerSpriteAnimator.Play("Idle");
    }
    public override void ExitState() 
    {
    }
    public override void Update() 
    { 
        
    }
}
