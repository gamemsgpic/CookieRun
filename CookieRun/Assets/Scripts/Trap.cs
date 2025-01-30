using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    private Vector3 initialLocalPosition; // 원래 위치 저장
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float lerpTime = 0f;
    private float lerpDuration = 1f; // 이동하는 시간 (3초)

    private bool isFlying = false;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition; // 처음 생성될 때 로컬 포지션 저장
    }

    public void LaunchTrap()
    {
        if (isFlying) return; // 이미 날아가고 있다면 중복 실행 방지

        isFlying = true;
        lerpTime = 0f;
        startPosition = transform.position;

        // 15도 ~ 75도 사이의 랜덤한 각도로 날아가도록 방향 설정
        float randomAngle = Random.Range(15f, 75f);
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;

        targetPosition = startPosition + (Vector3)direction * 30f; // 5 유닛 거리로 날아가기

        StartCoroutine(DisableAfterTime()); // 3초 후 자동 비활성화
    }

    private void Update()
    {
        if (isFlying)
        {
            lerpTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, lerpTime / lerpDuration);
        }
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(3f); // 3초 대기 후

        isFlying = false;
        transform.localPosition = initialLocalPosition; // 원래 위치로 복귀
        gameObject.SetActive(false); // 비활성화
    }
}
