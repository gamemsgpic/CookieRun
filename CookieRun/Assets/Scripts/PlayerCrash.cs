using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    private GameManager gm;
    private PlayerState playerState;

    private bool invincibility = false; // 무적 상태 여부
    private float inviStartTime = 0f;
    private float blinkStartTime = 0f;
    public float blinkEndTime = 0.1f;
    public float blinkReEndTime = 0.2f;
    public float inviEndTime = 2.4f;
    public Color color;
    public Color originalColor;
    private SpriteRenderer rbSprite;

    private void Start()
    {
        gm = GameManager.Instance;
        playerState = GetComponent<PlayerState>();
        rbSprite = GetComponent<SpriteRenderer>();

        if (playerState == null)
        {
            Debug.LogError("PlayerState가 연결되지 않았습니다.");
        }
        originalColor = rbSprite.color;
    }

    private void Update()
    {
        // 무적 상태 처리
        if (invincibility)
        {
            HandleInvincibility();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 트랩과 충돌했을 때
        if (!invincibility && collision.CompareTag("Trap"))
        {
            Debug.Log("데미지 받았다!");
            invincibility = true; // 무적 상태 시작
            playerState.MinusHp(10f); // 데미지로 HP 감소
        }

        // Item 레이어와 충돌 확인
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ApplyItemMagnet item = collision.GetComponent<ApplyItemMagnet>();
            if (item != null && item.isMovingToPlayer) // Magnet에 의해 이동 중인 경우만 처리
            {
                ApplyItemEffect(item);
            }
        }
    }

    private void ApplyItemEffect(ApplyItemMagnet item)
    {
        Debug.Log($"아이템 효과 적용: {item.gameObject.name}");
        playerState.AddScore(item.Score);
        playerState.AddCoins(item.Coins);
        gm.UpdateScore(playerState.score);
        gm.UpdateCoin(playerState.coins);
        item.gameObject.SetActive(false); // 아이템 비활성화
    }

    private void HandleInvincibility()
    {
        inviStartTime += Time.deltaTime;
        blinkStartTime += Time.deltaTime;

        if (blinkStartTime < blinkEndTime)
        {
            rbSprite.color = color; // 깜빡이는 색상 적용
        }
        else if (blinkStartTime > blinkEndTime && blinkStartTime < blinkReEndTime)
        {
            rbSprite.color = originalColor; // 원래 색상 복원
        }
        else
        {
            blinkStartTime = 0f; // 깜빡임 리셋
        }

        if (inviStartTime > inviEndTime)
        {
            rbSprite.color = originalColor; // 원래 색상 복원
            invincibility = false; // 무적 상태 종료
            inviStartTime = 0f;
            blinkStartTime = 0f;
        }
    }
}
