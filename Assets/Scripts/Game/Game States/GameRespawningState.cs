using UnityEngine;
public class GameRespawningState : GameState
{
    private RespawnMenu respawnMenu;
    public GameRespawningState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
        respawnMenu = Object.FindObjectOfType<RespawnMenu>(true);
        respawnMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public override void ExitState()
    {
        respawnMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public override void Update()
    {
        base.Update();
    }
}

