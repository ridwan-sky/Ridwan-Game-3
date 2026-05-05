using UnityEngine;
using UnityEngine.SceneManagement; // Penting untuk perpindahan scene

public class LevelManager : MonoBehaviour
{
    // Fungsi untuk tombol NEXT (Pindah ke level berikutnya)
    public void NextLevel()
    {
        // Mengambil index scene saat ini dan menambah 1 untuk ke scene selanjutnya
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Cek jika scene selanjutnya ada dalam Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Sudah di level terakhir!");
            // Bisa diarahkan kembali ke Main Menu jika level habis
            SceneManager.LoadScene("MainMenu"); 
        }
    }

    // Fungsi untuk tombol MENU (Kembali ke menu utama)
    public void GoToMainMenu()
    {
        // Ganti "MainMenu" sesuai dengan nama scene menu Anda
        SceneManager.LoadScene("MainMenu"); 
    }

    // Fungsi untuk tombol RESTART (Jika ingin menambahkan tombol ulang)
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}