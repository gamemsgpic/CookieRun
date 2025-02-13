using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; // UI 이벤트 처리를 위해 필요

public class PlayerMovement : MonoBehaviour
{
    public AudioManager audioManager;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer rbSprite;
    private PlayerSlide playerSlide;
    private PlayerItemEffects playerItemEffects;
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

    private bool jumpKeyHeld; // 점프 키가 눌린 상태 저장
    private bool jumpKeyUsed; // 점프 키가 눌린 상태 저장

    private bool oneCall = true;

    private bool previousIsGrounded = false; // 이전 프레임에서의 isGrounded 상태

    private void Start()
    {
        playerItemEffects = GetComponent<PlayerItemEffects>();
        animator = GetComponent<Animator>();
        playerSlide = GetComponent<PlayerSlide>();
        rbSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        jumpCount = maxJumpCount;
    }

    private void Update()
    {
        float rayLength = 0.1f;
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

        // **isGrounded가 처음 true가 될 때만 실행 (계속 호출 방지)**
        if (isGrounded && !previousIsGrounded)
        {
            HandleGroundedState();
        }

        // 이전 프레임의 isGrounded 상태 저장
        previousIsGrounded = isGrounded;
    }

    public void ButtonJump()
    {
        if (playerSlide.isSlide) // 슬라이드 중이라면 슬라이드 해제 후 점프 실행
        {
            playerSlide.StopSlide();
        }

        if (jumpCount > 0 && !playerSlide.isSlide)
        {
            audioManager.PlayerJumpSound();
            PerformJump();
        }
    }

    private void PerformJump()
    {
        if (currentJumpRoutine != null)
        {
            StopCoroutine(currentJumpRoutine);
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

    public void SingleJump()
    {
        if (isJumping || jumpCount <= 0) return; // 이미 점프 중이거나 점프 횟수가 0이면 실행하지 않음

        if (currentJumpRoutine != null)
        {
            StopCoroutine(currentJumpRoutine);
        }

        currentJumpRoutine = StartCoroutine(SingleJumpRoutine());
       
    }

    private IEnumerator SingleJumpRoutine()
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

        fallingSpeed += fallingSpeed * Time.deltaTime;
        rb.velocity = Vector2.down * fallingSpeed * fallingPlus;
        isJumping = false;
        isfalling = true;
        currentJumpRoutine = null;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DropArea"))
        {
            if (jumpCount == 2 && !isGrounded && !isJumping && !playerItemEffects.giant)
            {
                rb.isKinematic = true;
                fallingSpeed += fallingSpeed * Time.unscaledDeltaTime;
                rb.velocity = Vector2.down * fallingSpeed * (fallingPlus + fallingPlus);
                isJumping = false;
                isfalling = true;
                currentJumpRoutine = null;
            }
            else
            {
                if (oneCall)
                {
                    rb.velocity = Vector2.zero;
                    oneCall = false;
                }
                rb.isKinematic = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DropArea"))
        {
            oneCall = true;
            rb.isKinematic = false;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //        isJumping = false;
    //        isfalling = false;
    //        jumpCount = maxJumpCount;
    //        fallingSpeed = startFallingSpeed;
    //        rb.velocity = Vector2.zero;
    //        currentJumpRoutine = null;

    //        //// **점프 키가 눌려있고, 추가 점프가 사용되지 않았을 때만 실행**
    //        //if (jumpKeyHeld && !jumpKeyUsed && !playerSlide.isSlide)
    //        //{
    //        //    SingleJump();
    //        //    jumpKeyUsed = true; // 추가 점프 1회 제한 (중복 방지)
    //        //}
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}

    private void HandleGroundedState()
    {
        isJumping = false;
        isfalling = false;
        jumpCount = maxJumpCount;
        fallingSpeed = startFallingSpeed;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0f, Time.deltaTime * 5f)); // 속도를 점진적으로 줄이기
        currentJumpRoutine = null;

        if (jumpKeyHeld && !jumpKeyUsed && !playerSlide.isSlide)
        {
            SingleJump();
            jumpKeyUsed = true;  // 추가 점프 1회 제한
            jumpKeyHeld = false; // 강제로 입력 초기화 (불필요한 점프 방지)
        }
        else
        {
            jumpKeyUsed = false; // 땅에 닿았을 때 점프 다시 가능하도록 초기화
        }
    }

    // 버튼 누를 때 호출 (UI 버튼에 연결)
    public void SetJumpKeyHeldDown()
    {
        jumpKeyHeld = true;
    }

    public void SetJumpKeyHeldUp()
    {
        jumpKeyHeld = false;
        jumpKeyUsed = false; // 버튼을 떼면 추가 점프 가능하도록 초기화
    }
}
