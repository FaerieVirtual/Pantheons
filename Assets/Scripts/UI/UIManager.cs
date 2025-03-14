using UnityEngine.SceneManagement;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null) { Destroy(gameObject); }
        if (instance == null) { instance = this; }
    }
    //private void Start()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Camera camera = Camera.main;
    //    foreach (Canvas canvas in GetComponentsInChildren<Canvas>(true)) 
    //    {
    //        Debug.Log($"got canvas in child {canvas}");
    //        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null) 
    //        {
                
    //            bool wasInactive = false;
    //            if (!canvas.gameObject.activeSelf)
    //            {
    //                canvas.gameObject.SetActive(true);
    //                wasInactive = true;
    //            }

    //            Debug.Log($"Canvas doesn't have a camera, assigning {camera}");
    //            canvas.worldCamera = camera;

    //            if (wasInactive) { canvas.gameObject.SetActive(false); }
    //        }
    //    }
    //}
    //public void UpdateCanvasCameras() 
    //{ 
    
    //}

}
