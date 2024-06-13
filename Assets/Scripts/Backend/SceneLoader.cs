using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int nextSceneIndex;
    public string mode;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadScene();
        }
    }
    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        load.allowSceneActivation = false;
        while (!load.isDone)
        {
            if (load.progress >= 0.9f)
            {
                break;
            }
            yield return null;

        }
        load.completed += (x) =>
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByBuildIndex(nextSceneIndex));
            GameObject[] nextSceneObjects = SceneManager.GetSceneByBuildIndex(nextSceneIndex).GetRootGameObjects();
            GameObject pointToMove = null;
            CinemachineVirtualCamera virtualCamera = null;


            for (int i = 0; i < nextSceneObjects.Length; i++)
            {
                if (nextSceneObjects[i].CompareTag("Respawn") && mode == "respawn")
                {
                    pointToMove = nextSceneObjects[i];
                }
                if (nextSceneObjects[i].name == $"{SceneManager.GetActiveScene().name} Entrance" && mode == "load")
                {
                    pointToMove = nextSceneObjects[i];
                }
                if (nextSceneObjects[i].CompareTag("Camera"))
                {
                    virtualCamera = nextSceneObjects[i].GetComponent<CinemachineVirtualCamera>();
                }
                if (nextSceneObjects[i].CompareTag("Player"))
                {
                    player = nextSceneObjects[i];
                }
            }
            player.transform.position = pointToMove.transform.position;
            virtualCamera.Follow = player.transform;
        };

        load.allowSceneActivation = true;
        while (!load.isDone) { yield return null; }

        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    }
}
