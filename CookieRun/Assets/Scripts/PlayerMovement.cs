using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private PlayerSlide playerSlide;
    public int jumpCount;
    public int maxJumpCount = 2;
    public float jumpHeight = 5f;
    public float JumpSpeed = 8f;
    public float deceleration = 20f;
    public float startFallingSpeed = 8f;
    public float fallingSpeed = 8f;
    public float fallingPlus = 1f;
    public float fallingWaitTime = 0.2f;
    public bool isGrounded { get; private set; } = false;
    public bool isJumping { get; private set; } = false;

    private Coroutine currentJumpRoutine;

    private void Start()
    {
        playerSlide = GetComponent<PlayerSlide>();
        rbSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        jumpCount = maxJumpCount;
    }

    private void Update()
    {
        // 점프 처리
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0 && !playerSlide.isSlide)
        {
            if (currentJumpRoutine != null)
            {
                StopCoroutine(currentJumpRoutine);
                ResetJumpState();
            }
            currentJumpRoutine = StartCoroutine(JumpRoutine());
        }

        float rayLength = 0.2f;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            isGrounded = true;
            //Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.green);
            //Debug.Log("[PlayerMovement] Raycast로 땅 감지 성공!");
        }
        else
        {
            isGrounded = false;
            //Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
            //Debug.Log("[PlayerMovement] Raycast로 땅 감지 실패...");
        }

        if (!isGrounded && !isJumping)
        {
            rb.gravityScale = 3f;
        }
        else
        {
            rb.gravityScale = 1f;
        }    
    }

    public void ButtonJump()
    {
        if (jumpCount > 0 && !playerSlide.isSlide)
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
        fallingSpeed = startFallingSpeed;
        rb.gravityScale = 0f;
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
                break;
            }

            rb.position += new Vector2(0, currentSpeed * Time.fixedDeltaTime);
            currentSpeed -= deceleration * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
        if (jumpCount == 1)
        {
            fallingSpeed += fallingSpeed * Time.deltaTime;
            rb.velocity = Vector2.down * fallingSpeed * fallingPlus;
            isJumping = false;
            currentJumpRoutine = null;
        }
        else if (jumpCount <= 0)
        {
            fallingSpeed += fallingSpeed * Time.deltaTime;
            rb.velocity = Vector2.down * fallingSpeed * (fallingPlus + fallingPlus);
            isJumping = false;
            currentJumpRoutine = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"[PlayerMovement] 충돌 감지: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("[PlayerMovement] 땅과 충돌: isGrounded = true");
            isGrounded = true;
            isJumping = false;
            jumpCount = maxJumpCount;
            fallingSpeed = startFallingSpeed;
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
        isJumping = false;
        currentJumpRoutine = null;
    }
}
