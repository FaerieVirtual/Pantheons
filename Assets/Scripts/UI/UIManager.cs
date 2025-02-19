using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
}
