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
    public TextMeshProUGUI scoreText; // Slot untuk menarik ScoreText UI dari Hierarchy

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private Vector3 startPosition;
    private Vector3 initialScale;
    private int score = 0; // Variabel penyimpan poin

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        startPosition = transform.position;
        initialScale = transform.localScale; 
        
        rb.gravityScale = 3.5f; 
        rb.freezeRotation = true; 
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        UpdateScoreUI(); // Set tampilan skor awal
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (moveInput > 0) 
        {
            transform.localScale = initialScale; 
        }
        else if (moveInput < 0) 
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 0.9f) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Deteksi jatuh ke air
        if (collision.CompareTag("Water"))
        {
            Respawn();
        }

        // Deteksi menyentuh finish (papan kayu)
        if (collision.CompareTag("Finish"))
        {
            WinGame();
        }

        // LOGIKA MENGAMBIL JAMUR
        if (collision.CompareTag("Mushroom"))
        {
            CollectMushroom(collision.gameObject);
        }
    }

    void CollectMushroom(GameObject mushroom)
    {
        score += 1; // Tambah poin
        UpdateScoreUI(); // Perbarui teks di layar
        Destroy(mushroom); // Hilangkan objek jamur
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Mushroom: " + score; // Format tampilan teks skor
        }
    }

    void WinGame()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true); 
            rb.velocity = Vector2.zero;   
            rb.simulated = false; 
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Respawn()
    {
        transform.position = startPosition;
        rb.velocity = Vector2.zero; 
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}