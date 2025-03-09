using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
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
        //Nitril.Inventory.AddItem(ScriptableObject.CreateInstance<Pouch>(), 2);
        Nitril.Inventory.AddItem(Resources.Load<ManaStone>("Items/Small Mana Stone"), 3);

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
}

