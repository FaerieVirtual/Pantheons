using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GameLoadMenuState loadMenuState = new(GameManager.Instance.Machine);
        GameManager.Instance.Machine.ChangeState(loadMenuState);
    }

    public void Options()
    {
        throw new NotImplementedException();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
