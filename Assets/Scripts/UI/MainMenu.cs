using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //public void NewGame()
    //{
    //    //if (GameManager.Instance.DataManager.SaveIndex == 0) { GameManager.Instance.DataManager.SaveIndex = 1; }
    //    GameManager.Instance.machine.ChangeState(GameManager.Instance.LevelManager.GetLevelByID("A1"));
    //    if (UIManager.Instance != null && !UIManager.Instance.PlayerUI.activeSelf) UIManager.Instance.PlayerUI.SetActive(true);
    //}
    public void LoadGame()
    {
        GameLoadMenuState loadMenuState = new(GameManager.Instance.machine);
        GameManager.Instance.machine.ChangeState(loadMenuState);
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
