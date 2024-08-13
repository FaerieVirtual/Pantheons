using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IMenuBase
{
    public UnityEvent gameLoad = new();
    private void Start()
    {
    }
    public void NewGame()
    {
        GodMenu god = FindObjectOfType<GodMenu>(true);
        god.gameObject.SetActive(true);
        MainMenu main = FindObjectOfType<MainMenu>(true);
        main.gameObject.SetActive(false);

    }
    public void LoadGame()
    {
        throw new System.NotImplementedException();
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
