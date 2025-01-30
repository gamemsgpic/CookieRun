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
            // 풀에 남아있는 오브젝트가 없으면 새로 생성
            prefabToActivate = Instantiate(selectedPrefab);
        }

        // 위치 설정
        if (activePrefabs.Count == 0)
        {
            // 첫 번째 프리팹: StartMapPos 기준으로 배치
            prefabToActivate.transform.position = startGroundRightPos.position;
        }
        else
        {
            // 나머지 프리팹: 가장 오른쪽에 배치
            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");

            Vector3 offset = rightPivot.position - prefabToActivate.transform.Find("LeftPivot").position;
            prefabToActivate.transform.position += offset;
        }

        // 내부 자식 오브젝트 초기화
        foreach (Transform child in prefabToActivate.transform)
        {
            var itemMagnet = child.GetComponent<ApplyItemMagnet>();
            if (itemMagnet != null)
            {
                itemMagnet.ResetItem(); // 초기화 호출
            }
        }

        // 활성화 및 리스트 추가
        prefabToActivate.SetActive(true);
        activePrefabs.Add(prefabToActivate);
    }
}
