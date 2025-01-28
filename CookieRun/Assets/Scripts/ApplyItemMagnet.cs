//using UnityEngine;

//public class ApplyItemMagnet : MonoBehaviour
//{
//    public int score = 0;
//    public int coins = 0;
//    public bool onItmeEffect = false;
//    public float plusHp = 0f;

//    public bool isMovingToPlayer { get; private set; } = false; // Player로 이동 여부
//    private Vector3 magnetStartPosition;
//    private Transform targetTransform; // 목표 위치 (플레이어 Transform)

//    private float currentTime = 0f;
//    public float moveDuration = 0.5f; // 이동 시간 (Lerp 속도)

//    private Vector3 initialLocalPosition; // 아이템의 초기 로컬 위치

//    private void Start()
//    {
//        Debug.Log($"[디버그] {gameObject.name} 아이템 초기화 - 점수: {score}, 코인: {coins}");
//    }

//    private void Awake()
//    {
//        // Awake에서 아이템의 초기 로컬 위치를 저장
//        initialLocalPosition = transform.localPosition;
//    }

//    private void OnEnable()
//    {
//        // 프리팹 재활성화 시 초기화
//        ResetItem();
//    }

//    public void ResetItem()
//    {
//        isMovingToPlayer = false; // 이동 상태 초기화
//        transform.localPosition = initialLocalPosition; // 저장된 초기 로컬 위치로 복원
//        gameObject.SetActive(true); // 아이템 활성화
//    }

//    // Magnet과 충돌 시 호출
//    public void MoveTowardsPlayer(Transform target, float speed)
//    {
//        isMovingToPlayer = true;
//        targetTransform = target; // 목표 위치 설정
//        magnetStartPosition = transform.position; // 이동 시작 위치 갱신
//        currentTime = 0f; // 이동 시간 초기화
//    }

//    private void Update()
//    {
//        if (isMovingToPlayer && targetTransform != null)
//        {
//            currentTime += Time.deltaTime;
//            transform.position = Vector3.Lerp(magnetStartPosition, targetTransform.position, currentTime / moveDuration);

//            if (currentTime >= moveDuration)
//            {
//                isMovingToPlayer = false;

//                // 이동 완료 후 효과 적용
//                PlayerCrash playerCrash = targetTransform.GetComponent<PlayerCrash>();
//                if (playerCrash != null)
//                {
//                    playerCrash.ApplyItemEffect(this);
//                }
//                else
//                {
//                    Debug.LogError("[오류] PlayerCrash를 찾을 수 없습니다!");
//                }

//                gameObject.SetActive(false);
//            }
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        //if (collision.CompareTag("Player")) // Player 태그 확인
//        //{
//        //    Debug.Log("Player와 충돌, 아이템 비활성화!");
//        //    gameObject.SetActive(false); // 아이템 비활성화
//        //}

//        if (collision.CompareTag("Player")) // 직접 충돌 시 효과 적용
//        {
//            PlayerCrash playerCrash = collision.GetComponent<PlayerCrash>();
//            if (playerCrash != null)
//            {
//                playerCrash.ApplyItemEffect(this); // 효과 적용
//            }
//            gameObject.SetActive(false); // 아이템 비활성화
//        }
//    }
//}




using UnityEngine;

public class ApplyItemMagnet : MonoBehaviour
{
    public int score = 0;
    public int coins = 0;
    public bool onItmeEffect = false;
    public float plusHp = 0f;

    public bool isMovingToPlayer { get; private set; } = false; // Player로 이동 여부
    private Vector3 magnetStartPosition;
    private Transform targetTransform; // 목표 위치 (플레이어 Transform)

    private float currentTime = 0f;
    public float moveDuration = 0.5f; // 이동 시간 (Lerp 속도)

    private Vector3 initialLocalPosition; // 아이템의 초기 로컬 위치

    private void Start()
    {
        // ✅ 아이템이 초기화될 때, 설정된 점수와 코인 값 확인
        Debug.Log($"[디버그] {gameObject.name} 아이템 초기화 - 점수: {score}, 코인: {coins}");
    }

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

        // ✅ ResetItem() 실행 시 점수 값이 변경되는지 확인
        Debug.Log($"[디버그] ResetItem() 호출 - 점수: {score}, 코인: {coins}");
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
            transform.position = Vector3.Lerp(magnetStartPosition, targetTransform.position, currentTime / moveDuration);

            if (currentTime >= moveDuration)
            {
                isMovingToPlayer = false;

                // ✅ 이동 완료 후 효과 적용 여부 확인
                Debug.Log($"[디버그] {gameObject.name} 아이템 이동 완료, ApplyItemEffect 호출");

                PlayerCrash playerCrash = targetTransform.GetComponent<PlayerCrash>();
                if (playerCrash != null)
                {
                    playerCrash.ApplyItemEffect(this);
                }
                else
                {
                    Debug.LogError("[오류] PlayerCrash를 찾을 수 없습니다!");
                }

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ✅ 아이템이 직접 플레이어와 충돌했을 때 로그 출력
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"[디버그] {gameObject.name} 충돌, ApplyItemEffect 호출");

            PlayerCrash playerCrash = collision.GetComponent<PlayerCrash>();
            if (playerCrash != null)
            {
                playerCrash.ApplyItemEffect(this);
            }
            gameObject.SetActive(false);
        }
    }
}
