using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSlide : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerState playerState;
    public UIManager uiManager;
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
        playerState = GetComponent<PlayerState>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        normalSize = boxCollider2D.size;
        normalColliderOffset = boxCollider2D.offset;
        if (gameObject.name == "Angel")
        {
            slideSize = new Vector2(0.7f, 0.6f);
            slideColliderOffset = new Vector2(0.16f, 0.25f);
        }
        if (gameObject.name == "Brave")
        {
            slideSize = new Vector2(0.7f, 0.6f);
            slideColliderOffset = new Vector2(0.16f, 0.25f);
        }
        if (gameObject.name == "Zombie")
        {
            slideSize = new Vector2(0.7f, 0.6f);
            slideColliderOffset = new Vector2(0.16f, 0.25f);
        }
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.DownArrow) && playerMovement.isGrounded)
        //{
        //    isSlide = true;
        //    animator.SetBool("Slide", isSlide);
        //    boxCollider2D.size = slideSize;
        //    boxCollider2D.offset = slideColliderOffset;
        //}
        //else
        //{
        //    isSlide = false;
        //    animator.SetBool("Slide", isSlide);
        //    boxCollider2D.size = normalSize;
        //    boxCollider2D.offset = normalColliderOffset;
        //}
    }

    public void OnSlideButtonDown()
    {
        Debug.Log("on");
        if (!playerMovement.isJumping && !playerState.onDeath && !uiManager.pause)
        {
            isSlide = true;
            Debug.Log("슬라이드 트루!");
            animator.SetBool("Slide", isSlide);
            Debug.Log("슬라이드 애니메이션!");
            boxCollider2D.size = slideSize;
            boxCollider2D.offset = slideColliderOffset;
            Debug.Log("원래 콜라이더!");
        }
    }
    public void OnSlideButtonUp()
    {
        Debug.Log("off");
        isSlide = false;
        Debug.Log("슬라이드 펄스!");
        animator.SetBool("Slide", isSlide);
        Debug.Log("런 애니메이션!");
        boxCollider2D.size = normalSize;
        boxCollider2D.offset = normalColliderOffset;
        Debug.Log("콜라이더 사이즈 변경");
    }
}
