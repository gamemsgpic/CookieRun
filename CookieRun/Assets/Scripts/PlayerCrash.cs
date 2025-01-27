using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    private GameManager gm;
    private PlayerState playerState;

    private bool invincibility = false; // ���� ���� ����
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
            Debug.LogError("PlayerState�� ������� �ʾҽ��ϴ�.");
        }
        originalColor = rbSprite.color;
    }

    private void Update()
    {
        // ���� ���� ó��
        if (invincibility)
        {
            HandleInvincibility();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ Ʈ���� �浹���� ��
        if (!invincibility && collision.CompareTag("Trap"))
        {
            Debug.Log("������ �޾Ҵ�!");
            invincibility = true; // ���� ���� ����
            playerState.MinusHp(10f); // �������� HP ����
        }

        // Item ���̾�� �浹 Ȯ��
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ApplyItemMagnet item = collision.GetComponent<ApplyItemMagnet>();
            if (item != null && item.isMovingToPlayer) // Magnet�� ���� �̵� ���� ��츸 ó��
            {
                ApplyItemEffect(item);
            }
        }
    }

    private void ApplyItemEffect(ApplyItemMagnet item)
    {
        Debug.Log($"������ ȿ�� ����: {item.gameObject.name}");
        playerState.AddScore(item.Score);
        playerState.AddCoins(item.Coins);
        gm.UpdateScore(playerState.score);
        gm.UpdateCoin(playerState.coins);
        item.gameObject.SetActive(false); // ������ ��Ȱ��ȭ
    }

    private void HandleInvincibility()
    {
        inviStartTime += Time.deltaTime;
        blinkStartTime += Time.deltaTime;

        if (blinkStartTime < blinkEndTime)
        {
            rbSprite.color = color; // �����̴� ���� ����
        }
        else if (blinkStartTime > blinkEndTime && blinkStartTime < blinkReEndTime)
        {
            rbSprite.color = originalColor; // ���� ���� ����
        }
        else
        {
            blinkStartTime = 0f; // ������ ����
        }

        if (inviStartTime > inviEndTime)
        {
            rbSprite.color = originalColor; // ���� ���� ����
            invincibility = false; // ���� ���� ����
            inviStartTime = 0f;
            blinkStartTime = 0f;
        }
    }
}
