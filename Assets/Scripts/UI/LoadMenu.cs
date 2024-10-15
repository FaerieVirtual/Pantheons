using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    LevelManager levelManager = new();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Return();
    }
    public void LoadSaveFile(int index)
    {
        throw new System.NotImplementedException();
    }

    public void Return()
    {
        Scene mainmenu = SceneManager.GetSceneByName("MainMenu");
        levelManager.LoadScene(mainmenu);
    }
}
