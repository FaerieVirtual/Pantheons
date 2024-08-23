using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IMenuBase
{
    public UnityEvent gameLoad = new();
    public UnityEvent gameNew = new();
    private void Start()
    {
        gameNew.AddListener(GameManager.instance.NewGame);
    }
    public void NewGame()
    {
        gameNew.Invoke();
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
        throw new System.NotImplementedException();
    }
}
