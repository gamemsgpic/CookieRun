using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public GameObject[] prefabPool; // 사용할 프리팹 목록
    public List<GameObject> activePrefabs = new List<GameObject>(); // 활성화된 프리팹
    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // 오브젝트 풀
    public Transform StartMapPos; // 시작 위치 참조 (필요시)
    public Transform startGroundRightPos; // 첫 프리팹 배치 기준 위치
    public int maxActivePrefabs = 5; // 활성화된 프리팹의 최대 개수

    // 새로 활성화된 프리팹의 보정이 필요할 때 LateUpdate에서 처리하도록 임시로 저장
    private GameObject pendingAdjustmentPrefab = null;

    void Start()
    {
        // 각 프리팹 이름으로 오브젝트 풀 생성
        foreach (var prefab in prefabPool)
        {
            prefabPools[prefab.name] = new Queue<GameObject>();
        }

        // 초기 활성 프리팹 생성
        for (int i = 0; i < maxActivePrefabs; i++)
        {
            ActivatePrefab();
        }
    }

    public void DeactivatePrefab(GameObject prefab)
    {
        activePrefabs.Remove(prefab);

        // (Clone) 제거한 이름으로 오브젝트 풀에 반환
        string prefabName = prefab.name.Replace("(Clone)", "").Trim();
        prefab.SetActive(false);

        if (prefabPools.ContainsKey(prefabName))
        {
            prefabPools[prefabName].Enqueue(prefab);
        }

        // 새 프리팹 활성화
        ActivatePrefab();
    }

    public void ActivatePrefab()
    {
        // 랜덤 프리팹 선택
        GameObject selectedPrefab = prefabPool[Random.Range(0, prefabPool.Length)];
        GameObject prefabToActivate;
        string prefabName = selectedPrefab.name;

        // 오브젝트 풀에서 가져오거나 없으면 새로 생성
        if (prefabPools[prefabName].Count > 0)
        {
            prefabToActivate = prefabPools[prefabName].Dequeue();
        }
        else
        {
            prefabToActivate = Instantiate(selectedPrefab);
        }

        // 재사용 시 기본 상태로 리셋
        prefabToActivate.transform.position = Vector3.zero;
        prefabToActivate.transform.rotation = Quaternion.identity;
        prefabToActivate.transform.localScale = Vector3.one;

        // 내부의 LeftPivot 검색 (없으면 transform 자체 사용)
        Transform leftPivot = prefabToActivate.transform.Find("LeftPivot");
        if (leftPivot == null)
        {
            Debug.LogWarning("[MapManager] LeftPivot을 찾을 수 없습니다. 기본 transform을 사용합니다.");
            leftPivot = prefabToActivate.transform;
        }
        // 프리팹 내 기준점과의 오프셋 계산 (월드 좌표로 변환)
        Vector3 leftPivotWorldOffset = prefabToActivate.transform.TransformPoint(leftPivot.localPosition) - prefabToActivate.transform.position;

        // 첫 프리팹인 경우
        if (activePrefabs.Count == 0)
        {
            Vector3 newPos = startGroundRightPos.position - leftPivotWorldOffset;
            prefabToActivate.transform.position = newPos;
        }
        else
        {
            // 이전 프리팹의 RightPivot 기준으로 배치
            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");
            if (rightPivot != null)
            {
                Vector3 newPos = rightPivot.position - leftPivotWorldOffset;
                prefabToActivate.transform.position = newPos;
            }
            else
            {
                Debug.LogWarning("[MapManager] RightPivot을 찾을 수 없습니다.");
            }
        }

        // (필요하다면) 프리팹 내부 자식의 상태 초기화
        foreach (Transform child in prefabToActivate.transform)
        {
            var itemMagnet = child.GetComponent<ApplyItemMagnet>();
            if (itemMagnet != null)
            {
                itemMagnet.ResetItem();
            }
        }

        prefabToActivate.SetActive(true);
        activePrefabs.Add(prefabToActivate);

        // 새 프리팹 보정이 필요하므로 변수에 저장 (LateUpdate에서 처리)
        pendingAdjustmentPrefab = prefabToActivate;
    }

    // 업데이트 이후에 한번 더 위치보정
    void LateUpdate()
    {
        if (pendingAdjustmentPrefab != null)
        {
            int count = activePrefabs.Count;
            if (count > 1)
            {
                // 마지막에서 두 번째 프리팹 기준
                GameObject previousPrefab = activePrefabs[count - 2];
                Transform rightPivot = previousPrefab.transform.Find("RightPivot");
                Transform leftPivot = pendingAdjustmentPrefab.transform.Find("LeftPivot");

                if (rightPivot != null && leftPivot != null)
                {
                    Vector3 leftPivotWorldOffset = pendingAdjustmentPrefab.transform.TransformPoint(leftPivot.localPosition) - pendingAdjustmentPrefab.transform.position;
                    Vector3 newPos = rightPivot.position - leftPivotWorldOffset;
                    pendingAdjustmentPrefab.transform.position = newPos;
                }
                else
                {
                    Debug.LogWarning("[MapManager] LateUpdate 보정 시 RightPivot 또는 LeftPivot을 찾을 수 없습니다.");
                }
            }
            pendingAdjustmentPrefab = null;
        }
    }
}
