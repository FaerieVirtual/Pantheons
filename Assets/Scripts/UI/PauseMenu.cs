using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameRunningState running = new(GameManager.Instance.machine);
        GameManager.Instance.machine.ChangeState(running);
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
