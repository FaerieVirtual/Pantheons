using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    private DataSave GameSave;
    [HideInInspector] public StreamReader reader;

    public Image[] hpImages;
    public TextMeshProUGUI ManaCounter;
    public TextMeshProUGUI GoldCounter;
    public TextMeshProUGUI Area;
    public Animator playerSpriteAnimator;

    [HideInInspector] public int SelectedIndex;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Back();
    }
    public void LoadSaveFile()
    {
        if (GameSave == null)
        {
            GameSave = GameManager.Instance.DataManager.LoadFile("DefaultSave");
            if (UIManager.Instance != null && !UIManager.Instance.PlayerUI.activeSelf) UIManager.Instance.PlayerUI.SetActive(true);
        }

        GameManager.Instance.DataManager.Load(GameSave, SelectedIndex);
        GameManager.Instance.machine.ChangeState(GameManager.Instance.LevelManager.GetLevelByID(GameSave.lastLevelID));

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

        Dictionary<string, Level> SaveLevelDic = new();
        foreach (string key in GameSave.Levels.Keys)
        {
            SaveLevelDic.Add(key, GameSave.Levels[key].ToLevel());
        }
        GameManager.Instance.LevelManager.levels = SaveLevelDic;

        Dictionary<string, NPCData> SaveNPCDic = new();
        foreach (string key in GameSave.NPCs.Keys)
        {
            SaveNPCDic.Add(key, GameSave.NPCs[key].ToNPCData());
        }
        GameManager.Instance.DataManager.NPCs = SaveNPCDic;

        if (FindObjectOfType<UIManager>() == null)
        {
            Instantiate(Resources.Load<UIManager>("UI/UI"));
        }
        FindObjectOfType<UIManager>().PlayerUI.gameObject.SetActive(true);

        player.transform.position = new(GameSave.lastX, GameSave.lastY);
    }

    public void Back()
    {
        GameMainMenuState mainmenu = new(GameManager.Instance.machine);
        GameManager.Instance.machine.ChangeState(mainmenu);
    }

    public void SelectSave(int index)
    {
        GameSave = GameManager.Instance.DataManager.LoadFile(index);

        if (GameSave == null)
        {
            foreach (Image image in hpImages)
            {
                image.enabled = false;
            }
            ManaCounter.text = "";
            GoldCounter.text = "";
            Area.text = "";
            playerSpriteAnimator.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < hpImages.Length; i++)
            {
                if (i < GameSave.Hp) { hpImages[i].sprite = Resources.Load<Sprite>("Sprites/heartfull"); }
                else { hpImages[i].sprite = Resources.Load<Sprite>("Sprites/heartempty"); }

                if (i < GameSave.MaxHp) { hpImages[i].enabled = true; }
                else { hpImages[i].enabled = false; }
            }
            ManaCounter.text = GameSave.Mana.ToString();
            GoldCounter.text = $"Gold: {GameSave.Gold}";
            Area.text = $"Area: {GameManager.Instance.LevelManager.GetLevelByID(GameSave.lastLevelID).LevelScene}";
            if (!playerSpriteAnimator.gameObject.activeSelf) playerSpriteAnimator.gameObject.SetActive(true);
        }
        SelectedIndex = index;
    }
}
