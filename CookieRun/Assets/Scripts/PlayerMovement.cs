using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float hp;
    private int jumpCount;
    public int maxJumpCount = 2;
    public float jumpPow;
    public float jumpTime;
    private Vector2 startPos;
    private int Score = 0;
    private Rigidbody2D rb;

    private bool isGround = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount;
        jumpPow = 8f;
        jumpTime = 0.5f;
    }

    private void Update()
    {
        startPos = gameObject.transform.position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount > 0)
            {
                Jump();
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2f - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"));
        {
            isGround = true;
            jumpCount = maxJumpCount;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
    }

    private void Jump()
    {
        gameObject.transform.position = startPos * Vector2.up * jumpPow * Time.deltaTime;
        //rb.velocity = Vector3.zero;
        //rb.AddForce(Vector2.up * jumpPow, ForceMode2D.Impulse);
        jumpCount--;
    }
}
