using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerCrash : MonoBehaviour
{
    public AudioManager audioManager;
    public UIManager uiManager;
    public GameObject damageEffectPanel;
    private PlayerItemEffects playerItemEffcts;
    private PlayerState playerState;

    private float inviStartTime = 0f;
    private float blinkStartTime = 0f;
    public float blinkEndTime = 0.1f;
    public float blinkReEndTime = 0.2f;
    public float inviEndTime = 2.4f;
    public float damage = 5f;
    private float damageEftStartTime = 0f;
    public float damageEftEndTime = 0.2f;
    public Color color;
    public Color originalColor;
    private SpriteRenderer rbSprite;

    private void Start()
    {
        playerItemEffcts = GetComponent<PlayerItemEffects>();
        playerState = GetComponent<PlayerState>();
        rbSprite = GetComponent<SpriteRenderer>();
        damageEffectPanel.SetActive(false);
        if (playerState == null)
        {
            Debug.LogError("PlayerState가 연결되지 않았습니다.");
        }
        originalColor = rbSprite.color;
    }

    private void Update()
    {
        // 무적 상태 처리
        if (playerItemEffcts.invincibility)
        {
            HandleInvincibility();
        }

        if (playerItemEffcts.DamageEffect)
        {
            HandleDamageEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerItemEffcts.giant)
        {

            if (!playerItemEffcts.invincibility && collision.CompareTag("Trap"))
            {
                audioManager.PlayerhitSound();
                playerState.PlayHitAni();
                playerState.Oninvincibility(true);
                playerItemEffcts.OnDamageEffect(true);
                playerState.MinusHp(damage);
            }
        }
        else
        {
            if (collision.CompareTag("Trap"))
            {
                Trap trapScript = collision.GetComponent<Trap>();
                if (trapScript != null)
                {
                    trapScript.LaunchTrap();
                }
                else
                {
                    Debug.LogError("[오류] 트랩에 Trap 스크립트가 없습니다!");
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ApplyItemMagnet item = collision.GetComponent<ApplyItemMagnet>();

            if (item != null)
            {
                //if (collision.CompareTag("Jelly")) // 자석 아이템 태그 확인
                //{
                //    audioManager.PlayerJellySound();
                //}

                //if (collision.CompareTag("Magnet")) // 자석 아이템 태그 확인
                //{
                //    audioManager.PlayerItemClipSound();
                //    playerItemEffcts.OnOffMagnet(item.onItmeEffect, playerItemEffcts.maxMagnetRadius, true); // 자석 효과 활성화
                //    ApplyItemEffect(item);
                //}

                if (playerItemEffcts.onMagnet)
                {
                    item.MoveTowardsPlayer(transform, item.moveDuration);
                    ApplyItemEffect(item);
                }
                else //(!playerItemEffcts.onMagnet) // 자석이 비활성화된 상태에서도 효과 적용
                {
                    ApplyItemEffect(item);
                }
                //else
                //{
                //    item.MoveTowardsPlayer(transform, item.moveDuration);
                //}


                //if (collision.CompareTag("Potion"))
                //{
                //    playerState.PlusHp(item.plusHp);
                //}

                //if (collision.CompareTag("Crystal"))
                //{
                //    playerItemEffcts.ChangeScale(item.giantization, true);

                //}

                //if (collision.CompareTag("Giantization"))
                //{
                //    playerItemEffcts.ChangeScale(item.giantization, true);
                //}
            }
        }

        if (collision.CompareTag("DeadZone"))
        {
            playerState.SetOnDeath(true);
            uiManager.ShowScoreBoardWindow();
            Time.timeScale = 0f;
        }

        if (collision.CompareTag("SpeedUp"))
        {
            playerState.SpeedUP();
        }
    }




    public void ApplyItemEffect(ApplyItemMagnet item)
    {
        if (item.tag == "Jelly")
        {
            audioManager.PlayerJellySound();
        }

        if (item.tag == "Coin")
        {
            audioManager.PlayerCoinSound();
        }

        if (item.score == 0 && item.coins == 0)
        {
            Debug.LogError($"[오류] 아이템 {item.gameObject.name}의 점수 및 코인이 0입니다. 아이템 속성을 확인하세요!");
        }


        int previousScore = playerState.score;
        int previousCoins = playerState.coins;

        playerState.AddScore(item.score);
        playerState.AddCoins(item.coins);

        if (item.tag == "Magnet") // 자석 아이템 태그 확인
        {
            audioManager.PlayerItemClipSound();
            playerItemEffcts.OnOffMagnet(item.onItmeEffect, playerItemEffcts.maxMagnetRadius, true); // 자석 효과 활성화
        }

        if (item.tag == "Potion")
        {
            playerState.PlusHp(item.plusHp);
        }
        else if (item.tag == "Crystal")
        {
            playerState.PlusCrystal(item.Crystal);
        }
        else if(item.tag == "Giantization")
        {
            playerItemEffcts.ChangeScale(item.giantization, true);
        }
        uiManager.UpdateScoreText(playerState.score);
        uiManager.UpdateCoinText(playerState.coins);

        item.gameObject.SetActive(false);


    }

    private void HandleDamageEffect()
    {
        if (!uiManager.pause)
        {
            damageEftStartTime += Time.unscaledDeltaTime;
            if (!playerState.onDeath)
            {

                if (damageEftStartTime < damageEftEndTime)
                {
                    damageEffectPanel.SetActive(true);
                    Time.timeScale = 0;
                    Time.timeScale += Time.unscaledDeltaTime;
                }
                else
                {
                    damageEffectPanel.SetActive(false);
                    Time.timeScale += Time.unscaledDeltaTime;
                }

                if (damageEftStartTime > damageEftEndTime * 5f)
                {
                    playerState.OnDamageEffect(false);
                    Time.timeScale = playerState.currentTimeScale;
                    damageEftStartTime = 0f;
                }
            }
        }
    }


    private void HandleInvincibility()
    {
        if (!uiManager.pause)
        {
            inviStartTime += Time.unscaledDeltaTime;
            blinkStartTime += Time.unscaledDeltaTime;

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
                playerState.Oninvincibility(false); // 무적 상태 종료
                inviStartTime = 0f;
                blinkStartTime = 0f;
            }
        }
    }
}
