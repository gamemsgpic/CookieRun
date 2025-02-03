using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public GameObject[] prefabPool; // 사용할 프리팹 목록
    public List<GameObject> activePrefabs = new List<GameObject>(); // 활성화된 프리팹
    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // 오브젝트 풀
    public Transform StartMapPos; // StartMapPos 참조
    public Transform startGroundRightPos; // StartMapPos 참조
    public int maxActivePrefabs = 5; // 활성화된 프리팹의 최대 개수

    void Start()
    {
        // 각 프리팹에 대해 오브젝트 풀 생성
        foreach (var prefab in prefabPool)
        {
            prefabPools[prefab.name] = new Queue<GameObject>();
        }

        // 초기 활성 프리팹 설정
        for (int i = 0; i < maxActivePrefabs; i++)
        {
            ActivatePrefab();
        }
    }

    public void DeactivatePrefab(GameObject prefab)
    {
        // 활성화 리스트에서 제거
        activePrefabs.Remove(prefab);

        // 비활성화하고 오브젝트 풀에 반환
        string prefabName = prefab.name.Replace("(Clone)", "").Trim();
        prefab.SetActive(false);

        if (prefabPools.ContainsKey(prefabName))
        {
            prefabPools[prefabName].Enqueue(prefab);
        }

        // 새로운 프리팹 활성화
        ActivatePrefab();
    }

    public void ActivatePrefab()
    {
        // 랜덤으로 프리팹 선택
        GameObject selectedPrefab = prefabPool[Random.Range(0, prefabPool.Length)];
        GameObject prefabToActivate;

        // 오브젝트 풀에서 가져오기
        string prefabName = selectedPrefab.name;
        if (prefabPools[prefabName].Count > 0)
        {
            prefabToActivate = prefabPools[prefabName].Dequeue();
        }
        else
        {
            // 풀에 남은 오브젝트가 없으면 새로 생성
            prefabToActivate = Instantiate(selectedPrefab);
        }

        // 오브젝트 풀에서 재사용하는 경우, 이전에 적용된 위치 정보를 초기화
        // (프리팹의 기본 위치가 (0,0,0)이라 가정하거나, 별도의 초기 위치를 저장해두었다면 그 값을 사용)
        prefabToActivate.transform.position = Vector3.zero;

        // 프리팹 내부의 LeftPivot을 찾음 (이 값은 로컬 좌표임)
        Transform leftPivot = prefabToActivate.transform.Find("LeftPivot");

        if (activePrefabs.Count == 0)
        {
            // 첫 번째 프리팹의 경우: startGroundRightPos의 위치에 맞춰 배치
            if (leftPivot != null)
            {
                // 새 프리팹의 LeftPivot이 startGroundRightPos와 일치하도록 위치 계산
                // leftPivot.localPosition는 프리팹 내부의 로컬 좌표이므로, 이를 빼주면 올바른 월드 좌표가 계산됩니다.
                Vector3 newPos = startGroundRightPos.position - leftPivot.localPosition;
                prefabToActivate.transform.position = newPos;
            }
            else
            {
                Debug.LogWarning("[MapManager] LeftPivot을 찾을 수 없습니다.");
                prefabToActivate.transform.position = startGroundRightPos.position;
            }
        }
        else
        {
            // 두 번째 이후의 프리팹: 마지막 활성 프리팹의 RightPivot의 위치를 기준으로 배치
            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");

            if (rightPivot != null && leftPivot != null)
            {
                // 마지막 프리팹의 RightPivot은 월드 좌표로 얻을 수 있으므로,
                // 새 프리팹의 LeftPivot이 해당 위치와 일치하도록 배치합니다.
                Vector3 newPos = rightPivot.position - leftPivot.localPosition;
                prefabToActivate.transform.position = newPos;
            }
            else
            {
                Debug.LogWarning("[MapManager] RightPivot 또는 LeftPivot을 찾을 수 없습니다.");
            }
        }

        // 내부 자식 오브젝트 초기화 (예를 들어, ApplyItemMagnet 초기화)
        foreach (Transform child in prefabToActivate.transform)
        {
            var itemMagnet = child.GetComponent<ApplyItemMagnet>();
            if (itemMagnet != null)
            {
                itemMagnet.ResetItem(); // 초기화 호출
            }
        }

        // 프리팹 활성화 후 activePrefabs 리스트에 추가
        prefabToActivate.SetActive(true);
        activePrefabs.Add(prefabToActivate);
    }

}
