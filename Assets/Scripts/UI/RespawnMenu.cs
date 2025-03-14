using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnMenu : MonoBehaviour
{
    GameStatemachine machine;
    private void OnEnable()
    {
        if (TryGetComponent(out Canvas canvas) && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    private void Start()
    {
        machine = GameManager.Instance.machine;
    }
    public void Respawn() 
    {
        Level level = GameManager.Instance.LevelManager.levels.Values.FirstOrDefault(level => level.HasFlag("RespawnLevel"));
        PlayerManager.Instance.ResetPlayer();
        machine.ChangeState(level);
    }
}
