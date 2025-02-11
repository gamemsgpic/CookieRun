//using UnityEngine;
//using System.Collections.Generic;

//public class MapManager : MonoBehaviour
//{
//    public GameObject[] prefabPool; // ����� ������ ���
//    public List<GameObject> activePrefabs = new List<GameObject>(); // Ȱ��ȭ�� ������
//    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // ������Ʈ Ǯ
//    public Transform StartMapPos; // ���� ��ġ ���� (�ʿ��)
//    public Transform startGroundRightPos; // ù ������ ��ġ ���� ��ġ
//    public int maxActivePrefabs = 5; // Ȱ��ȭ�� �������� �ִ� ����

//    // ���� Ȱ��ȭ�� �������� ������ �ʿ��� �� LateUpdate���� ó���ϵ��� �ӽ÷� ����
//    private GameObject pendingAdjustmentPrefab = null;

//    void Start()
//    {
//        // �� ������ �̸����� ������Ʈ Ǯ ����
//        foreach (var prefab in prefabPool)
//        {
//            prefabPools[prefab.name] = new Queue<GameObject>();
//        }

//        // �ʱ� Ȱ�� ������ ����
//        for (int i = 0; i < maxActivePrefabs; i++)
//        {
//            ActivatePrefab();
//        }
//    }

//    public void DeactivatePrefab(GameObject prefab)
//    {
//        activePrefabs.Remove(prefab);

//        // (Clone) ������ �̸����� ������Ʈ Ǯ�� ��ȯ
//        string prefabName = prefab.name.Replace("(Clone)", "").Trim();
//        prefab.SetActive(false);

//        if (prefabPools.ContainsKey(prefabName))
//        {
//            prefabPools[prefabName].Enqueue(prefab);
//        }

//        // �� ������ Ȱ��ȭ
//        ActivatePrefab();
//    }

//    public void ActivatePrefab()
//    {
//        // ���� ������ ����
//        GameObject selectedPrefab = prefabPool[Random.Range(0, prefabPool.Length)];
//        GameObject prefabToActivate;
//        string prefabName = selectedPrefab.name;

//        // ������Ʈ Ǯ���� �������ų� ������ ���� ����
//        if (prefabPools[prefabName].Count > 0)
//        {
//            prefabToActivate = prefabPools[prefabName].Dequeue();
//        }
//        else
//        {
//            prefabToActivate = Instantiate(selectedPrefab);
//        }

//        // ���� �� �⺻ ���·� ����
//        prefabToActivate.transform.position = Vector3.zero;
//        prefabToActivate.transform.rotation = Quaternion.identity;
//        prefabToActivate.transform.localScale = Vector3.one;

//        // ������ LeftPivot �˻� (������ transform ��ü ���)
//        Transform leftPivot = prefabToActivate.transform.Find("LeftPivot");
//        if (leftPivot == null)
//        {
//            Debug.LogWarning("[MapManager] LeftPivot�� ã�� �� �����ϴ�. �⺻ transform�� ����մϴ�.");
//            leftPivot = prefabToActivate.transform;
//        }
//        // ������ �� ���������� ������ ��� (���� ��ǥ�� ��ȯ)
//        Vector3 leftPivotWorldOffset = prefabToActivate.transform.TransformPoint(leftPivot.localPosition) - prefabToActivate.transform.position;

//        // ù �������� ���
//        if (activePrefabs.Count == 0)
//        {
//            Vector3 newPos = startGroundRightPos.position - leftPivotWorldOffset;
//            prefabToActivate.transform.position = newPos;
//        }
//        else
//        {
//            // ���� �������� RightPivot �������� ��ġ
//            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
//            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");
//            if (rightPivot != null)
//            {
//                Vector3 newPos = rightPivot.position - leftPivotWorldOffset;
//                prefabToActivate.transform.position = newPos;
//            }
//            else
//            {
//                Debug.LogWarning("[MapManager] RightPivot�� ã�� �� �����ϴ�.");
//            }
//        }

//        // (�ʿ��ϴٸ�) ������ ���� �ڽ��� ���� �ʱ�ȭ
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

//        // �� ������ ������ �ʿ��ϹǷ� ������ ���� (LateUpdate���� ó��)
//        pendingAdjustmentPrefab = prefabToActivate;
//    }

//    // ������Ʈ ���Ŀ� �ѹ� �� ��ġ����
//    void LateUpdate()
//    {
//        if (pendingAdjustmentPrefab != null)
//        {
//            int count = activePrefabs.Count;
//            if (count > 1)
//            {
//                // ���������� �� ��° ������ ����
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
//                    Debug.LogWarning("[MapManager] LateUpdate ���� �� RightPivot �Ǵ� LeftPivot�� ã�� �� �����ϴ�.");
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

    public List<WavePrefabList> wavePrefabs = new List<WavePrefabList>(); // ���̺꺰 ������ ����Ʈ
    public List<GameObject> clearMapPrefabs = new List<GameObject>(); // Ŭ����� ������ ����Ʈ
    public List<GameObject> startMapPrefabs = new List<GameObject>(); // ��ŸƮ�� ������ ����Ʈ
    public List<GameObject> activePrefabs = new List<GameObject>(); // Ȱ��ȭ�� ������ ����Ʈ
    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // ������Ʈ Ǯ

    public Transform StartMapPos; // ���� ��ġ
    public Transform startGroundRightPos; // ù ������ ��ġ ��ġ
    public Transform clearPivot; // Ŭ���� �Ǻ� (������ ���� ����)
    public int maxActivePrefabs = 5; // �ִ� Ȱ�� ������ ����
    public int currentWave { get; private set; } = 0; // ���� ���̺�

    void Start()
    {
        InitializePrefabPools();
        StartWave(0); // ù ���̺� ����
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

        // �θ�� �ڽ� ������Ʈ ��Ȱ��ȭ
        prefab.SetActive(false);
        foreach (Transform child in prefab.transform)
        {
            child.gameObject.SetActive(false);
        }

        // ������Ʈ Ǯ�� ��ȯ
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

        // �θ� Ȱ��ȭ
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

        // ��� �ڽ� ������Ʈ Ȱ��ȭ
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
                trap.gameObject.SetActive(true); // Ʈ�� Ȱ��ȭ
                trap.transform.localPosition = trap.initialLocalPosition; // ���� ��ġ�� ����
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

        GameObject lastPrefab = activePrefabs[activePrefabs.Count - 2]; // ���������� �� ��° ������
        GameObject newPrefab = activePrefabs[activePrefabs.Count - 1]; // ���� �ֱ� �߰��� ������

        Transform lastRightPivot = lastPrefab.transform.Find("RightPivot");
        Transform newLeftPivot = newPrefab.transform.Find("LeftPivot");

        if (lastRightPivot != null && newLeftPivot != null)
        {
            // ��ġ ����: ������ �������� RightPivot�� ���ο� �������� LeftPivot�� ��Ȯ�� ����
            Vector3 offset = newLeftPivot.position - newPrefab.transform.position;
            newPrefab.transform.position = lastRightPivot.position - offset;
        }

        // Ȱ��ȭ�� �� ������ �����ϸ� �߰� ��ġ
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
    //                Tile newTile = ScriptableObject.CreateInstance<Tile>(); // ���ο� Ÿ�� ��ü ����
    //                newTile.sprite = newSprite; // ��������Ʈ ����
    //                newTiles[i] = newTile;
    //            }
    //            else
    //            {
    //                newTiles[i] = null;
    //            }
    //        }
    //
    //        tilemap.SetTilesBlock(bounds, newTiles); // �� ���� ����
    //    }
    //}


}
