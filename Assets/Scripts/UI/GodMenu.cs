using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GodMenu : MonoBehaviour
{ 
    public List<GodBase> gods;
    public Image godView;
<<<<<<< Updated upstream
    //public TextMeshPro descriptionText;
    public TextMeshPro godname;
=======
    public Text descriptionText;
>>>>>>> Stashed changes
    public TextMeshPro stat1;
    public TextMeshPro stat2;
    public TextMeshPro stat3;
    public TextMeshPro stat4;
    public Button confirmButton;

    public List<Sprite> viewSprites, symbolSprites;
    //public List<string> descriptions;

    public SpriteRenderer selected;
    public SpriteRenderer previous;
    public SpriteRenderer next;

    private int currentIndex = 0;

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmSelection);
        confirmButton.onClick.AddListener(GameManager.instance.ChooseGod);
        UpdateSelection();
        viewSprites = new List<Sprite>();
        symbolSprites = new List<Sprite>();
        for (int i = 0; i < gods.Count; i++)
        {
            viewSprites.Add(gods[i].Profile);
            symbolSprites.Add(gods[i].Symbol);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Return))
        {
            OnConfirmSelection();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { OnNext(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { OnPrevious(); }
    }

    public void OnNext()
    {
        currentIndex = (currentIndex + 1) % viewSprites.Count;
        UpdateSelection();
    }

    public void OnPrevious()
    {
        currentIndex = (currentIndex - 1 + viewSprites.Count) % viewSprites.Count;
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        if (gods[currentIndex] != null)
        {
            GodBase god = gods[currentIndex];

            godView.sprite = god.Profile;
<<<<<<< Updated upstream
            //descriptionText.text = god.description;
            selected.sprite = god.Symbol;
            stat1.text = god.stat1;
=======
            //descriptionText.text = god.description;//descriptionText.text = descriptions[currentIndex];
            selected.sprite = god.Symbol;
>>>>>>> Stashed changes
            stat2.text = god.stat2;
            stat3.text = god.stat3;
            stat4.text = god.stat4;
            godname.text = god.name;

        }
        int prevIndex = (currentIndex - 1 + viewSprites.Count) % viewSprites.Count;
        previous.sprite = gods[prevIndex].Symbol;
        int nextIndex = (currentIndex + 1 + viewSprites.Count) % viewSprites.Count;
        next.sprite = gods[nextIndex].Symbol;

    }

    public void OnConfirmSelection()
    {
<<<<<<< Updated upstream
        GameRunningState running = new(GameManager.instance.machine);
=======
        GameManager.instance.god = gods[currentIndex];
        GodMenu god = FindObjectOfType<GodMenu>(true);
        god.gameObject.SetActive(false);
>>>>>>> Stashed changes

        GameManager.instance.god = gods[currentIndex];
        GameManager.instance.machine.ChangeState(running);

        SceneManager.LoadScene("AP1");
    }
}
