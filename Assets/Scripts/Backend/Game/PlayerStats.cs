using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class PlayerStats : ScriptableObject
{
    public Scene respawnPointScene;

    [Header("Player Values")]
    public int maxHp;
    public int maxDef;

    [Header("Unlocked")]
    public bool dash;
    public bool doubleJump;

    [Header("Inventory")]
    public List<Item> inventory;
    public List<Charm> equippedCharms;

}