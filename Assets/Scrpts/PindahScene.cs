using UnityEngine;
using UnityEngine.SceneManagement; // Ini wajib ada untuk pindah scene

public class PindahScene : MonoBehaviour
{
    // Fungsi ini akan dipanggil saat tombol New Game diklik
    public void MulaiPermainan()
    {
        // Pastikan tulisan "Game" sama persis dengan nama scene di Build Settings Anda
        SceneManager.LoadScene("Game");
    }

    // Fungsi untuk tombol Exit
    public void KeluarGame()
    {
        Debug.Log("Game Ditutup");
        Application.Quit();
    }
}