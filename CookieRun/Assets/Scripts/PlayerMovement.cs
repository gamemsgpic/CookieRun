using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; // UI 이벤트 처리를 위해 필요

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
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
    public bool isfalling { get; private set; } = false;

    private Coroutine currentJumpRoutine;

    private bool jumpKeyHeld = false; // 점프 키가 눌린 상태 저장
    private bool jumpKeyUsed = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerSlide = GetComponent<PlayerSlide>();
        rbSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        jumpCount = maxJumpCount;

        jumpKeyHeld = false;
        jumpKeyUsed = false;
    }

    public void ResetJumpKeyHeld()
    {
        jumpKeyHeld = false;
        jumpKeyUsed = true;
    }

    private void Update()
    {
        // 키보드 입력도 감지하여 유지
        jumpKeyHeld = Input.GetKey(KeyCode.Space) || jumpKeyHeld;

        // 점프 처리
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0 && !playerSlide.isSlide)
        {
            PerformJump();
            jumpKeyUsed = false;

        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpKeyUsed = true;
        }

        float rayLength = 0.2f;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

        isGrounded = hit.collider != null;

        if (!isGrounded && !isJumping)
        {
            rb.gravityScale = 3f;
        }
        else if (isGrounded)
        {
            animator.SetTrigger("Ground");
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            rb.gravityScale = 1f;
        }

        if (jumpCount == 1)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("DoubleJump", false);
        }
        else if (jumpCount == 0)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", true);
        }

        if (isfalling)
        {
            animator.SetTrigger("Falling");
        }
    }

    public void ButtonJump()
    {
        if (jumpCount > 0 && !playerSlide.isSlide)
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        if (currentJumpRoutine != null)
        {
            StopCoroutine(currentJumpRoutine);
            ResetJumpState();
        }
        currentJumpRoutine = StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        fallingSpeed = startFallingSpeed;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        isJumping = true;
        isfalling = false;
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
            isfalling = true;
            currentJumpRoutine = null;
        }
        else if (jumpCount <= 0)
        {
            fallingSpeed += fallingSpeed * Time.deltaTime;
            rb.velocity = Vector2.down * fallingSpeed * (fallingPlus + fallingPlus);
            isJumping = false;
            isfalling = true;
            currentJumpRoutine = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
            isfalling = false;
            jumpCount = maxJumpCount;
            fallingSpeed = startFallingSpeed;
            ResetJumpState();

            // 버튼 또는 키보드가 눌린 상태라면 자동으로 점프
            if (jumpKeyHeld && !jumpKeyUsed)
            {
                PerformJump();
                jumpKeyUsed = true;
            }
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

    // 버튼 누를 때 호출 (UI 버튼에 연결)
    public void SetJumpKeyHeld(bool isHeld)
    {
        jumpKeyHeld = isHeld;
        jumpKeyUsed = false;
    }
}
