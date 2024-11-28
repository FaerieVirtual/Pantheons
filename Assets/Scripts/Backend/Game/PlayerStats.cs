using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class PlayerStats : ScriptableObject
{
    public Scene respawnPointScene;

    [Header("Statistics")]
    public int maxHp;
    public int maxDef;

    [Header("Abilities")]
    public bool dash;
    public bool doubleJump;

    [Header("Inventory")]
    public List<Item> inventory;
    public List<Charm> equippedCharms;
    public List<Charm> unequippedCharms;

}