using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    public void NewGame()
    {
        PlayerPrefs.SetInt("HasSave", 1);
        SceneManager.LoadScene("Game");
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("HasSave", 0) == 1)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Belum ada save!");
        }
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
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