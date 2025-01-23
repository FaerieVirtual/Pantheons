using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadScene(int nextSceneIndex, bool movePlayer = true)
    {
        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        load.allowSceneActivation = false;
        load.completed += (x) =>
        {
            if (movePlayer)
            {
                try { SceneManager.MoveGameObjectToScene(PlayerManager.instance.gameObject, SceneManager.GetSceneByBuildIndex(nextSceneIndex)); }
                catch { Debug.Log("Moving the player failed. Not moving from a menu."); }
            }
        };
        WaitForSecondsRealtime wait = new(0.1f)
        load.allowSceneActivation = true;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    }

    public void LoadScene(string nextSceneName, bool movePlayer = true)
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
                catch { Debug.Log("Moving the player failed. Not moving from a menu."); }
            }
        };
        load.allowSceneActivation = true;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    }

    //private IEnumerator LoadSceneAsync(int sceneIndex)
    //{
    //    AsyncOperation load = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    //    load.allowSceneActivation = false;
    //    load.completed += (x) => { SceneManager.MoveGameObjectToScene(PlayerManager.Instance.gameObject, nextScene); };
    //    load.allowSceneActivation = true;

    //    yield return new WaitForSeconds(.2f); 

    //    int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
    //    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    //}
}
