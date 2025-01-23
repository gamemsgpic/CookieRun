using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public GameObject[] prefabPool; // ����� ������ ���
    public List<GameObject> activePrefabs = new List<GameObject>(); // Ȱ��ȭ�� ������
    private Dictionary<string, Queue<GameObject>> prefabPools = new Dictionary<string, Queue<GameObject>>(); // ������Ʈ Ǯ
    public int maxActivePrefabs = 5; // ȭ�鿡 ������ �ִ� Ȱ�� ������ ��

    void Start()
    {
        // �� �����տ� ���� ������Ʈ Ǯ ���� / Queue�� ���Լ����ϴ� ģ�� Stack�� ���������̴�. ����� �� ����.
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

        // ������Ʈ Ǯ���� �������� / ��Ȱ�� �ҷ��� �ϴ°� ������ ���� ���� �켱������ ��Ȱ�� ������ �ʿ��ϸ� �̰� ���� ����ϱ�
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

        // ��ġ ���� / �Ǻ��� �°� �� ������ ��ġ���� 
        if (activePrefabs.Count > 0)
        {
            GameObject lastActivePrefab = activePrefabs[activePrefabs.Count - 1];
            Transform rightPivot = lastActivePrefab.transform.Find("RightPivot");
            Transform leftPivot = prefabToActivate.transform.Find("LeftPivot");

            Vector3 offset = rightPivot.position - leftPivot.position;
            prefabToActivate.transform.position += offset;
        }
        else
        {
            prefabToActivate.transform.position = Vector3.zero; // �ʱ� ��ġ ����
        }

        // Ȱ��ȭ �� ����Ʈ �߰�
        prefabToActivate.SetActive(true);
        activePrefabs.Add(prefabToActivate);
    }
}
