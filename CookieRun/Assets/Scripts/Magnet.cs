using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float attractionSpeed = 5f; // �������� ���׳ݿ� ���� �̵��ϴ� �ӵ�

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ApplyItemMagnet item = collision.GetComponent<ApplyItemMagnet>();
            if (item != null && !item.isMovingToPlayer) // �̹� �̵� ���� �ƴ� ��츸 ó��
            {
                Debug.Log($"Magnet�� {collision.gameObject.name} ����");
                item.MoveTowardsPlayer(transform.parent, attractionSpeed); // �÷��̾��� ��ġ�� ��ǥ�� �̵�
            }
        }
    }
}
