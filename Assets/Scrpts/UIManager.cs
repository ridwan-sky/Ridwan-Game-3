using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public void OpenMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // pause game
    }

    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // lanjut game
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f; // WAJIB supaya game tidak freeze
        SceneManager.LoadScene("MainMenu");
    }
}