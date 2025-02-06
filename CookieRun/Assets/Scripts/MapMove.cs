using UnityEngine;

public class MapMove : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    private MapManager mapManager;
    public Transform startMapPos; // StartMapPos ����
    private Transform rightPivot; // ������ �Ǻ� ����

    void Start()
    {
        // RightPivot�� �ڽ� ������Ʈ���� ã��
        rightPivot = transform.Find("RightPivot");
        mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            startMapPos = mapManager.StartMapPos;
        }
        else
        {
            Debug.LogError("MapManager�� ã�� �� �����ϴ�! StartMapPos�� ������ �� �����ϴ�.");
        }
    }

    void Update()
    {
        // �������� �̵�
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // RightPivot�� StartMapPos���� �������� ���� MapManager�� ��Ȱ��ȭ ��û
        if (mapManager != null && rightPivot.position.x < mapManager.StartMapPos.position.x)
        {
            mapManager.DeactivatePrefab(gameObject);
        }
    }
}
