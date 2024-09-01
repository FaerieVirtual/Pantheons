
using UnityEngine;
using UnityEngine.SceneManagement;


public class Entrance : MonoBehaviour
{
    private LevelManager levelManager;
    public Scene nextArea;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            levelManager.LoadScene(nextArea);

            foreach (GameObject obj in nextArea.GetRootGameObjects())
            {
                if (obj.GetComponent<Entrance>() != null && obj.name == $"{nextArea.name}")
                {
                    PlayerManager.instance.transform.position = obj.transform.position;
                }
            }
        }
    }

    
}

