using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    public Image[] HpImages;
    public TextMeshProUGUI ManaCounter;

    private void Start()
    {
        PlayerManager.Instance.UpdateHealth();
        PlayerManager.Instance.UpdateMana();
    }
}

