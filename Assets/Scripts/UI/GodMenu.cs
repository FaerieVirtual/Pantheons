using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GodMenu : MonoBehaviour, IMenuBase
{
    public God god;

    public List<RectTransform> cards; 
    public ScrollRect scrollRect;
    private float snapSpeed = 10f; 
    //public float scaleFactor = 0.9f; 

    private int currentIndex = 2; 
    private bool isSnapping = false;

    private void Start()
    {
        //UpdateCardVisuals();
    }

    private void Update()
    {
        if (isSnapping)
        {
            //float targetX = -cards[currentIndex].anchoredPosition.x;
            //float newX = Mathf.Lerp(scrollRect.content.anchoredPosition.x, targetX, Time.deltaTime * snapSpeed);
            //scrollRect.content.anchoredPosition = new Vector2(newX, scrollRect.content.anchoredPosition.y);

            //if (Mathf.Abs(scrollRect.content.anchoredPosition.x - targetX) < 0.1f)
            //{
            //    scrollRect.content.anchoredPosition = new Vector2(targetX, scrollRect.content.anchoredPosition.y);
            //    isSnapping = false;
            //}

        }
    }

    public void ScrollLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            isSnapping = true;
            //UpdateCardVisuals();
        }
    }

    public void ScrollRight()
    {
        if (currentIndex < 4)//cards.Count - 1)
        {
            currentIndex++;
            isSnapping = true;
            //UpdateCardVisuals();
        }
    }

    //private void UpdateCardVisuals()
    //{
    //    for (int i = 0; i < cards.Count; i++)
    //    {
    //        float scale = (i == currentIndex) ? 1f : scaleFactor;

    //        cards[i].localScale = Vector3.one * scale;
    //    }
    //}
    public void Confirm()
    {
        switch (currentIndex)
        {
            case 0:
                Game.god = null; break;
            case 1:
                Game.god = new Heavens(); break;
            case 2:
                Game.god = new Chance(); break;
            case 3:
                Game.god = new Traveler(); break;
            case 4:
                Game.god = null; break;
        }
        Debug.Log("Máme boha!" + god);
        if (Game.god != null) { SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(2).name); }
    }

    public void Options()
    {
        throw new NotImplementedException();
    }

    public void Quit()
    {
        throw new NotImplementedException();
    }
}

public class God
{


}
public class Heavens : God
{ 
    
}
public class Chance : God 
{ 

}

public class Traveler : God 
{ 

}