using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameStateMachine machine;

    public Dictionary<string, Level> Levels;
    private void Start()
    {
        machine = GameManager.Instance.Machine;
        InitializeLevels();
    }

    #region LoadScenes
    public static async Task LoadScene(int nextSceneIndex)
    {
        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);

        load.allowSceneActivation = false;
        while (load.progress < 0.9f) await Task.Yield();
        load.allowSceneActivation = true;
        while (!load.isDone) await Task.Yield();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(oldSceneIndex);
    }

    public static async Task LoadScene(string nextSceneName)
    {
        int nextSceneIndex = SceneUtility.GetBuildIndexByScenePath(nextSceneName);
        await LoadScene(nextSceneIndex);
    }

    public static async Task LoadScene(int nextSceneIndex, LevelConnector connector) 
    {
        int oldSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);

        load.allowSceneActivation = false;
        while (load.progress < 0.9f) await Task.Yield();
        load.allowSceneActivation = true;
        while (!load.isDone) await Task.Yield();

        SceneManager.MoveGameObjectToScene(PlayerManager.Instance.gameObject, SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        Transform playerTrans = PlayerManager.Instance.GetComponent<Transform>();

        LevelConnector sisterconnector = FindObjectsOfType<LevelConnector>()
            .FirstOrDefault(sisterconnector => connector.SisterConnectorID == sisterconnector.ConnectorID);
        PlayerManager.Instance.GetComponent<Transform>().position = sisterconnector.GetSpawnpoint();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        SceneManager.UnloadSceneAsync(oldSceneIndex);
    }
    public static async Task LoadScene(string nextSceneName, LevelConnector connector) 
    {
        int nextSceneIndex = SceneUtility.GetBuildIndexByScenePath(nextSceneName);
        await LoadScene(nextSceneIndex, connector);
    }
    #endregion

    #region LevelManagement
    private void InitializeLevels()
    {
        Levels = new()
        {
            { "A1", new Demo1(machine) },
            { "A2", new Demo2(machine) },
            { "A3", new Demo3(machine) },
            { "A4", new Demo4(machine) },
        };
    }

    public Level GetLevelByID(string ID)
    {
        try { return Levels[ID]; }
        catch
        {
            Debug.Log("Attempted to fetch a non-existing level.");
            return null;
        }
    }
    public Level GetLevelByFlag(string flag) 
    { 

        foreach (Level level in Levels.Values) 
        {
            if (level.HasFlag(flag)) return level;
        }
        Debug.Log("Attempted to fetch a level with a non-existing flag.");
        return null;
    }
    #endregion
}
