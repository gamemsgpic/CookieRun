using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float hp;
    private int jumpCount;
    public int maxJumpCount = 2;
    public float jumpPow = 5f; // 점프 높이 결정
    public float fastRiseMultiplier = 2f; // 초기 속도 보정 값 (더 빠르게 상승)
    public float reducedGravityTime = 0.1f; // 중력을 줄이는 시간
    private Rigidbody2D rb;
    private bool isGround = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            Jump();
        }

        // 하강 시 중력 보정
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2f - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = maxJumpCount;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.zero; // 기존 속도를 초기화
        rb.AddForce(Vector2.up * jumpPow * fastRiseMultiplier, ForceMode2D.Impulse); // 초기 속도를 더욱 강하게

        jumpCount--;

        // 상승 초기에 중력 영향을 줄임
        StartCoroutine(TemporarilyReduceGravity());
    }

    private IEnumerator TemporarilyReduceGravity()
    {
        float originalGravity = rb.gravityScale; // 기존 중력 스케일 저장
        rb.gravityScale = originalGravity * 0.5f; // 중력을 일시적으로 감소
        yield return new WaitForSeconds(reducedGravityTime); // 설정한 시간 동안 유지
        rb.gravityScale = originalGravity; // 중력을 원래대로 복원
    }
}