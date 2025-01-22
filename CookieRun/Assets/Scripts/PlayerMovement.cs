using System.Collections;
using UnityEngine;
using static Unity.Collections.Unicode;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    public int Score { get; private set; }
    public int jumpCount;
    public int maxJumpCount = 2;
    public float jumpHeight = 5f;
    public float JumpSpeed = 8f;
    public float deceleration = 20f;
    public float gravityVelue = 0f;
    //public float fallingSpeed = 8f;
    //public float fallingWaitTime = 0.2f;
    public bool isGrounded { get; private set; } = false;
    public bool isJumping { get; private set; } = false;
    private bool invincibility = false;
    private bool highestHeight;
    private Coroutine currentJumpRoutine;

    private float inviStartTime = 0f;
    private float blinkStartTime = 0f;
    public float blinkEndTime = 0.1f;
    public float blinkReEndTime = 0.2f;
    public float inviEndTime = 2.4f;
    public Color color;
    public Color originalColor;
    //public Color color; 이거 나중에 쓰자

    private void Start()
    {
        rbSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityVelue;
        jumpCount = maxJumpCount;
        originalColor = rbSprite.color;
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

        if (invincibility)
        {
            inviStartTime += Time.deltaTime;
            blinkStartTime += Time.deltaTime;

            Debug.Log("시간이 돈다!");

            if (blinkStartTime < blinkEndTime)
            {
                rbSprite.color = color;
                Debug.Log("컬러가 바꼈다!");
            }
            else if (blinkStartTime > blinkEndTime && blinkStartTime < blinkReEndTime)
            {
                rbSprite.color = originalColor;
                Debug.Log("컬러가 돌아왔다!");
            }
            else
            {
                blinkStartTime = 0f;
            }

            if (inviStartTime > inviEndTime)
            {
                rbSprite.color = originalColor;
                invincibility = false;
                inviStartTime = 0f;
                blinkStartTime = 0f;
            }
        }

    }

    private IEnumerator JumpRoutine()
    {
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
                highestHeight = true;
                break;
            }

            rb.position += new Vector2(0, currentSpeed * Time.fixedDeltaTime);
            currentSpeed -= deceleration * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        //yield return new WaitForSeconds(0.08f);
        //
        //if (highestHeight)
        //{
        //    float waitTime = 0f;
        //    waitTime += Time.deltaTime;
        //    if (waitTime >= fallingWaitTime)
        //    {
        //        highestHeight = false;
        //    }
        //}
        //else
        //{
        //    rb.velocity = Vector2.down * fallingSpeed;
        //    isJumping = false;
        //    currentJumpRoutine = null;
        //}
        rb.gravityScale = gravityVelue;
        isJumping = false;
        currentJumpRoutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = maxJumpCount;
            ResetJumpState();
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            Score += 100;
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

    private void OnDamage()
    {
        Debug.Log("데미지 받았다!");
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (!invincibility)
        {
            if (collision.CompareTag("Trap"))
            {
                invincibility = true;
                OnDamage();
                Debug.Log("부딪쳤다!");
            }
        }
    }
}


