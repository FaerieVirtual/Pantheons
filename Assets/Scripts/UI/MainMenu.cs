using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IMenuBase
{
    public void NewGame()
    {
        SceneManager.LoadScene("AP1");
    }
    public void LoadGame() { }

    public void Options()
    {
        throw new System.NotImplementedException();
    }

    public void Quit()
    {
        throw new System.NotImplementedException();
    }
}
