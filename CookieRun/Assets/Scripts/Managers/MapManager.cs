//using UnityEngine;
//using System.Collections.Generic;

//public class MapManager : MonoBehaviour
//{
//    public GameObject[] prefabPool; // 사용할 프리팹 목록
//    public List<GameObject> activePrefabs = new List<GameObject>(); // 활성화된 프리팹
//    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // 오브젝트 풀
//    public Transform StartMapPos; // 시작 위치 참조 (필요시)
//    public Transform startGroundRightPos; // 첫 프리팹 배치 기준 위치
//    public int maxActivePrefabs = 5; // 활성화된 프리팹의 최대 개수

//    // 새로 활성화된 프리팹의 보정이 필요할 때 LateUpdate에서 처리하도록 임시로 저장
//    private GameObject pendingAdjustmentPrefab = null;

//    void Start()
//    {
//        // 각 프리팹 이름으로 오브젝트 풀 생성
//        foreach (var prefab in prefabPool)
//        {
//            prefabPools[prefab.name] = new Queue<GameObject>();
//        }

//        // 초기 활성 프리팹 생성
//        for (int i = 0; i < maxActivePrefabs; i++)
//        {
//            ActivatePrefab();
//        }
//    }

//    public void DeactivatePrefab(GameObject prefab)
//    {
//        activePrefabs.Remove(prefab);

//        // (Clone) 제거한 이름으로 오브젝트 풀에 반환
//        string prefabName = prefab.name.Replace("(Clone)", "").Trim();
//        prefab.SetActive(false);

//        if (prefabPools.ContainsKey(prefabName))
//        {
//            prefabPools[prefabName].Enqueue(prefab);
//        }

//        // 새 프리팹 활성화
//        ActivatePrefab();
//    }

//    public void ActivatePrefab()
//    {
//        // 랜덤 프리팹 선택
//        GameObject selectedPrefab = prefabPool[Random.Range(0, prefabPool.Length)];
//        GameObject prefabToActivate;
//        string prefabName = selectedPrefab.name;

//        // 오브젝트 풀에서 가져오거나 없으면 새로 생성
//        if (prefabPools[prefabName].Count > 0)
//        {
//            prefabToActivate = prefabPools[prefabName].Dequeue();
//        }
//        else
//        {
//            prefabToActivate = Instantiate(selectedPrefab);
//        }

//        // 재사용 시 기본 상태로 리셋
//        prefabToActivate.transform.position = Vector3.zero;
//        prefabToActivate.transform.rotation = Quaternion.identity;
//        prefabToActivate.transform.localScale = Vector3.one;

//        // 내부의 LeftPivot 검색 (없으면 transform 자체 사용)
//        Transform leftPivot = prefabToActivate.transform.Find("LeftPivot");
//        if (leftPivot == null)
//        {
//            Debug.LogWarning("[MapManager] LeftPivot을 찾을 수 없습니다. 기본 transform을 사용합니다.");
//            leftPivot = prefabToActivate.transform;
//        }
//        // 프리팹 내 기준점과의 오프셋 계산 (월드 좌표로 변환)
//        Vector3 leftPivotWorldOffset = prefabToActivate.transform.TransformPoint(leftPivot.localPosition) - prefabToActivate.transform.position;

//        // 첫 프리팹인 경우
//        if (activePrefabs.Count == 0)
//        {
//            Vector3 newPos = startGroundRightPos.position - leftPivotWorldOffset;
//            prefabToActivate.transform.position = newPos;
//        }
//        else
//        {
//            // 이전 프리팹의 RightPivot 기준으로 배치
//            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
//            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");
//            if (rightPivot != null)
//            {
//                Vector3 newPos = rightPivot.position - leftPivotWorldOffset;
//                prefabToActivate.transform.position = newPos;
//            }
//            else
//            {
//                Debug.LogWarning("[MapManager] RightPivot을 찾을 수 없습니다.");
//            }
//        }

