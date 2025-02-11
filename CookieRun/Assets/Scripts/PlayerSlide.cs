using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSlide : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerItemEffects playerItemEffects;
    private PlayerState playerState;
    public UIManager uiManager;
    private BoxCollider2D boxCollider2D;
    private CapsuleCollider2D capsuleCollider2D;
    private Animator animator;
    private Vector2 normalSize;
    private Vector2 slideSize;
    private Vector2 normalColliderOffset;
    private Vector2 slideColliderOffset;
    public bool isSlide { get; private set; } = false;

    private bool slideKeyHeld = false; // 슬라이드 버튼이 눌린 상태인지 저장

    private void Start()
    {
        playerItemEffects = GetComponent<PlayerItemEffects>();
        playerMovement = GetComponent<PlayerMovement>();
        playerState = GetComponent<PlayerState>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        normalSize = boxCollider2D.size;
        normalColliderOffset = boxCollider2D.offset;

        slideSize = new Vector2(0.7f, 0.6f);
        slideColliderOffset = new Vector2(0f, 0.25f);
    }

    public void OnSlideButtonDown()
    {
        slideKeyHeld = true; // 슬라이드 버튼을 누른 상태 저장

        if (playerMovement.isGrounded && !playerState.onDeath && !uiManager.pause)
        {
            StartSlide();
        }
    }

    public void OnSlideButtonUp()
    {
        slideKeyHeld = false; // 슬라이드 버튼을 뗀 상태 저장
        StopSlide();
    }

    private void StartSlide()
    {
        isSlide = true;
        animator.SetBool("Slide", isSlide);
        if (!playerItemEffects.giant)
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

    public void StopSlide()
    {
        isSlide = false;
        animator.SetBool("Slide", false); // 애니메이션 상태 변경
        boxCollider2D.size = normalSize;
        boxCollider2D.offset = normalColliderOffset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 착지 시, 슬라이드 버튼이 눌린 상태라면 자동으로 슬라이드 실행
            if (slideKeyHeld && playerMovement.isGrounded && !playerState.onDeath && !uiManager.pause)
            {
                StartSlide();
            }
        }
    }

}
