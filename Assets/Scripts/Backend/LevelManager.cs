using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int nextSceneIndex;

    public void LoadScene(Scene nextScene)
    {
        StartCoroutine(LoadSceneAsync(nextScene));
    }

    private IEnumerator LoadSceneAsync(Scene nextScene)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(nextScene.buildIndex, LoadSceneMode.Additive);
        load.allowSceneActivation = false;
        load.completed += (x) => { SceneManager.MoveGameObjectToScene(PlayerManager.instance.gameObject, nextScene); };
        load.allowSceneActivation = true;

        yield return new WaitForSeconds(.2f); 

        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    }
}
