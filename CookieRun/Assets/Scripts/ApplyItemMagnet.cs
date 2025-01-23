using UnityEngine;

public class ApplyItemMagnet : MonoBehaviour
{
    public int Score = 100;
    public int Coins = 100;

    public bool isMovingToPlayer { get; private set; } = false; // Player�� �̵� ����
    private Vector3 magnetStartPosition;
    private Transform targetTransform; // ��ǥ ��ġ (�÷��̾� Transform)
    private float attractionSpeed = 5f; // Player�� ���� �̵� �ӵ�

    private float currentTime = 0f;
    public float moveDuration = 0.5f; // �̵� �ð� (Lerp �ӵ�)

    private Vector3 initialLocalPosition; // �������� �ʱ� ���� ��ġ

    private void Awake()
    {
        // Awake���� �������� �ʱ� ���� ��ġ�� ����
        initialLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        // ������ ��Ȱ��ȭ �� �ʱ�ȭ
        ResetItem();
    }

    public void ResetItem()
    {
        isMovingToPlayer = false; // �̵� ���� �ʱ�ȭ
        transform.localPosition = initialLocalPosition; // ����� �ʱ� ���� ��ġ�� ����
        gameObject.SetActive(true); // ������ Ȱ��ȭ
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
                isMovingToPlayer = false;
            }
        }
    }

    //// �÷��̾���� �浹 ó��
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player")) // Player �±� Ȯ��
    //    {
    //        Debug.Log("Player�� �浹, ������ ��Ȱ��ȭ!");
    //        gameObject.SetActive(false); // ������ ��Ȱ��ȭ
    //    }
    //}
}
