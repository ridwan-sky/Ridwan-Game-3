using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    public GameObject tutorialPanel;

    // =========================
    // NEW GAME
    // =========================
    public void NewGame()
    {
        // Hapus semua save lama
        PlayerPrefs.DeleteKey("score");
        PlayerPrefs.DeleteKey("lives");

        PlayerPrefs.DeleteKey("playerX");
        PlayerPrefs.DeleteKey("playerY");
        PlayerPrefs.DeleteKey("playerZ");

        // Tandai bukan continue
        PlayerPrefs.SetInt("ContinueGame", 0);

        PlayerPrefs.Save();

        // Masuk game baru
        SceneManager.LoadScene("Game");
    }

    // =========================
    // CONTINUE
    // =========================
    public void ContinueGame()
    {
        // Cek apakah ada save
        if (PlayerPrefs.HasKey("playerX"))
        {
            // Tandai ini continue
            PlayerPrefs.SetInt("ContinueGame", 1);

            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Belum ada save game!");
        }
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Debug.Log("Game keluar");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}