//        // (필요하다면) 프리팹 내부 자식의 상태 초기화
//        foreach (Transform child in prefabToActivate.transform)
//        {
//            var itemMagnet = child.GetComponent<ApplyItemMagnet>();
//            if (itemMagnet != null)
//            {
//                itemMagnet.ResetItem();
//            }
//        }

//        prefabToActivate.SetActive(true);
//        activePrefabs.Add(prefabToActivate);

//        // 새 프리팹 보정이 필요하므로 변수에 저장 (LateUpdate에서 처리)
//        pendingAdjustmentPrefab = prefabToActivate;
//    }

//    // 업데이트 이후에 한번 더 위치보정
//    void LateUpdate()
//    {
//        if (pendingAdjustmentPrefab != null)
//        {
//            int count = activePrefabs.Count;
//            if (count > 1)
//            {
//                // 마지막에서 두 번째 프리팹 기준
//                GameObject previousPrefab = activePrefabs[count - 2];
//                Transform rightPivot = previousPrefab.transform.Find("RightPivot");
//                Transform leftPivot = pendingAdjustmentPrefab.transform.Find("LeftPivot");

//                if (rightPivot != null && leftPivot != null)
//                {
//                    Vector3 leftPivotWorldOffset = pendingAdjustmentPrefab.transform.TransformPoint(leftPivot.localPosition) - pendingAdjustmentPrefab.transform.position;
//                    Vector3 newPos = rightPivot.position - leftPivotWorldOffset;
//                    pendingAdjustmentPrefab.transform.position = newPos;
//                }
//                else
//                {
//                    Debug.LogWarning("[MapManager] LateUpdate 보정 시 RightPivot 또는 LeftPivot을 찾을 수 없습니다.");
//                }
//            }
//            pendingAdjustmentPrefab = null;
//        }
//    }
//}


