using UnityEngine;
public class GameRespawningState : GameState
{
    public GameRespawningState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {

        UIManager.Instance.RespawnMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public override void ExitState()
    {
        UIManager.Instance.RespawnMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    public override void Update()
    {
        base.Update();
    }
}

