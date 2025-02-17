using Assets.Scripts.Backend.Game.Game_States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameStatemachine machine;

    private void Start()
    {
        machine = GameManager.Instance.machine;

    }
    public void NewGame()
    {
        GameManager.Instance.machine.ChangeState(GameManager.Instance.levelManager.GetLevel("A1"));
    }
    public void LoadGame()
    {
        throw new System.NotImplementedException();

        //GameLoadMenuState loadMenuState = new(machine);
        //machine.ChangeState(loadMenuState);
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
