//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public float hp;
//    private int jumpCount;
//    public int maxJumpCount = 2;
//    public float jumpPow = 5f; // 점프 높이 결정
//    public float fastRiseMultiplier = 2f; // 초기 속도 보정 값 (더 빠르게 상승)
//    public float reducedGravityTime = 0.1f; // 중력을 줄이는 시간
//    private Rigidbody2D rb;
//    private bool isGround = false;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        jumpCount = maxJumpCount;
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
//        {
//            Jump();
//        }

//        // 하강 시 중력 보정
//        if (rb.velocity.y < 0)
//        {
//            rb.velocity += Vector2.up * Physics2D.gravity.y * (2f - 1) * Time.deltaTime;
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGround = true;
//            jumpCount = maxJumpCount;
//        }
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGround = false;
//        }
//    }

//    private void Jump()
//    {
//        rb.velocity = Vector2.zero; // 기존 속도를 초기화
//        rb.AddForce(Vector2.up * jumpPow * fastRiseMultiplier, ForceMode2D.Impulse); // 초기 속도를 더욱 강하게

//        jumpCount--;

//        // 상승 초기에 중력 영향을 줄임
//        StartCoroutine(TemporarilyReduceGravity());
//    }

//    private IEnumerator TemporarilyReduceGravity()
//    {
//        float originalGravity = rb.gravityScale; // 기존 중력 스케일 저장
//        rb.gravityScale = originalGravity * 0.5f; // 중력을 일시적으로 감소
//        yield return new WaitForSeconds(reducedGravityTime); // 설정한 시간 동안 유지
//        rb.gravityScale = originalGravity; // 중력을 원래대로 복원
//    }
//}


//using System.Collections;
//using UnityEngine;
//
//public class PlayerMovement : MonoBehaviour
//{
//    public float jumpHeight = 3f; // 점프 높이
//    public float jumpDuration = 0.2f; // 점프 상승 시간
//    private bool isJumping = false; // 점프 중인지 확인
//    private bool isGrounded = false; // 바닥에 닿았는지 확인
//    private Rigidbody2D rb; // Rigidbody2D 참조
//
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//    }
//
//    private void Update()
//    {
//        // 점프 입력
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            StartCoroutine(JumpRoutine());
//        }
//    }
//
//    private IEnumerator JumpRoutine()
//    {
//        rb.isKinematic = true; // Rigidbody를 비활성화하여 트랜스폼 점프를 사용
//        isJumping = true;
//
//        float elapsedTime = 0f;
//        float startY = transform.position.y;
//        float targetY = startY + jumpHeight;
//
//        // 트랜스폼 점프 (상승)
//        while (elapsedTime < jumpDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            float newY = Mathf.Lerp(startY, targetY, elapsedTime / jumpDuration);
//            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
//            yield return null;
//        }
//
//        // 최고점에 도달 후 Rigidbody 활성화
//        rb.isKinematic = false; // 물리 활성화
//        rb.velocity = Vector2.zero; // 속도 초기화
//        isJumping = false;
//    }
//
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        // 바닥에 닿았는지 확인
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
//        }
//    }
//
//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        // 바닥에서 떨어졌는지 확인
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = false;
//        }
//    }
//}

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int jumpCount;
    public int maxJumpCount = 2; // 최대 점프 횟수
    public float jumpHeight = 5f; // 점프 높이
    public float initialJumpSpeed = 8f; // 초기 점프 속도
    public float deceleration = 20f; // 점프 감속률
    private bool isGrounded = false; // 바닥에 닿았는지 확인
    private Rigidbody2D rb; // Rigidbody2D 참조
    private bool isJumping = false; // 현재 점프 중인지 확인
    private Coroutine currentJumpRoutine; // 현재 실행 중인 점프 루틴

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount; // 점프 횟수 초기화
    }

    private void Update()
    {
        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            // 기존 점프 루틴 중지 및 초기화
            if (currentJumpRoutine != null)
            {
                StopCoroutine(currentJumpRoutine);
                ResetJumpState(); // 점프 상태 초기화
            }

            // 새로운 점프 시작
            currentJumpRoutine = StartCoroutine(JumpRoutine());
        }
    }

    private IEnumerator JumpRoutine()
    {
        rb.isKinematic = true; // Rigidbody 비활성화
        rb.velocity = Vector2.zero; // Rigidbody 속도 초기화
        isJumping = true; // 점프 중 상태 설정
        jumpCount--; // 점프 횟수 감소

        float currentSpeed = initialJumpSpeed;
        float startY = transform.position.y;
        float targetY = startY + jumpHeight;

        // 상승: 속도를 점진적으로 줄이며 이동
        while (currentSpeed > 0)
        {
            float nextY = transform.position.y + currentSpeed * Time.deltaTime;

            // 목표 높이를 초과하지 않도록 제한
            if (nextY >= targetY)
            {
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                break;
            }

            transform.position += new Vector3(0, currentSpeed * Time.deltaTime, 0);
            currentSpeed -= deceleration * Time.deltaTime; // 속도 감소

            yield return null;
        }

        // 최고점에서 자연스러운 하강 전환
        rb.isKinematic = false; // Rigidbody 활성화
        rb.velocity = Vector2.zero; // Rigidbody 속도 초기화
        isJumping = false; // 점프 상태 해제
        currentJumpRoutine = null; // 점프 루틴 초기화
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = maxJumpCount; // 바닥에 닿으면 점프 횟수 복구
            ResetJumpState(); // 점프 상태 완전 초기화
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 바닥에서 떨어졌는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void ResetJumpState()
    {
        if (rb.isKinematic == false)
        {
            rb.velocity = Vector2.zero; // Rigidbody 속도 초기화
        }
        isJumping = false; // 점프 상태 초기화
        currentJumpRoutine = null; // 실행 중인 루틴 초기화
    }
}



