using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameRunningState running = new(GameManager.instance.machine);

    public void Resume()
    {
        GameManager.instance.machine.ChangeState(running);
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
