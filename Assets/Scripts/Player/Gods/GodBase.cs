using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GodBase : MonoBehaviour
{
    public Sprite Profile, Symbol;

    //[HideInInspector]
    //public string description;

    [HideInInspector]
    public string stat1, stat2, stat3, stat4;

    public float atkMultiplier = 1;
    public float mAtkMultiplier = 1;
    public float manaGainMultiplier = 1;

    public int hpAdditional = 0;
    public int defAdditional = 0;

    public float goldMultiplier = 1;
    public float itemMultiplier = 1;
    public float speedMultiplier = 1;

}
