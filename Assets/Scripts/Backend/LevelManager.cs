using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int nextSceneIndex;
    public string mode;
    private GameObject player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            LoadScene();
        }
    }
    public void LoadScene()
    {
        mode = "load";
        StartCoroutine(LoadSceneAsync(mode));
    }

    public void OnPlayerRespawn()
    {
        mode = "respawn";
        player = GameObject.FindGameObjectWithTag("Player");
        nextSceneIndex = player.GetComponent<PlayerManager>().respawnPointScene;
        if (nextSceneIndex == SceneManager.GetActiveScene().buildIndex)
        {
            GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
            transform.position = respawnPoint.transform.position;
        }
        else { StartCoroutine(LoadSceneAsync(mode)); }
    }
    private IEnumerator LoadSceneAsync(string mode)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        load.allowSceneActivation = false;
        //while (!load.isDone)
        //{
        //    if (load.progress >= 0.9f)
        //    {
        //        break;
        //    }
        //    yield return null;

        //}
        load.completed += (x) =>
        {

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
            //virtualCamera.Follow = player.transform;
        };

        load.allowSceneActivation = true;
        while (!load.isDone) { yield return null; }

        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(oldSceneIndex));
    }
}
