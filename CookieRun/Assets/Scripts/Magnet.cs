using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float attractionSpeed = 5f; // 아이템이 마그넷에 의해 이동하는 속도

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ApplyItemMagnet item = collision.GetComponent<ApplyItemMagnet>();
            if (item != null && !item.isMovingToPlayer) // 이미 이동 중이 아닌 경우만 처리
            {
                Debug.Log($"Magnet이 {collision.gameObject.name} 감지");
                item.MoveTowardsPlayer(transform.parent, attractionSpeed); // 플레이어의 위치를 목표로 이동
            }
        }
    }
}
