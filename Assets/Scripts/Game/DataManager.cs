using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    #region Data Management
    public DataSave GameSave;
    public int SaveIndex;
    private readonly JsonSerializerSettings settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
    };
    public DataSave LoadFile(int index)
    {
        string saveName = $"GameSave{index}";
        return LoadFile(saveName);
    }
    public DataSave LoadFile(string filename)
    {
        string loadPath = Path.Combine("Saves", $"{filename}.json");

        if (!Directory.Exists("Saves")) return null;
        if (!File.Exists(loadPath)) return null;

        string SaveJSON = File.ReadAllText(loadPath);
        if (string.IsNullOrEmpty(SaveJSON)) return null;
        return JsonConvert.DeserializeObject<DataSave>(SaveJSON, settings);
    }


    public async void Load(DataSave save, int index)
    {
        GameSave = save;
        SaveIndex = index;

        if (GameSave == null) 
        {
            GameSave = GameManager.Instance.DataManager.LoadFile("DefaultSave");
            if (UIManager.Instance != null && !UIManager.Instance.PlayerUI.activeSelf) UIManager.Instance.PlayerUI.SetActive(true);
        }

        Level level = GameManager.Instance.LevelManager.GetLevelByID(GameSave.lastLevelID); 
        GameManager.Instance.Machine.ChangeState(level);
        if (!level.HasFlag("LastLevel")) { level.SetFlag("LastLevel"); }

        if (FindObjectOfType<UIManager>() == null)
        {
            Instantiate(Resources.Load<UIManager>("UI/UI"));
        }

        PlayerManager player = Instantiate(Resources.Load<PlayerManager>("Player/Player"));
        player.Inventory = GameSave.inventory.ToInventory();
        if (GameSave.weapon.ItemPath != null) player.equippedWeapon.AddItem(Resources.Load<WeaponItem>(GameSave.weapon.ItemPath), GameSave.weapon.Quantity);
        if (GameSave.consumable.ItemPath != null) player.equippedConsumable.AddItem(Resources.Load<ConsumableItem>(GameSave.consumable.ItemPath), GameSave.consumable.Quantity);
        if (GameSave.amulet1.ItemPath != null)
        {
            Amulet amulet1 = Resources.Load<Amulet>(GameSave.amulet1.ItemPath);
            player.equippedAmulet1.AddItem(amulet1, GameSave.amulet1.Quantity);
            amulet1.OnEquip();
        }
        if (GameSave.amulet2.ItemPath != null)
        {
            Amulet amulet2 = Resources.Load<Amulet>(GameSave.amulet2.ItemPath);
            player.equippedAmulet2.AddItem(amulet2, GameSave.amulet2.Quantity);
            amulet2.OnEquip();
        }
        if (GameSave.amulet3.ItemPath != null)
        {
            Amulet amulet3 = Resources.Load<Amulet>(GameSave.amulet3.ItemPath);
            player.equippedAmulet3.AddItem(amulet3, GameSave.amulet3.Quantity);
            amulet3.OnEquip();
        }

        player.baseMaxHp = GameSave.baseMaxHp;
        player.Hp = GameSave.Hp;
        player.baseMaxMana = GameSave.baseMaxMana;
        player.Mana = GameSave.Mana;
        player.Gold = GameSave.Gold;

        foreach (string key in GameSave.Levels.Keys)
        {
            GameManager.Instance.LevelManager.Levels[key].ResetFlags();

            foreach (string flag in GameSave.Levels[key].flags)
            {
                GameManager.Instance.LevelManager.Levels[key].Flags.Add(flag);
            }
        }

        Dictionary<string, NPCData> SaveNPCDic = new();
        foreach (string key in GameSave.NPCs.Keys)
        {
            GameManager.Instance.DataManager.NPCs[key].Flags = GameSave.NPCs[key].Flags;
            GameManager.Instance.DataManager.NPCs[key].Inventory = GameSave.NPCs[key].Inventory.ToInventory();
        }

        if (FindObjectOfType<UIManager>() == null)
        {
            Instantiate(Resources.Load<UIManager>("UI/UI"));
        }
        FindObjectOfType<UIManager>().PlayerUI.SetActive(true);

        await Task.Delay(500);
        PlayerManager.Instance.Stop();
        PlayerManager.Instance.transform.position = new(GameSave.lastX, GameSave.lastY);
    }
    public void SaveFile(DataSave save, int index)
    {
        string savePath = Path.Combine("Saves", $"GameSave{index}.json");

        if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");

        string SaveJSON = JsonConvert.SerializeObject(save, Formatting.Indented, settings);
        if (string.IsNullOrEmpty(SaveJSON)) return;
        File.WriteAllText(savePath, SaveJSON);
    }

    public DataSave Save()
    {
        DataSave save = GameSave;
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
        foreach (string key in GameManager.Instance.LevelManager.Levels.Keys)
        {
            SaveLevelDic.Add(key, GameManager.Instance.LevelManager.Levels[key].ToSaveLevel());
        }
        save.Levels = SaveLevelDic;

        if (GameManager.Instance.LevelManager.GetLevelByFlag("LastLevel") != null)
        {
            save.lastLevelID = GameManager.Instance.LevelManager.GetLevelByFlag("LastLevel").LevelID;
        }
        else if (GameManager.Instance.Machine.PreviousState is Level tmp)
        {
            tmp.SetFlag("LastLevel");
            save.lastLevelID = tmp.LevelID;
        }

        foreach (Level level in GameManager.Instance.LevelManager.Levels.Values)
        {
            if (level.HasFlag("LastLevel"))
            {
                save.lastLevelID = level.LevelID;
                break;
            }
        }
        save.lastX = player.transform.position.x;
        save.lastY = player.transform.position.y;

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
        Nitril.Inventory.AddItem(Resources.Load<AbilityAmulet>("Items/Treeheart"), 1);
        Nitril.Inventory.AddItem(Resources.Load<WeaponItem>("Items/Rusty Dagger"), 1);
        Nitril.Inventory.AddItem(Resources.Load<BoostAmulet>("Items/Prayer Bead"), 1);

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

