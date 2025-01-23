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
        //GameManager.Instance.Load();
    }

    public void Back()
    {
        levelManager.LoadScene("MainMenu");
    }
}
