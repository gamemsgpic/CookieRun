using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private Vector2 normalSize;
    private Vector2 slideSize;
    private Vector2 normalColliderOffset;
    private Vector2 slideColliderOffset;
    private bool isSlide = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        normalSize = boxCollider2D.size;
        normalColliderOffset = boxCollider2D.offset;
        slideSize = new Vector2(0.7f, 0.6f);
        slideColliderOffset = new Vector2(0f, 0.25f);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && playerMovement.isGrounded)
        {
            isSlide = true;
            animator.SetBool("Slide", isSlide);
            boxCollider2D.size = slideSize;
            boxCollider2D.offset = slideColliderOffset;
        }
        else
        {
            isSlide = false;
            animator.SetBool("Slide", isSlide);
            boxCollider2D.size = normalSize;
            boxCollider2D.offset = normalColliderOffset;
        }
    }

    public void OnSlideButtonDown()
    {
        if (playerMovement.isGrounded)
        {
            isSlide = true;
            animator.SetBool("Slide", isSlide);
            boxCollider2D.size = slideSize;
            boxCollider2D.offset = slideColliderOffset;
        }
    }
    public void OnSlideButtonUp()
    {
        isSlide = false;
        animator.SetBool("Slide", isSlide);
        boxCollider2D.size = normalSize;
        boxCollider2D.offset = normalColliderOffset;
    }
}
