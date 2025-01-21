using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private BoxCollider2D boxCollider2D;
    private Vector2 normalSize;
    private Vector2 slideSize;
    private Vector2 normalColliderOffset;
    private Vector2 slideColliderOffset;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        normalSize = boxCollider2D.size;
        normalColliderOffset = boxCollider2D.offset;
        slideSize = new Vector2(0.7f, 0.5f);
        slideColliderOffset = new Vector2(0f, 0.3f);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && playerMovement.isGrounded)
        {
            boxCollider2D.size = slideSize;
            boxCollider2D.offset = slideColliderOffset;
        }
        else
        {
            boxCollider2D.size = normalSize;
            boxCollider2D.offset = normalColliderOffset;
        }
    }

}
