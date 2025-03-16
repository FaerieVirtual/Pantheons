using System.Collections.Generic;

public class DataSave 
{
    public int baseMaxHp;
    public int MaxHp;
    public int Hp;

    public int baseMaxMana;
    public int MaxMana;
    public int Mana;

    public int Gold;

    public SaveInventory inventory;
    public SaveSlot weapon = new();
    public SaveSlot consumable = new();
    public SaveSlot amulet1 = new();
    public SaveSlot amulet2 = new();
    public SaveSlot amulet3 = new();

    public Dictionary<string, SaveLevel> Levels = new();
    public string lastLevelID;
    public int lastX, lastY;

    public Dictionary<string, SaveNPCData> NPCs = new();
}