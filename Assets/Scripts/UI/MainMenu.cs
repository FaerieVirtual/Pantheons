using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
private LevelManager levelManager;
    private void Start()
    {
        levelManager = new LevelManager();
    }
    public void NewGame()
    {

        Scene godmenu = SceneManager.GetSceneByName("GodMenu");
        levelManager.LoadScene(godmenu);
    }
    public void LoadGame()
    {
        Scene loadmenu = SceneManager.GetSceneByName("LoadMenu");
        levelManager.LoadScene(loadmenu);
    }

    public void Options()
    {
        throw new System.NotImplementedException();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
