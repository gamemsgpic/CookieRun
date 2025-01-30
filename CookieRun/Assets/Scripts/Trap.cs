using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    private Vector3 initialLocalPosition; // ���� ��ġ ����
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float lerpTime = 0f;
    private float lerpDuration = 1f; // �̵��ϴ� �ð� (3��)

    private bool isFlying = false;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition; // ó�� ������ �� ���� ������ ����
    }

    public void LaunchTrap()
    {
        if (isFlying) return; // �̹� ���ư��� �ִٸ� �ߺ� ���� ����

        isFlying = true;
        lerpTime = 0f;
        startPosition = transform.position;

        // 15�� ~ 75�� ������ ������ ������ ���ư����� ���� ����
        float randomAngle = Random.Range(15f, 75f);
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;

        targetPosition = startPosition + (Vector3)direction * 30f; // 5 ���� �Ÿ��� ���ư���

        StartCoroutine(DisableAfterTime()); // 3�� �� �ڵ� ��Ȱ��ȭ
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
        yield return new WaitForSeconds(3f); // 3�� ��� ��

        isFlying = false;
        transform.localPosition = initialLocalPosition; // ���� ��ġ�� ����
        gameObject.SetActive(false); // ��Ȱ��ȭ
    }
}
