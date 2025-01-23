using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GodMenu : MonoBehaviour
{
    public Image godView;
    public TextMeshPro descriptionText;
    public TextMeshPro godname;
    public TextMeshPro stat1;
    public TextMeshPro stat2;
    public TextMeshPro stat3;
    public TextMeshPro stat4;
    public Button confirmButton;

    public List<Sprite> viewSprites = new();//, symbolSprites = new();
    public List<string> descriptions;

    public SpriteRenderer selected;
    public SpriteRenderer previous;
    public SpriteRenderer next;

    private int currentIndex = 0;

    public List<GodBase> gods;
    private LevelManager levelManager;

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmSelection);
        //viewSprites = new List<Sprite>();
        //symbolSprites = new List<Sprite>();
        for (int i = 0; i < gods.Count; i++)
        {
            viewSprites.Add(gods[i].Profile);
            //symbolSprites.Add(gods[i].Symbol);
        }
        UpdateSelection();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) OnConfirmSelection(); //|| Input.GetButton("Confirm")) OnConfirmSelection();
        if (Input.GetKey(KeyCode.RightArrow)) OnNext();//|| Input.GetButton("MoveRight"))  OnNext(); 
        if (Input.GetKey(KeyCode.LeftArrow)) OnPrevious();//|| Input.GetButton("MoveLeft")) OnPrevious();
        if (Input.GetKey(KeyCode.Escape)) Return();
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
            descriptionText.text = god.description;
            selected.sprite = god.Symbol;
            descriptionText.text = god.description;
            selected.sprite = god.Symbol;
            stat1.text = god.stat1;
            stat2.text = god.stat2;
            stat3.text = god.stat3;
            stat4.text = god.stat4;
            godname.text = god.name;
            

        }
        //int prevIndex = (currentIndex - 1 + viewSprites.Count) % viewSprites.Count;
        //previous.sprite = gods[prevIndex].Symbol;
        //int nextIndex = (currentIndex + 1 + viewSprites.Count) % viewSprites.Count;
        //next.sprite = gods[nextIndex].Symbol;

    }

    public void OnConfirmSelection()
    {
        if (gods[currentIndex] != null) GameManager.Instance.god = gods[currentIndex];
        GameRunningState runningState = new(GameManager.Instance.machine);
        GameManager.Instance.machine.ChangeState(runningState);
    }
    
    public void Return() 
    {
        levelManager.LoadScene("MainMenu");
    }
}
