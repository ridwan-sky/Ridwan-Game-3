using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform groundCheck;
    public float distance = 0.5f;
    public LayerMask groundLayer;

    // Layer untuk objek penghalang (misalnya Box)
    public LayerMask obstacleLayer;

    private bool movingLeft = true;

    void Update()
    {
        // 1. Gerakkan musuh
        if (movingLeft)
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.right * speed * Time.deltaTime);

        // 2. Cek apakah masih ada tanah di depan
        RaycastHit2D groundHit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            distance,
            groundLayer
        );

        // 3. Cek apakah ada penghalang di depan
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;

        RaycastHit2D obstacleHit = Physics2D.Raycast(
            groundCheck.position,
            direction,
            0.2f,
            obstacleLayer
        );

        // 4. Jika tidak ada tanah ATAU ada penghalang → balik arah
        if (groundHit.collider == null || obstacleHit.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingLeft = !movingLeft;

        // Membalik sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Pindahkan GroundCheck ke sisi sebaliknya
        Vector3 gcPos = groundCheck.localPosition;
        gcPos.x *= -1;
        groundCheck.localPosition = gcPos;
    }

    // Menampilkan garis Raycast di Scene View
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            // Raycast ke bawah (cek tanah)
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                groundCheck.position,
                groundCheck.position + Vector3.down * distance
            );

            // Raycast ke samping (cek penghalang)
            Gizmos.color = Color.blue;
            Vector3 dir = movingLeft ? Vector3.left : Vector3.right;
            Gizmos.DrawLine(
                groundCheck.position,
                groundCheck.position + dir * 0.2f
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Flip();
        }
    }
}
