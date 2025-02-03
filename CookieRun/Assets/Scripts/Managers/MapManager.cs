using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public GameObject[] prefabPool; // ����� ������ ���
    public List<GameObject> activePrefabs = new List<GameObject>(); // Ȱ��ȭ�� ������
    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // ������Ʈ Ǯ
    public Transform StartMapPos; // ���� ��ġ ���� (�ʿ��)
    public Transform startGroundRightPos; // ù ������ ��ġ ���� ��ġ
    public int maxActivePrefabs = 5; // Ȱ��ȭ�� �������� �ִ� ����

    // ���� Ȱ��ȭ�� �������� ������ �ʿ��� �� LateUpdate���� ó���ϵ��� �ӽ÷� ����
    private GameObject pendingAdjustmentPrefab = null;

    void Start()
    {
        // �� ������ �̸����� ������Ʈ Ǯ ����
        foreach (var prefab in prefabPool)
        {
            prefabPools[prefab.name] = new Queue<GameObject>();
        }

        // �ʱ� Ȱ�� ������ ����
        for (int i = 0; i < maxActivePrefabs; i++)
        {
            ActivatePrefab();
        }
    }

    public void DeactivatePrefab(GameObject prefab)
    {
        activePrefabs.Remove(prefab);

        // (Clone) ������ �̸����� ������Ʈ Ǯ�� ��ȯ
        string prefabName = prefab.name.Replace("(Clone)", "").Trim();
        prefab.SetActive(false);

        if (prefabPools.ContainsKey(prefabName))
        {
            prefabPools[prefabName].Enqueue(prefab);
        }

        // �� ������ Ȱ��ȭ
        ActivatePrefab();
    }

    public void ActivatePrefab()
    {
        // ���� ������ ����
        GameObject selectedPrefab = prefabPool[Random.Range(0, prefabPool.Length)];
        GameObject prefabToActivate;
        string prefabName = selectedPrefab.name;

        // ������Ʈ Ǯ���� �������ų� ������ ���� ����
        if (prefabPools[prefabName].Count > 0)
        {
            prefabToActivate = prefabPools[prefabName].Dequeue();
        }
        else
        {
            prefabToActivate = Instantiate(selectedPrefab);
        }

        // ���� �� �⺻ ���·� ����
        prefabToActivate.transform.position = Vector3.zero;
        prefabToActivate.transform.rotation = Quaternion.identity;
        prefabToActivate.transform.localScale = Vector3.one;

        // ������ LeftPivot �˻� (������ transform ��ü ���)
        Transform leftPivot = prefabToActivate.transform.Find("LeftPivot");
        if (leftPivot == null)
        {
            Debug.LogWarning("[MapManager] LeftPivot�� ã�� �� �����ϴ�. �⺻ transform�� ����մϴ�.");
            leftPivot = prefabToActivate.transform;
        }
        // ������ �� ���������� ������ ��� (���� ��ǥ�� ��ȯ)
        Vector3 leftPivotWorldOffset = prefabToActivate.transform.TransformPoint(leftPivot.localPosition) - prefabToActivate.transform.position;

        // ù �������� ���
        if (activePrefabs.Count == 0)
        {
            Vector3 newPos = startGroundRightPos.position - leftPivotWorldOffset;
            prefabToActivate.transform.position = newPos;
        }
        else
        {
            // ���� �������� RightPivot �������� ��ġ
            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");
            if (rightPivot != null)
            {
                Vector3 newPos = rightPivot.position - leftPivotWorldOffset;
                prefabToActivate.transform.position = newPos;
            }
            else
            {
                Debug.LogWarning("[MapManager] RightPivot�� ã�� �� �����ϴ�.");
            }
        }

        // (�ʿ��ϴٸ�) ������ ���� �ڽ��� ���� �ʱ�ȭ
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

        // �� ������ ������ �ʿ��ϹǷ� ������ ���� (LateUpdate���� ó��)
        pendingAdjustmentPrefab = prefabToActivate;
    }

    // ������Ʈ ���Ŀ� �ѹ� �� ��ġ����
    void LateUpdate()
    {
        if (pendingAdjustmentPrefab != null)
        {
            int count = activePrefabs.Count;
            if (count > 1)
            {
                // ���������� �� ��° ������ ����
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
                    Debug.LogWarning("[MapManager] LateUpdate ���� �� RightPivot �Ǵ� LeftPivot�� ã�� �� �����ϴ�.");
                }
            }
            pendingAdjustmentPrefab = null;
        }
    }
}
