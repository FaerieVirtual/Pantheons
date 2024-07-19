using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null) { Destroy(gameObject); }
        if (instance == null) { instance = this; }
    }

    public void OnPlayerDeath() 
    { 
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
    }
    public void OnPlayerRespawn()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }
}
