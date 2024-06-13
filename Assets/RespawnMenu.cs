using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMenu : MonoBehaviour, IMenuBase
{
    public static RespawnMenu instance;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        if (instance != null) { Destroy(gameObject); }
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
