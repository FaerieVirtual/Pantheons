using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    #region Data Management
    public DataSave GameSave;
    public string SavePath => $"Saves/GameSave{SaveIndex}.txt";
    public int SaveIndex;

    JsonSerializerSettings settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
    };
    public DataSave LoadFile(int index)
    {
        string loadPath = Path.Combine("Saves", $"GameSave{index}.txt");

        if (!Directory.Exists("Saves")) return null;
        if (!File.Exists(loadPath)) return null;

        string SaveJSON = File.ReadAllText(loadPath);
        if (string.IsNullOrEmpty(SaveJSON)) return null;
        return JsonConvert.DeserializeObject<DataSave>(SaveJSON, settings);
    }

    public void Load(DataSave save, int index)
    {
        GameSave = save;
        SaveIndex = index;
    }
    public void SaveFile(DataSave save, int index)
    {
        string savePath = Path.Combine("Saves", $"GameSave{index}.txt");

        if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");

        string SaveJSON = JsonConvert.SerializeObject(save, Formatting.Indented, settings);
        Debug.Log(SaveJSON);
        if (string.IsNullOrEmpty(SaveJSON)) return;
        File.WriteAllText(savePath, SaveJSON);
    }

    public DataSave Save()
    {
        DataSave save = new();
        PlayerManager player = PlayerManager.Instance;

        save.baseMaxHp = player.baseMaxHp;
        save.MaxHp = player.MaxHp;
        save.Hp = player.Hp;

        save.baseMaxMana = player.baseMaxMana;
        save.MaxMana = player.MaxMana;
        save.Mana = player.Mana;

        save.Gold = player.Gold;

        save.inventory = player.Inventory.ToSaveInventory();
        if (!player.equippedWeapon.IsEmpty)
        {
            save.weapon.ItemPath = $"Items/{player.equippedWeapon.Item.name}";
            save.weapon.Quantity = player.equippedWeapon.Quantity;
        }
        if (!player.equippedConsumable.IsEmpty)
        {
            save.consumable.ItemPath = $"Items/{player.equippedConsumable.Item.name}";
            save.consumable.Quantity = player.equippedConsumable.Quantity;
        }
        if (!player.equippedAmulet1.IsEmpty)
        {
            save.amulet1.ItemPath = $"Items/{player.equippedAmulet1.Item.name}";
            save.amulet1.Quantity = player.equippedAmulet1.Quantity;
        }
        if (!player.equippedAmulet2.IsEmpty)
        {
            save.amulet2.ItemPath = $"Items/{player.equippedAmulet2.Item.name}";
            save.amulet2.Quantity = player.equippedAmulet2.Quantity;
        }
        if (!player.equippedAmulet3.IsEmpty)
        {
            save.amulet3.ItemPath = $"Items/{player.equippedAmulet3.Item.name}";
            save.amulet3.Quantity = player.equippedAmulet3.Quantity;
        }

        Dictionary<string, SaveLevel> SaveLevelDic = new();
        foreach (string key in GameManager.Instance.LevelManager.levels.Keys)
        {
            SaveLevelDic.Add(key, GameManager.Instance.LevelManager.levels[key].ToSaveLevel());
        }
        save.Levels = SaveLevelDic;

        foreach (Level level in GameManager.Instance.LevelManager.levels.Values)
        {
            if (level.HasFlag("LastLevel"))
            {
                save.lastLevelID = level.LevelID;
                break;
            }
        }
        save.lastX = Mathf.RoundToInt(player.transform.position.x);
        save.lastY = Mathf.RoundToInt(player.transform.position.y);

        Dictionary<string, SaveNPCData> SaveNPCDic = new();
        foreach (string key in NPCs.Keys)
        {
            SaveNPCDic.Add(key, NPCs[key].ToSaveNPCData());
        }
        save.NPCs = SaveNPCDic;

        return save;
    }

    #endregion

    #region NPC Data
    public Dictionary<string, NPCData> NPCs = new();

    private void Start()
    {
        InstantiateNPCs();
    }

    private void InstantiateNPCs()
    {
        NPCData Nitril;
        Nitril = ScriptableObject.CreateInstance<NPCData>();

        Nitril.Inventory = new Inventory();
        Nitril.Inventory.AddItem(Resources.Load<HealthPotion>("Items/Small Blood vial"), 2);
        Nitril.Inventory.AddItem(Resources.Load<ManaStone>("Items/Small Mana Stone"), 3);
        Nitril.Inventory.AddItem(Resources.Load<BoostAmulet>("Items/Heart Locket"), 1);
        Nitril.Inventory.AddItem(Resources.Load<AbilityAmulet>("Items/Herald's Wings"), 1);
        Nitril.Inventory.AddItem(Resources.Load<AbilityAmulet>("Items/Nitril's Fang"), 1);

        Nitril.NPCResponses = new()
        {
            new NPCResponse
            {
                ExclusionFlag = "Met",
                SplitResponse = new()
                {
                    "Hi. I'm a vendor NPC. My name is Nitril.",
                    "I'm talking to you, but after we're done, I will offer you some wares.",
                    "There's a campfire ahead. Sit at it, set a respawn and save your game.",
                    "#!"
                },
            },
            new NPCResponse
            {
                ExclusionFlag = "InventoryEmpty",
                SplitResponse = new() { "#$" }
            },
            new NPCResponse
            {
                TriggerFlag = "InventoryEmpty",
                SplitResponse = new() { "Apologies. I have no more stock. Maybe come back later?", "#" }
            }
        };
        NPCs.Add("Nitril", Nitril);
    }
    #endregion
}

