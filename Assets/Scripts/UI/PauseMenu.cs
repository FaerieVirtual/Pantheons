using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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

    public void BackToMenu()
    {
        DataManager manager = GameManager.Instance.DataManager;

            manager.SaveFile(manager.Save(), manager.SaveIndex);
        GameMainMenuState mainmenu = new(GameManager.Instance.machine);
        GameManager.Instance.machine.ChangeState(mainmenu);
    }
}
