using UnityEngine;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    public PlayerHealth playerHealth;

    void Update()
    {
        if (livesText != null && playerHealth != null)
        {
            livesText.text = "Lives: " + playerHealth.currentLives;
        }
    }
}