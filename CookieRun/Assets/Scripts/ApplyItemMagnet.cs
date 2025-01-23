using UnityEngine;

public class ApplyItemMagnet : MonoBehaviour
{
    public int Score = 100;
    public int Coins = 100;

    public bool isMovingToPlayer { get; private set; } = false; // Player로 이동 여부
    private Vector3 magnetStartPosition;
    private Transform targetTransform; // 목표 위치 (플레이어 Transform)
    private float attractionSpeed = 5f; // Player를 향한 이동 속도

    private float currentTime = 0f;
    public float moveDuration = 0.5f; // 이동 시간 (Lerp 속도)

    private Vector3 initialLocalPosition; // 아이템의 초기 로컬 위치

    private void Awake()
    {
        // Awake에서 아이템의 초기 로컬 위치를 저장
        initialLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        // 프리팹 재활성화 시 초기화
        ResetItem();
    }

    public void ResetItem()
    {
        isMovingToPlayer = false; // 이동 상태 초기화
        transform.localPosition = initialLocalPosition; // 저장된 초기 로컬 위치로 복원
        gameObject.SetActive(true); // 아이템 활성화
    }

    // Magnet과 충돌 시 호출
    public void MoveTowardsPlayer(Transform target, float speed)
    {
        isMovingToPlayer = true;
        targetTransform = target; // 목표 위치 설정
        attractionSpeed = speed; // 속도 설정
        magnetStartPosition = transform.position; // 이동 시작 위치 갱신
        currentTime = 0f; // 이동 시간 초기화
    }

    private void Update()
    {
        if (isMovingToPlayer && targetTransform != null)
        {
            // Lerp를 사용해 부드럽게 이동
            currentTime += Time.deltaTime;
            transform.position = Vector2.Lerp(magnetStartPosition, targetTransform.position + (Vector3.up * 1f), currentTime / moveDuration);

            // 이동 완료 조건
            if (currentTime >= moveDuration)
            {
                Debug.Log("Player에 도달!");
                isMovingToPlayer = false;
            }
        }
    }

    //// 플레이어와의 충돌 처리
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player")) // Player 태그 확인
    //    {
    //        Debug.Log("Player와 충돌, 아이템 비활성화!");
    //        gameObject.SetActive(false); // 아이템 비활성화
    //    }
    //}
}
