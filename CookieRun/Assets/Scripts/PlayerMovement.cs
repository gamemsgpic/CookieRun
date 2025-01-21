//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public float hp;
//    private int jumpCount;
//    public int maxJumpCount = 2;
//    public float jumpPow = 5f; // ���� ���� ����
//    public float fastRiseMultiplier = 2f; // �ʱ� �ӵ� ���� �� (�� ������ ���)
//    public float reducedGravityTime = 0.1f; // �߷��� ���̴� �ð�
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

//        // �ϰ� �� �߷� ����
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
//        rb.velocity = Vector2.zero; // ���� �ӵ��� �ʱ�ȭ
//        rb.AddForce(Vector2.up * jumpPow * fastRiseMultiplier, ForceMode2D.Impulse); // �ʱ� �ӵ��� ���� ���ϰ�

//        jumpCount--;

//        // ��� �ʱ⿡ �߷� ������ ����
//        StartCoroutine(TemporarilyReduceGravity());
//    }

//    private IEnumerator TemporarilyReduceGravity()
//    {
//        float originalGravity = rb.gravityScale; // ���� �߷� ������ ����
//        rb.gravityScale = originalGravity * 0.5f; // �߷��� �Ͻ������� ����
//        yield return new WaitForSeconds(reducedGravityTime); // ������ �ð� ���� ����
//        rb.gravityScale = originalGravity; // �߷��� ������� ����
//    }
//}


//using System.Collections;
//using UnityEngine;
//
//public class PlayerMovement : MonoBehaviour
//{
//    public float jumpHeight = 3f; // ���� ����
//    public float jumpDuration = 0.2f; // ���� ��� �ð�
//    private bool isJumping = false; // ���� ������ Ȯ��
//    private bool isGrounded = false; // �ٴڿ� ��Ҵ��� Ȯ��
//    private Rigidbody2D rb; // Rigidbody2D ����
//
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//    }
//
//    private void Update()
//    {
//        // ���� �Է�
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            StartCoroutine(JumpRoutine());
//        }
//    }
//
//    private IEnumerator JumpRoutine()
//    {
//        rb.isKinematic = true; // Rigidbody�� ��Ȱ��ȭ�Ͽ� Ʈ������ ������ ���
//        isJumping = true;
//
//        float elapsedTime = 0f;
//        float startY = transform.position.y;
//        float targetY = startY + jumpHeight;
//
//        // Ʈ������ ���� (���)
//        while (elapsedTime < jumpDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            float newY = Mathf.Lerp(startY, targetY, elapsedTime / jumpDuration);
//            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
//            yield return null;
//        }
//
//        // �ְ����� ���� �� Rigidbody Ȱ��ȭ
//        rb.isKinematic = false; // ���� Ȱ��ȭ
//        rb.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
//        isJumping = false;
//    }
//
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        // �ٴڿ� ��Ҵ��� Ȯ��
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
//        }
//    }
//
//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        // �ٴڿ��� ���������� Ȯ��
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
    public int maxJumpCount = 2; // �ִ� ���� Ƚ��
    public float jumpHeight = 5f; // ���� ����
    public float initialJumpSpeed = 8f; // �ʱ� ���� �ӵ�
    public float deceleration = 20f; // ���� ���ӷ�
    private bool isGrounded = false; // �ٴڿ� ��Ҵ��� Ȯ��
    private Rigidbody2D rb; // Rigidbody2D ����
    private bool isJumping = false; // ���� ���� ������ Ȯ��
    private Coroutine currentJumpRoutine; // ���� ���� ���� ���� ��ƾ

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount; // ���� Ƚ�� �ʱ�ȭ
    }

    private void Update()
    {
        // ���� �Է�
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            // ���� ���� ��ƾ ���� �� �ʱ�ȭ
            if (currentJumpRoutine != null)
            {
                StopCoroutine(currentJumpRoutine);
                ResetJumpState(); // ���� ���� �ʱ�ȭ
            }

            // ���ο� ���� ����
            currentJumpRoutine = StartCoroutine(JumpRoutine());
        }
    }

    private IEnumerator JumpRoutine()
    {
        rb.isKinematic = true; // Rigidbody ��Ȱ��ȭ
        rb.velocity = Vector2.zero; // Rigidbody �ӵ� �ʱ�ȭ
        isJumping = true; // ���� �� ���� ����
        jumpCount--; // ���� Ƚ�� ����

        float currentSpeed = initialJumpSpeed;
        float startY = transform.position.y;
        float targetY = startY + jumpHeight;

        // ���: �ӵ��� ���������� ���̸� �̵�
        while (currentSpeed > 0)
        {
            float nextY = transform.position.y + currentSpeed * Time.deltaTime;

            // ��ǥ ���̸� �ʰ����� �ʵ��� ����
            if (nextY >= targetY)
            {
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                break;
            }

            transform.position += new Vector3(0, currentSpeed * Time.deltaTime, 0);
            currentSpeed -= deceleration * Time.deltaTime; // �ӵ� ����

            yield return null;
        }

        // �ְ������� �ڿ������� �ϰ� ��ȯ
        rb.isKinematic = false; // Rigidbody Ȱ��ȭ
        rb.velocity = Vector2.zero; // Rigidbody �ӵ� �ʱ�ȭ
        isJumping = false; // ���� ���� ����
        currentJumpRoutine = null; // ���� ��ƾ �ʱ�ȭ
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ٴڿ� ��Ҵ��� Ȯ��
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = maxJumpCount; // �ٴڿ� ������ ���� Ƚ�� ����
            ResetJumpState(); // ���� ���� ���� �ʱ�ȭ
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // �ٴڿ��� ���������� Ȯ��
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void ResetJumpState()
    {
        if (rb.isKinematic == false)
        {
            rb.velocity = Vector2.zero; // Rigidbody �ӵ� �ʱ�ȭ
        }
        isJumping = false; // ���� ���� �ʱ�ȭ
        currentJumpRoutine = null; // ���� ���� ��ƾ �ʱ�ȭ
    }
}



