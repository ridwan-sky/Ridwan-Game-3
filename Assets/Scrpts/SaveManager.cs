using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // =========================
    // SAVE SCORE
    // =========================
    public static void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
    }

    public static int LoadScore()
    {
        return PlayerPrefs.GetInt("score", 0);
    }

    // =========================
    // SAVE LIVES
    // =========================
    public static void SaveLives(int lives)
    {
        PlayerPrefs.SetInt("lives", lives);
    }

    public static int LoadLives()
    {
        return PlayerPrefs.GetInt("lives", 5);
    }

    // =========================
    // SAVE POSITION
    // =========================
    public static void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("playerX", position.x);
        PlayerPrefs.SetFloat("playerY", position.y);
        PlayerPrefs.SetFloat("playerZ", position.z);

        PlayerPrefs.Save();
    }

    public static Vector3 LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat("playerX", 0);
        float y = PlayerPrefs.GetFloat("playerY", 0);
        float z = PlayerPrefs.GetFloat("playerZ", 0);

        return new Vector3(x, y, z);
    }
}