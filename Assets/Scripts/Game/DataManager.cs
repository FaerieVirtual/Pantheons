using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    #region Save/Load & File management
    //private DataSave save;

    //private Dictionary<ScriptableObject, string> saveFileDic;
    //private void SaveDicRefresh() 
    //{ 
    //    saveFileDic = new Dictionary<ScriptableObject, string>()
    //    {
    //        {gameStats, $@"..\..\..\Saves\Save {gameIndex}\game"},
    //        {playerStats, $@"..\..\..\Saves\Save {gameIndex}\player" },
    //        {previewStats, $@"..\..\..\Saves\Save {gameIndex}\preview" }           
    //    };
    //}

    //public void Save()
    //{
    //    string folderPath = @"..\..\..\Saves";
    //    string savePath = Path.Combine(folderPath, $"Save {gameIndex}");
    //    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath); 
    //    if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

    //    //string GameJSON = JsonUtility.ToJson(gameStats);
    //    //string PlayerJSON = JsonUtility.ToJson(playerStats);
    //    //string PreviewJSON = JsonUtility.ToJson(previewStats);
    //    foreach (var entry in saveFileDic) 
    //    { 
    //        string JSON = JsonUtility.ToJson(entry.Key);
    //        File.WriteAllText(entry.Value, JSON);
    //    }
    //    //File.WriteAllText(Path.Combine(savePath, "game"), GameJSON);
    //    //File.WriteAllText(Path.Combine(savePath, "player"), PlayerJSON);
    //    //File.WriteAllText(Path.Combine(savePath, "preview"), PreviewJSON);
    //}

    //public void Load(int saveIndex = 0)
    //{
    //    string folderPath = Path.Combine(@"..\..\..\Saves", $"Save {saveIndex}");
    //    if (saveIndex != gameIndex)
    //    {
    //        gameIndex = saveIndex;
    //        SaveDicRefresh();
    //    }
    //    if (Directory.Exists(folderPath))
    //    {
    //        //foreach (var entry in saveFileDic) 
    //        //{ 
    //        //    if (Directory.Exists(entry.Value)) 
    //        //    { 
    //        //        string JSON = File.ReadAllText(entry.Value);
    //        //        var type = entry.Key.GetType();
    //        //        entry.Key = JsonUtility.FromJson<type>(JSON);
    //        //    }
    //        //}
    //        if (Directory.Exists(Path.Combine(folderPath, "game")))
    //        {
    //            string GameJSON = File.ReadAllText(Path.Combine(folderPath, "game"));
    //            gameStats = JsonUtility.FromJson<GameStats>(GameJSON);
    //        }
    //        else { Console.WriteLine("Error: Save corrupted: game not found"); }
    //        if (Directory.Exists(Path.Combine(folderPath, "player")))
    //        {
    //            string PlayerJSON = File.ReadAllText(Path.Combine(folderPath, "player"));
    //            playerStats = JsonUtility.FromJson<PlayerStats>(PlayerJSON);
    //        }
    //        else { Console.WriteLine("Error: Save corrupted: player not found"); }
    //        if (Directory.Exists(Path.Combine(folderPath, "preview")))
    //        {
    //            string PreviewJSON = File.ReadAllText(Path.Combine(folderPath, "preview"));
    //            previewStats = JsonUtility.FromJson<PreviewStats>(PreviewJSON);
    //        }
    //        else { Console.WriteLine("Error: Save corrupted: preview not found"); }
    //    }
    //    else { Console.WriteLine("Error: Save corrupted: folder not found."); }
    //}

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
        Nitril.Inventory.AddItem(Resources.Load<WeaponItem>("Items/Dagger"), 1);
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

