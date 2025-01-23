using UnityEngine;

public class CoinMoveTest : MonoBehaviour
{
    public float currentSpeed = 2f; // �⺻ ���� �̵� �ӵ�
    public int Score = 100;
    public int Coins = 100;

    public bool isMovingToPlayer { get; private set; } = false; // Player�� �̵� ����
    private Vector3 startPosition; // ������ �ʱ� ��ġ
    private Vector3 magnetStartPosition; // ������ �ʱ� ��ġ
    private Transform targetTransform; // ��ǥ ��ġ (�÷��̾� Transform)
    private float attractionSpeed = 5f; // Player�� ���� �̵� �ӵ�

    private float currentTime = 0f;
    public float moveDuration = 0.5f; // �̵� �ð� (Lerp �ӵ�)

    private void Start()
    {
        startPosition = transform.position; // �ʱ� ��ġ ����
    }

    // Magnet�� �浹 �� ȣ��
    public void MoveTowardsPlayer(Transform target, float speed)
    {
        isMovingToPlayer = true;
        targetTransform = target; // ��ǥ ��ġ ����
        attractionSpeed = speed; // �ӵ� ����
        magnetStartPosition = transform.position; // �̵� ���� ��ġ ����
        currentTime = 0f; // �̵� �ð� �ʱ�ȭ
    }

    private void Update()
    {
        if (isMovingToPlayer && targetTransform != null)
        {
            // Lerp�� ����� �ε巴�� �̵�
            currentTime += Time.deltaTime;
            transform.position = Vector2.Lerp(magnetStartPosition, targetTransform.position + (Vector3.up * 1f), currentTime / moveDuration);

            // �̵� �Ϸ� ����
            if (currentTime >= moveDuration)
            {
                Debug.Log("Player�� ����!");
                isMovingToPlayer = false; // �̵� ���� ����
            }
        }
        else
        {
            // Magnet�� ������ ���� ���� �� �������� �̵�
            transform.Translate(Vector2.left * currentSpeed * Time.deltaTime, Space.World);

            // ȭ�� ������ ������ �ʱ� ��ġ�� ����
            if (transform.position.x < -15f)
            {
                ResetPosition();
            }
        }
    }

    public void ResetPosition()
    {
        Debug.Log($"Item {gameObject.name} �ʱ�ȭ");
        transform.position = startPosition;
        isMovingToPlayer = false;
    }
}
