using System.IO;
using System.Linq;
using UnityEngine;

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
    //public void BackToMenu()
    //{
    //    //DataManager manager = GameManager.Instance.DataManager;
    //    //using (var writer = new StreamWriter(GameManager.Instance.DataManager.SavePath, false))
    //    //{
    //    //    manager.SaveFile(manager.Save(), manager.SaveIndex, writer);
    //    //    writer.Flush();
    //    //    writer.Close();
    //    //    writer.Dispose();
    //    //}

    //    GameMainMenuState mainmenu = new(GameManager.Instance.machine);
    //    GameManager.Instance.machine.ChangeState(mainmenu);
    //}

}