using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public class WavePrefabList
    {
        public List<GameObject> prefabs;
    }

    public Sprite[] tileSprite;

    public List<WavePrefabList> wavePrefabs = new List<WavePrefabList>(); // 웨이브별 프리팹 리스트
    public List<GameObject> clearMapPrefabs = new List<GameObject>(); // 클리어맵 프리팹 리스트
    public List<GameObject> startMapPrefabs = new List<GameObject>(); // 스타트맵 프리팹 리스트
    public List<GameObject> activePrefabs = new List<GameObject>(); // 활성화된 프리팹 리스트
    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // 오브젝트 풀

    public Transform StartMapPos; // 시작 위치
    public Transform startGroundRightPos; // 첫 프리팹 배치 위치
    public Transform clearPivot; // 클리어 피봇 (프리팹 제거 기준)
    public int maxActivePrefabs = 5; // 최대 활성 프리팹 개수
    public int currentWave { get; private set; } = 0; // 현재 웨이브

    void Start()
    {
        InitializePrefabPools();
        StartWave(0); // 첫 웨이브 시작
    }

    private void InitializePrefabPools()
    {
        foreach (var wave in wavePrefabs)
        {
            if (wave.prefabs == null)
            {
                continue;
            }

            foreach (var prefab in wave.prefabs)
            {
                if (!prefabPools.ContainsKey(prefab.name))
                    prefabPools[prefab.name] = new Queue<GameObject>();
            }
        }

        foreach (var prefab in clearMapPrefabs)
        {
            if (!prefabPools.ContainsKey(prefab.name))
                prefabPools[prefab.name] = new Queue<GameObject>();
        }

        foreach (var prefab in startMapPrefabs)
        {
            if (!prefabPools.ContainsKey(prefab.name))
                prefabPools[prefab.name] = new Queue<GameObject>();
        }
    }

    public void StartWave(int wave)
    {
        if (wave < 0 || wave >= wavePrefabs.Count)
        {
            return;
        }

        currentWave = wave;
        ClearOldPrefabs();
        AddTransitionPrefabs();
        SpawnInitialWavePrefabs();
    }

    private void ClearOldPrefabs()
    {
        for (int i = activePrefabs.Count - 1; i >= 0; i--)
        {
            GameObject prefab = activePrefabs[i];
            Transform leftPivot = prefab.transform.Find("LeftPivot");

            if (leftPivot != null && leftPivot.position.x > clearPivot.position.x)
            {
                DeactivatePrefab(prefab);
            }
        }
    }

    private void AddTransitionPrefabs()
    {
        if (activePrefabs.Count == 0)
        {
            return;
        }

        GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
        Transform lastRightPivot = lastActivePrefab.transform.Find("RightPivot");

        if (lastRightPivot == null)
        {
            return;
        }


        GameObject clearPrefab = Instantiate(clearMapPrefabs[Random.Range(0, clearMapPrefabs.Count)]);
        Transform clearLeftPivot = clearPrefab.transform.Find("LeftPivot");
        clearPrefab.transform.position = lastRightPivot.position - (clearLeftPivot ? clearLeftPivot.localPosition : Vector3.zero);
        clearPrefab.SetActive(true);
        activePrefabs.Add(clearPrefab);

        GameObject startPrefab = Instantiate(startMapPrefabs[Random.Range(0, startMapPrefabs.Count)]);
        Transform startLeftPivot = startPrefab.transform.Find("LeftPivot");
        Transform clearRightPivot = clearPrefab.transform.Find("RightPivot");

        if (clearRightPivot)
        {
            startPrefab.transform.position = clearRightPivot.position - (startLeftPivot ? startLeftPivot.localPosition : Vector3.zero);
            startPrefab.SetActive(true);
            activePrefabs.Add(startPrefab);
        }
    }


    private void SpawnInitialWavePrefabs()
    {
        for (int i = 0; i < 3; i++)
        {
            ActivatePrefab();
        }
    }

    public void DeactivatePrefab(GameObject prefab)
    {
        activePrefabs.Remove(prefab);
        string prefabName = prefab.name.Replace("(Clone)", "").Trim();

        // 부모와 자식 오브젝트 비활성화
        prefab.SetActive(false);
        foreach (Transform child in prefab.transform)
        {
            child.gameObject.SetActive(false);
        }

        // 오브젝트 풀에 반환
        if (prefabPools.ContainsKey(prefabName))
        {
            prefabPools[prefabName].Enqueue(prefab);
        }
    }



    public void ActivatePrefab()
    {
        List<GameObject> availablePrefabs = GetCurrentWavePrefabs();
        if (availablePrefabs.Count == 0)
        {
            return;
        }

        GameObject selectedPrefab = availablePrefabs[Random.Range(0, availablePrefabs.Count)];
        GameObject prefabToActivate;

        if (prefabPools.ContainsKey(selectedPrefab.name) && prefabPools[selectedPrefab.name].Count > 0)
        {
            prefabToActivate = prefabPools[selectedPrefab.name].Dequeue();
        }
        else
        {
            prefabToActivate = Instantiate(selectedPrefab);
        }

        // 부모 활성화
        prefabToActivate.SetActive(true);

        prefabToActivate.transform.position = Vector3.zero;
        prefabToActivate.transform.rotation = Quaternion.identity;
        prefabToActivate.transform.localScale = Vector3.one;

        Transform leftPivot = prefabToActivate.transform.Find("LeftPivot") ?? prefabToActivate.transform;
        Vector3 leftPivotWorldOffset = prefabToActivate.transform.TransformPoint(leftPivot.localPosition) - prefabToActivate.transform.position;

        if (activePrefabs.Count == 0)
        {
            prefabToActivate.transform.position = startGroundRightPos.position - leftPivotWorldOffset;
        }
        else
        {
            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");
            if (rightPivot != null)
            {
                prefabToActivate.transform.position = rightPivot.position - leftPivotWorldOffset;
            }
        }

        // 모든 자식 오브젝트 활성화
        foreach (Transform child in prefabToActivate.transform)
        {
            child.gameObject.SetActive(true);

            var tableObj = child.GetComponent<SampleTableObjectSC>();
            if (tableObj != null)
            {
                tableObj.Init();
            }

            var itemMagnet = child.GetComponent<ApplyItemMagnet>();
            if (itemMagnet != null)
            {
                itemMagnet.ResetItem();
            }
        }

        activePrefabs.Add(prefabToActivate);
       // if (currentWave == 0)
       // {
       //     ChangeAllTilemapSprites(prefabToActivate, tileSprite[0]);
       // }
       // else if (currentWave == 1)
       // {
       //     ChangeAllTilemapSprites(prefabToActivate, tileSprite[1]);
       // }
       // else if (currentWave == 2)
       // {
       //     ChangeAllTilemapSprites(prefabToActivate, tileSprite[2]);
       // }
       // else if (currentWave == 3)
       // {
       //     ChangeAllTilemapSprites(prefabToActivate, tileSprite[3]);
       // }
    }



    private List<GameObject> GetCurrentWavePrefabs()
    {
        if (currentWave == wavePrefabs.Count - 1)
        {
            List<GameObject> allPrefabs = new List<GameObject>();
            foreach (var waveList in wavePrefabs)
            {
                allPrefabs.AddRange(waveList.prefabs);
            }
            return allPrefabs;
        }
        else
        {
            return wavePrefabs[currentWave].prefabs;
        }
    }

    private void ResetPrefab(GameObject prefab)
    {
        prefab.transform.position = Vector3.zero;
        prefab.transform.rotation = Quaternion.identity;
        prefab.transform.localScale = Vector3.one;

        prefab.SetActive(true);

        foreach (Transform child in prefab.transform)
        {
            child.gameObject.SetActive(true);

            if (child.TryGetComponent<ApplyItemMagnet>(out var itemMagnet))
            {
                itemMagnet.ResetItem();
            }
            else if (child.TryGetComponent<Trap>(out var trap))
            {
                trap.gameObject.SetActive(true); // 트랩 활성화
                trap.transform.localPosition = trap.initialLocalPosition; // 원래 위치로 복귀
            }
            else
            {
                child.localPosition = Vector3.zero;
                child.localRotation = Quaternion.identity;
            }
        }
    }


    void LateUpdate()
    {
        if (activePrefabs.Count < 2) return;

        GameObject lastPrefab = activePrefabs[activePrefabs.Count - 2]; // 마지막에서 두 번째 프리팹
        GameObject newPrefab = activePrefabs[activePrefabs.Count - 1]; // 가장 최근 추가된 프리팹

        Transform lastRightPivot = lastPrefab.transform.Find("RightPivot");
        Transform newLeftPivot = newPrefab.transform.Find("LeftPivot");

        if (lastRightPivot != null && newLeftPivot != null)
        {
            // 위치 보정: 마지막 프리팹의 RightPivot과 새로운 프리팹의 LeftPivot을 정확히 맞춤
            Vector3 offset = newLeftPivot.position - newPrefab.transform.position;
            newPrefab.transform.position = lastRightPivot.position - offset;
        }

        // 활성화된 맵 개수가 부족하면 추가 배치
        while (activePrefabs.Count < maxActivePrefabs)
        {
            ActivatePrefab();
        }
    }

    //private void ChangeAllTilemapSprites(GameObject prefab, Sprite newSprite)
    //{
    //    Tilemap[] tilemaps = prefab.GetComponentsInChildren<Tilemap>();
    //
    //    foreach (Tilemap tilemap in tilemaps)
    //    {
    //        BoundsInt bounds = tilemap.cellBounds;
    //        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
    //
    //        Tile[] newTiles = new Tile[allTiles.Length];
    //
    //        for (int i = 0; i < allTiles.Length; i++)
    //        {
    //            if (allTiles[i] is Tile oldTile)
    //            {
    //                Tile newTile = ScriptableObject.CreateInstance<Tile>(); // 새로운 타일 객체 생성
    //                newTile.sprite = newSprite; // 스프라이트 변경
    //                newTiles[i] = newTile;
    //            }
    //            else
    //            {
    //                newTiles[i] = null;
    //            }
    //        }
    //
    //        tilemap.SetTilesBlock(bounds, newTiles); // 한 번에 변경
    //    }
    //}


}
