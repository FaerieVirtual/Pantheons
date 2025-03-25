using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
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
    public void LoadSaveFile() // Listener to a button in a menu
    {
        DataManager dataManager = GameManager.Instance.DataManager;
        dataManager.Load(dataManager.LoadFile(SelectedIndex), SelectedIndex);
    }

    public void Back()
    {
        GameMainMenuState mainmenu = new(GameManager.Instance.Machine);
        GameManager.Instance.Machine.ChangeState(mainmenu);
    }

    public void SelectSave(int index)
    {
        DataSave gameSave = GameManager.Instance.DataManager.LoadFile(index);

        if (gameSave == null)
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
                if (i < gameSave.Hp) { hpImages[i].sprite = Resources.Load<Sprite>("Sprites/heartfull"); }
                else { hpImages[i].sprite = Resources.Load<Sprite>("Sprites/heartempty"); }

                if (i < gameSave.MaxHp) { hpImages[i].enabled = true; }
                else { hpImages[i].enabled = false; }
            }
            ManaCounter.text = gameSave.Mana.ToString();
            GoldCounter.text = $"Gold: {gameSave.Gold}";
            Area.text = $"Area: {GameManager.Instance.LevelManager.GetLevelByID(gameSave.lastLevelID).LevelScene}";
            if (!playerSpriteAnimator.gameObject.activeSelf) playerSpriteAnimator.gameObject.SetActive(true);
        }
        SelectedIndex = index;
    }
}
