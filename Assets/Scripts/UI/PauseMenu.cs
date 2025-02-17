using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameStatemachine machine;
    private void Start()
    {
        machine = GameManager.Instance.machine;
    }
    public void Resume()
    {
        machine.ChangeState(machine.PreviousState);
    }

    public void Options()
    {
        throw new System.NotImplementedException();
    }

    public void ReturnToMain() 
    {
        GameMainMenuState mainstate = new(machine);
        machine.ChangeState(mainstate);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
