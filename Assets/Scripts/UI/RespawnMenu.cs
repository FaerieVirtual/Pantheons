using System.Linq;
using UnityEngine;

public class RespawnMenu : MonoBehaviour
{
    GameStateMachine machine;
    private void OnEnable()
    {
        if (TryGetComponent(out Canvas canvas) && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
    private void Start()
    {
        machine = GameManager.Instance.Machine;
    }
    public void Respawn()
    {
        Level level = GameManager.Instance.LevelManager.Levels.Values.FirstOrDefault(level => level.HasFlag("RespawnLevel"));
        PlayerManager.Instance.ResetPlayer();
        machine.ChangeState(level);
    }
    public void BackToMenu()
    {
        DataManager manager = GameManager.Instance.DataManager;
        manager.SaveFile(manager.Save(), manager.SaveIndex);

        GameMainMenuState mainmenu = new(GameManager.Instance.Machine);
        GameManager.Instance.Machine.ChangeState(mainmenu);
    }

}
