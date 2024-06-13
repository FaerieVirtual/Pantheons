using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public static Healthbar instance;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        if (instance != null) { Destroy(gameObject); }
    }

}
