using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    public GameManager gm;

    public PlayerState playerState; // PlayerState 컴포넌트를 참조
    public int jumpCount;
    public int maxJumpCount = 2;
    public float jumpHeight = 5f;
    public float JumpSpeed = 8f;
    public float deceleration = 20f;
    public float startFallingSpeed = 8f;
    public float fallingSpeed = 8f;
    public float fallingWaitTime = 0.2f;
    public bool isGrounded { get; private set; } = false;
    public bool isJumping { get; private set; } = false;

    private bool invincibility = false; // 무적 상태 여부
    private Coroutine currentJumpRoutine;

    private float inviStartTime = 0f;
    private float blinkStartTime = 0f;
    public float blinkEndTime = 0.1f;
    public float blinkReEndTime = 0.2f;
    public float inviEndTime = 2.4f;
    public Color color;
    public Color originalColor;

    private void Start()
    {
        gm = GameManager.Instance;
        rbSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // PlayerState를 찾거나 연결 확인
        if (playerState == null)
        {
            playerState = GetComponent<PlayerState>();
            if (playerState == null)
            {
                Debug.LogError("PlayerState가 연결되지 않았습니다. PlayerMovement에서 PlayerState를 확인하세요.");
            }
        }

        jumpCount = maxJumpCount;
        originalColor = rbSprite.color;
        startFallingSpeed = fallingSpeed;
    }

    private void Update()
    {
        // HP 감소 (테스트용)
        playerState.ReduceHp(Time.deltaTime);

        // 점프 처리
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            if (currentJumpRoutine != null)
            {
                StopCoroutine(currentJumpRoutine);
                ResetJumpState();
            }
            currentJumpRoutine = StartCoroutine(JumpRoutine());
        }

        // 무적 상태 처리
        if (invincibility)
        {
            HandleInvincibility();
        }
    }

    private void HandleInvincibility()
    {
        inviStartTime += Time.deltaTime;
        blinkStartTime += Time.deltaTime;

        if (blinkStartTime < blinkEndTime)
        {
            rbSprite.color = color; // 깜빡이는 색상 적용
        }
        else if (blinkStartTime > blinkEndTime && blinkStartTime < blinkReEndTime)
        {
            rbSprite.color = originalColor; // 원래 색상 복원
        }
        else
        {
            blinkStartTime = 0f; // 깜빡임 리셋
        }

        if (inviStartTime > inviEndTime)
        {
            rbSprite.color = originalColor; // 원래 색상 복원
            invincibility = false; // 무적 상태 종료
            inviStartTime = 0f;
            blinkStartTime = 0f;
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

        fallingSpeed += fallingSpeed + 0.5f * Time.deltaTime;
        rb.velocity = Vector2.down * fallingSpeed;
        isJumping = false;
        currentJumpRoutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincibility && collision.CompareTag("Trap"))
        {
            invincibility = true; // 무적 상태 시작
            playerState.ReduceHp(10f); // 데미지로 HP 감소
        }

        if (collision.CompareTag("Coin"))
        {
            CoinMoveTest coin = collision.gameObject.GetComponent<CoinMoveTest>();
            if (coin != null)
            {
                playerState.AddScore(coin.Score);
                playerState.AddCoins(coin.Coins);
                gm.UpdateScore(playerState.score);
            }
        }
    }
}
