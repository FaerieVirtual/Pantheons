using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    LevelManager levelManager = new();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Back();
    }
    public void LoadSaveFile(int index)
    {
        GameManager.instance.Load();
    }

    public void Back()
    {
        Scene mainmenu = SceneManager.GetSceneByName("MainMenu");
        levelManager.LoadScene(mainmenu);
    }
}
