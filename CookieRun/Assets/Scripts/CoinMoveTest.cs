using UnityEngine;

public class CoinMoveTest : MonoBehaviour
{
    public float currentSpeed = 2f; // 기본 좌측 이동 속도
    public int Score = 100;
    public int Coins = 100;

    public bool isMovingToPlayer { get; private set; } = false; // Player로 이동 여부
    private Vector3 startPosition; // 아이템 초기 위치
    private Vector3 magnetStartPosition; // 아이템 초기 위치
    private Transform targetTransform; // 목표 위치 (플레이어 Transform)
    private float attractionSpeed = 5f; // Player를 향한 이동 속도

    private float currentTime = 0f;
    public float moveDuration = 0.5f; // 이동 시간 (Lerp 속도)

    private void Start()
    {
        startPosition = transform.position; // 초기 위치 저장
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
                isMovingToPlayer = false; // 이동 상태 종료
            }
        }
        else
        {
            // Magnet의 영향을 받지 않을 때 좌측으로 이동
            transform.Translate(Vector2.left * currentSpeed * Time.deltaTime, Space.World);

            // 화면 밖으로 나가면 초기 위치로 리셋
            if (transform.position.x < -15f)
            {
                ResetPosition();
            }
        }
    }

    public void ResetPosition()
    {
        Debug.Log($"Item {gameObject.name} 초기화");
        transform.position = startPosition;
        isMovingToPlayer = false;
    }
}
