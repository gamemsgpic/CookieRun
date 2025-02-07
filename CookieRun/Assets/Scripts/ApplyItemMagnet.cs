using UnityEngine;

public class ApplyItemMagnet : MonoBehaviour
{
    public int score = 0;
    public int coins = 0;
    public bool onItmeEffect = false;
    public float plusHp = 0f;
    public Vector3 giantization;

    public bool isMovingToPlayer { get; private set; } = false; // Player로 이동 여부
    private Vector3 magnetStartPosition;
    private Transform targetTransform; // 목표 위치 (플레이어 Transform)

    private float currentTime = 0f;
    public float moveDuration = 0.1f; // 이동 시간 (Lerp 속도)

    private Vector3 initialLocalPosition; // 아이템의 초기 로컬 위치
    private TableObjectSC data; // CSV 데이터 관리 스크립트

    private void Awake()
    {
        // Awake에서 아이템의 초기 로컬 위치를 저장
        initialLocalPosition = transform.localPosition;

        // TableObjectSC가 존재하는지 확인
        data = GetComponentInChildren<TableObjectSC>();

        // TableObjectSC가 없는 경우에도 오류 없이 진행되도록 처리
        if (data == null)
        {
            //Debug.Log($"[ApplyItemMagnet] No TableObjectSC attached to {gameObject.name}, skipping table initialization.", gameObject);
        }
    }

    private void OnEnable()
    {
        ResetItem();
    }

    public void ResetItem()
    {
        // 기본적으로 모든 프리팹을 활성화
        isMovingToPlayer = false; // 이동 상태 초기화
        transform.localPosition = initialLocalPosition; // 저장된 초기 로컬 위치로 복원
        gameObject.SetActive(true); // 프리팹 활성화 (TableObjectSC가 없어도 실행됨)

        // TableObjectSC가 있는 경우에만 데이터를 적용
        if (data != null)
        {
            score = data.Score;
            coins = (int)data.Coin;
            gameObject.GetComponentInChildren<TableObjectSC>().gameObject.SetActive(true);
        }
    }


    // Magnet과 충돌 시 호출
    public void MoveTowardsPlayer(Transform target, float speed)
    {
        isMovingToPlayer = true;
        targetTransform = target; // 목표 위치 설정
        magnetStartPosition = transform.position; // 이동 시작 위치 갱신
        currentTime = 0f; // 이동 시간 초기화
    }

    private void Update()
    {
        if (isMovingToPlayer && targetTransform != null)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(magnetStartPosition, (targetTransform.position + (Vector3.up * 1f)), currentTime / moveDuration);

            if (currentTime >= moveDuration)
            {
                isMovingToPlayer = false;

                PlayerCrash playerCrash = targetTransform.GetComponent<PlayerCrash>();
                if (playerCrash != null)
                {
                    playerCrash.ApplyItemEffect(this);
                }

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 아이템이 직접 플레이어와 충돌했을 때 효과 적용
        if (collision.CompareTag("Player"))
        {
            PlayerCrash playerCrash = collision.GetComponent<PlayerCrash>();
            if (playerCrash != null)
            {
                playerCrash.ApplyItemEffect(this);
            }
            gameObject.SetActive(false);
        }
    }
}
