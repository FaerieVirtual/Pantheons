using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameStatemachine machine;
    private void OnEnable()
    {
        if (TryGetComponent(out Canvas canvas) && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

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

    public void Quit()
    {
        Application.Quit();
    }
}
