using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject PlayerUI;
    public GameObject InventoryUI;
    public GameObject PauseMenuUI;
    public GameObject RespawnMenuUI;
    public GameObject TradeMenuUI;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null) { Destroy(gameObject); }
        if (Instance == null) { Instance = this; }
    }
}
