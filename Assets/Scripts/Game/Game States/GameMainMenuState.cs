using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameMainMenuState : GameState
{
    public GameMainMenuState(GameStateMachine machine) : base(machine)
    {
        this.machine = machine;
    }
    public override async void EnterState()
    {
        base.EnterState();
        await Task.Delay(200);
        foreach (Scene scene in SceneManager.GetAllScenes())
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        if (!SceneManager.GetSceneByBuildIndex(0).isLoaded) await LevelManager.LoadScene("MainMenu");
        if (UIManager.Instance != null && UIManager.Instance.PlayerUI != null && UIManager.Instance.PlayerUI.activeSelf) UIManager.Instance.PlayerUI.SetActive(false);
    }
}
