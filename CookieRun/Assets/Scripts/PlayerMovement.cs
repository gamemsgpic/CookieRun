using System.Collections;
using UnityEngine;
using static Unity.Collections.Unicode;

public class PlayerMovement : MonoBehaviour
{
    public int jumpCount;
    public int maxJumpCount = 2;
    public float jumpHeight = 5f;
    public float JumpSpeed = 8f;
    public float deceleration = 20f;
    public float fallingSpeed = 8f;
    public float fallingWaitTime = 0.2f;
    public bool isGrounded { get; private set; } = false;
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool highestHeight;
    private Coroutine currentJumpRoutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        jumpCount = maxJumpCount;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {

            if (currentJumpRoutine != null)
            {
                StopCoroutine(currentJumpRoutine);
                ResetJumpState();
            }

            currentJumpRoutine = StartCoroutine(JumpRoutine());
        }
    }

    private IEnumerator JumpRoutine()
    {
        rb.velocity = Vector2.zero;
        isJumping = true;
        jumpCount--;
        float currentSpeed = JumpSpeed;
        float startY = transform.position.y;
        float targetY = startY + jumpHeight;

        while (currentSpeed > 0)
        {
            float nextY = rb.position.y + currentSpeed * Time.fixedDeltaTime;

            if (nextY >= targetY)
            {
                rb.position = new Vector2(rb.position.x, targetY);
                highestHeight = true;
                break;
            }

            rb.position += new Vector2(0, currentSpeed * Time.fixedDeltaTime);
            currentSpeed -= deceleration * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        if (highestHeight)
        {
            float waitTime = 0f;
            waitTime += Time.deltaTime;
            if (waitTime >= fallingWaitTime)
            {
                highestHeight = false;
            }
        }
        else
        {
            rb.velocity = Vector2.down * fallingSpeed;
            isJumping = false;
            currentJumpRoutine = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = maxJumpCount;
            ResetJumpState();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void ResetJumpState()
    {

        rb.velocity = Vector2.zero;
        highestHeight = false;
        isJumping = false;
        currentJumpRoutine = null;
    }
}


