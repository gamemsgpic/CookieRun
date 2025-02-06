using UnityEngine;

public class MapMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    private MapManager mapManager;
    public Transform startMapPos; // StartMapPos 참조
    private Transform rightPivot; // 오른쪽 피봇 참조

    void Start()
    {
        // RightPivot을 자식 오브젝트에서 찾음
        rightPivot = transform.Find("RightPivot");
        mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            startMapPos = mapManager.StartMapPos;
        }
        else
        {
            Debug.LogError("MapManager를 찾을 수 없습니다! StartMapPos를 설정할 수 없습니다.");
        }
    }

    void Update()
    {
        // 왼쪽으로 이동
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // RightPivot이 StartMapPos보다 왼쪽으로 가면 MapManager에 비활성화 요청
        if (mapManager != null && rightPivot.position.x < mapManager.StartMapPos.position.x)
        {
            mapManager.DeactivatePrefab(gameObject);
        }
    }
}
