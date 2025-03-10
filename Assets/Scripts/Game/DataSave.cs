using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class DataSave : ScriptableObject
{
    public Scene respawnPointScene;

    [Header("Statistics")]
    public int maxHp;
    public int maxDef;

    [Header("Abilities")]
    public bool dash;
    public bool doubleJump;

    [Header("Inventory")]
    public List<IItem> inventory;
    //public List<ICharm> equippedCharms;
    //public List<ICharm> unequippedCharms;

}