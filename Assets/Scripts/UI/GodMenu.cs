using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodMenu : MonoBehaviour
{
    public List<GodBase> gods;
    public Image godView;
    public Text descriptionText;
    public Text stat1;
    public Text stat2;
    public Text stat3;
    public Text stat4;
    public Button confirmButton;

    public List<Sprite> viewSprites;
    //public List<string> descriptions;
    public List<Sprite> symbolSprites;

    //public GameObject selectedObject;
    //public GameObject previousObject;
    //public GameObject nextObject;
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

        //selected = selectedObject.GetComponent<SpriteRenderer>();
        //previous = previousObject.GetComponent<SpriteRenderer>();
        //next = nextObject.GetComponent<SpriteRenderer>();
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

            godView.sprite = god.Profile;//godView.sprite = viewSprites[currentIndex];
            //descriptionText.text = god.description;//descriptionText.text = descriptions[currentIndex];
            selected.sprite = god.Symbol;//symbolSprites[currentIndex];
            stat1.text = god.stat1;
            stat2.text = god.stat2;
            stat3.text = god.stat3;
            stat4.text = god.stat4;

        }
        int prevIndex = (currentIndex - 1 + viewSprites.Count) % viewSprites.Count;
        previous.sprite = gods[prevIndex].Symbol;//symbolSprites[prevIndex];
        int nextIndex = (currentIndex + 1 + viewSprites.Count) % viewSprites.Count;
        next.sprite = gods[nextIndex].Symbol;//symbolSprites[nextIndex];

    }

    public void OnConfirmSelection()
    {
        GameManager.instance.god = gods[currentIndex];

    }
}
