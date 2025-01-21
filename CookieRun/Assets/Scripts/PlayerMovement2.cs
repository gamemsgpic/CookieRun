using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float hp;
    private int jumpCount;
    public int maxJumpCount = 2;
    public float jumpPow = 5f; // ���� ���� ����
    public float fastRiseMultiplier = 2f; // �ʱ� �ӵ� ���� �� (�� ������ ���)
    public float reducedGravityTime = 0.1f; // �߷��� ���̴� �ð�
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

        // �ϰ� �� �߷� ����
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
        rb.velocity = Vector2.zero; // ���� �ӵ��� �ʱ�ȭ
        rb.AddForce(Vector2.up * jumpPow * fastRiseMultiplier, ForceMode2D.Impulse); // �ʱ� �ӵ��� ���� ���ϰ�

        jumpCount--;

        // ��� �ʱ⿡ �߷� ������ ����
        StartCoroutine(TemporarilyReduceGravity());
    }

    private IEnumerator TemporarilyReduceGravity()
    {
        float originalGravity = rb.gravityScale; // ���� �߷� ������ ����
        rb.gravityScale = originalGravity * 0.5f; // �߷��� �Ͻ������� ����
        yield return new WaitForSeconds(reducedGravityTime); // ������ �ð� ���� ����
        rb.gravityScale = originalGravity; // �߷��� ������� ����
    }
}