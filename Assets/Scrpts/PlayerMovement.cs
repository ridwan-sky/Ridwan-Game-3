using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float jumpForce = 14f;
    public float acceleration = 7f;
    public float decceleration = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    [Header("Screen Boundary")]
    public float minX = -8.5f;
    public float maxX = 8.5f;

    [Header("UI Settings")]
    public GameObject victoryPanel;
    public TextMeshProUGUI scoreText;

    [Header("Lives Settings")]
    public int maxLives = 5;
    public int currentLives;
    public GameObject gameOverPanel;
    public TextMeshProUGUI livesText;

    [Header("Damage Settings")]
    public float damageCooldown = 1f;   // Jeda agar nyawa tidak berkurang berkali-kali

    [Header("Audio Settings")]
    public AudioSource audioSource;

    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip mushroomSound;
    public AudioClip winSound;
    public AudioClip gameOverSound;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private Vector3 startPosition;
    private Vector3 initialScale;
    private int score = 0;

    // Mencegah damage berulang
    private bool isInvincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;
        initialScale = transform.localScale;

        rb.gravityScale = 3.5f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Sembunyikan panel kemenangan
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // Inisialisasi nyawa
        currentLives = maxLives;

        // Sembunyikan Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Update UI
        UpdateScoreUI();
        UpdateLivesUI();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Cek apakah player menyentuh ground
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                checkRadius,
                groundLayer
            );
        }

        // Lompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            // Mainkan suara lompat
            audioSource.PlayOneShot(jumpSound);
        }

        // Flip sprite
        if (moveInput > 0)
        {
            transform.localScale = initialScale;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(
                -initialScale.x,
                initialScale.y,
                initialScale.z
            );
        }

        // Batasi posisi X
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(
            clampedX,
            transform.position.y,
            transform.position.z
        );
    }

    void FixedUpdate()
    {
        // Gerakan halus
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f)
            ? acceleration
            : decceleration;

        float movement = Mathf.Pow(
            Mathf.Abs(speedDif) * accelRate,
            0.9f
        ) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }

    // Trigger untuk Water, Mushroom, Finish, dan Enemy (jika Enemy pakai Is Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            TakeDamage();
        }

        if (collision.CompareTag("Enemy"))
        {
            TakeDamage();
        }

        if (collision.CompareTag("Finish"))
        {
            WinGame();
        }

        if (collision.CompareTag("Mushroom"))
        {
            CollectMushroom(collision.gameObject);
        }
    }

    // Collision untuk Enemy (jika Enemy TIDAK pakai Is Trigger)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    void CollectMushroom(GameObject mushroom)
    {
        score++;
        // Mainkan suara mushroom
        audioSource.PlayOneShot(mushroomSound);
        UpdateScoreUI();
        Destroy(mushroom);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Mushroom: " + score;
        }
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    void TakeDamage()
    {
        // Jika sedang invincible, abaikan damage
        if (isInvincible)
            return;

        // Aktifkan invincible sementara
        isInvincible = true;

        // Kurangi nyawa
        currentLives--;
        // Mainkan suara damage
        audioSource.PlayOneShot(damageSound);
        UpdateLivesUI();

        Debug.Log("Nyawa tersisa: " + currentLives);

        // Jika nyawa habis
        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            Respawn();

            // Matikan invincible setelah beberapa detik
            Invoke(nameof(ResetInvincibility), damageCooldown);
        }
    }

    void ResetInvincibility()
    {
        isInvincible = false;
    }

    void Respawn()
    {
        // Pindahkan player ke posisi awal
        transform.position = startPosition;

        // Hentikan kecepatan
        rb.velocity = Vector2.zero;
    }

    void GameOver()
    {
        // Mainkan suara game over
        audioSource.PlayOneShot(gameOverSound);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Hentikan player
        rb.velocity = Vector2.zero;
        rb.simulated = false;

        // Tampilkan cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void WinGame()
    {
        // Mainkan suara menang
        audioSource.PlayOneShot(winSound);
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            rb.velocity = Vector2.zero;
            rb.simulated = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                checkRadius
            );
        }
    }
}