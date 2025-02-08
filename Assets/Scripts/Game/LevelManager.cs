using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static readonly GameStatemachine machine = GameManager.Instance.machine;
    public Dictionary<string, Level> levels = new()
    {
        {"A1", new Demo1(machine) }
    };

    public static async Task LoadScene(int nextSceneIndex, bool movePlayer = true)
    {
        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        load.allowSceneActivation = false;
        load.completed += (x) =>
        {
            if (movePlayer)
            {
                try { SceneManager.MoveGameObjectToScene(PlayerManager.instance.gameObject, SceneManager.GetSceneByBuildIndex(nextSceneIndex)); }
                catch { Debug.Log("Moving the player failed."); }
            }
        };
        load.allowSceneActivation = true;
        while (!load.isDone) await Task.Yield();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    }

    public static async Task LoadScene(string nextSceneName, bool movePlayer = true)
    {
        int nextSceneIndex = SceneManager.GetSceneByName(nextSceneName).buildIndex;
        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        load.allowSceneActivation = false;
        load.completed += (x) =>
        {
            if (movePlayer)
            {
                try { SceneManager.MoveGameObjectToScene(PlayerManager.instance.gameObject, SceneManager.GetSceneByBuildIndex(nextSceneIndex)); }
                catch { Debug.Log("Moving the player failed."); }
            }
        };
        load.allowSceneActivation = true;
        while (!load.isDone) await Task.Yield();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    }

    public Level GetLevel(string ID)
    {
        try
        {
            return levels[ID];
        }
        catch 
        {
            Debug.Log("Attempted to fetch a non-existing level.");
            return null; 
        }
    }
}
