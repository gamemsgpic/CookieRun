using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float attractionSpeed = 10f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ApplyItemMagnet item = collision.GetComponent<ApplyItemMagnet>();
            if (item != null && !item.isMovingToPlayer)
            {
                item.MoveTowardsPlayer(transform.parent, attractionSpeed);
            }
        }
    }
}
