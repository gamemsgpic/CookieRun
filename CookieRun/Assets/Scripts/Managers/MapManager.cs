using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public GameObject[] prefabPool; // ����� ������ ���
    public List<GameObject> activePrefabs = new List<GameObject>(); // Ȱ��ȭ�� ������
    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // ������Ʈ Ǯ
    public Transform StartMapPos; // StartMapPos ����
    public Transform startGroundRightPos; // StartMapPos ����
    public int maxActivePrefabs = 5; // Ȱ��ȭ�� �������� �ִ� ����

    void Start()
    {
        // �� �����տ� ���� ������Ʈ Ǯ ����
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
        // Ȱ��ȭ ����Ʈ���� ����
        activePrefabs.Remove(prefab);

        // ��Ȱ��ȭ�ϰ� ������Ʈ Ǯ�� ��ȯ
        string prefabName = prefab.name.Replace("(Clone)", "").Trim();
        prefab.SetActive(false);

        if (prefabPools.ContainsKey(prefabName))
        {
            prefabPools[prefabName].Enqueue(prefab);
        }

        // ���ο� ������ Ȱ��ȭ
        ActivatePrefab();
    }

    public void ActivatePrefab()
    {
        // �������� ������ ����
        GameObject selectedPrefab = prefabPool[Random.Range(0, prefabPool.Length)];
        GameObject prefabToActivate;

        // ������Ʈ Ǯ���� ��������
        string prefabName = selectedPrefab.name;
        if (prefabPools[prefabName].Count > 0)
        {
            prefabToActivate = prefabPools[prefabName].Dequeue();
        }
        else
        {
            // Ǯ�� �����ִ� ������Ʈ�� ������ ���� ����
            prefabToActivate = Instantiate(selectedPrefab);
        }

        // ��ġ ����
        if (activePrefabs.Count == 0)
        {
            // ù ��° ������: StartMapPos �������� ��ġ
            prefabToActivate.transform.position = startGroundRightPos.position;
        }
        else
        {
            // ������ ������: ���� �����ʿ� ��ġ
            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");

            Vector3 offset = rightPivot.position - prefabToActivate.transform.Find("LeftPivot").position;
            prefabToActivate.transform.position += offset;
        }

        // ���� �ڽ� ������Ʈ �ʱ�ȭ
        foreach (Transform child in prefabToActivate.transform)
        {
            var itemMagnet = child.GetComponent<ApplyItemMagnet>();
            if (itemMagnet != null)
            {
                itemMagnet.ResetItem(); // �ʱ�ȭ ȣ��
            }
        }

        // Ȱ��ȭ �� ����Ʈ �߰�
        prefabToActivate.SetActive(true);
        activePrefabs.Add(prefabToActivate);
    }
}
