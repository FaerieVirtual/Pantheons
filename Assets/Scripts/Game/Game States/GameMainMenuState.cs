using System.Linq;
using UnityEngine.SceneManagement;

public class GameMainMenuState : GameState
{
    public GameMainMenuState(GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
    }
    public override async void EnterState()
    {
        base.EnterState();
        if (!SceneManager.GetSceneByBuildIndex(0).isLoaded) await LevelManager.LoadScene("MainMenu");
        if (UIManager.Instance != null && UIManager.Instance.PlayerUI != null && UIManager.Instance.PlayerUI.activeSelf) UIManager.Instance.PlayerUI.SetActive(false);
    }
    public override void Update()
    {
        base.Update();
    }
}
