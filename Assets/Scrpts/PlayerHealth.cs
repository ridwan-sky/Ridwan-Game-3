using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives")]
    public int maxLives = 5;      // Jumlah nyawa awal
    public int currentLives;      // Nyawa saat ini

    [Header("UI Game Over")]
    public GameObject gameOverPanel;   // Drag GameOverPanel dari Canvas ke sini

    [Header("Respawn")]
    public Transform respawnPoint;     // Titik respawn (opsional)

    private bool isDead = false;

    void Start()
    {
        // Set nyawa awal
        currentLives = maxLives;

        // Sembunyikan Game Over di awal
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage()
    {
        // Jika sudah game over, abaikan
        if (isDead) return;

        // Kurangi nyawa
        currentLives--;
        Debug.Log("Nyawa tersisa: " + currentLives);

        // Jika nyawa habis -> Game Over
        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            // Jika masih ada nyawa -> Respawn
            Respawn();
        }
    }

    void Respawn()
    {
        // Jika ada titik respawn, pindahkan player ke sana
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            // Jika tidak ada respawn point, kembali ke posisi awal scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void GameOver()
    {
        isDead = true;
        Debug.Log("GAME OVER");

        // Tampilkan panel Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Hentikan permainan
        Time.timeScale = 0f;
    }

    // Jika menyentuh musuh atau air (collider biasa)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Water"))
        {
            TakeDamage();
        }
    }

    // Jika Water menggunakan Is Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            TakeDamage();
        }
    }

    // Tombol Retry pada Game Over
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Tombol Main Menu pada Game Over
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}