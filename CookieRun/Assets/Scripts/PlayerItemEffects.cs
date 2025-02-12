using System.Collections;
using UnityEngine;

public class PlayerItemEffects : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject magnet;
    private Animator animator;
    public float itemEffectTime = 3f;
    public float giantEffectTime = 0f;
    public float magnetEffectTime = 0f;
    public float resurrectionWaitTime = 0f;
    public bool onMagnet { get; set; } = false;
    public bool onAngelMagnet { get; set; } = false;
    public bool onNormalMagnet { get; set; } = false;
    public bool giant { get; private set; } = false;
    public bool invincibility { get; private set; } = false;
    public bool resurrectionWait { get; private set; } = false;
    public bool DamageEffect { get; private set; } = false;

    private float magnetRadius = 0f;
    public float maxMagnetRadius = 6f;
    public float angelMagnetRadius = 3f;
    private Vector3 normalScale;

    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void InitializeEffects(GameObject magnetObject)
    {
        magnet = magnetObject;
        normalScale = transform.localScale;
        magnetRadius = magnet.GetComponent<CircleCollider2D>().radius;

        if (gameObject.name != "Angel")
        {
            magnet.SetActive(false);
            OnOffMagnet(false, maxMagnetRadius, false);
        }
        else
        {
            magnet.SetActive(true);
            OnOffMagnet(true, angelMagnetRadius, false);
        }
    }

    public void UpdateItemEffects(float timescale)
    {
        if (resurrectionWait)
        {
            PlayUnscaledAnimation("Resurrection");
            resurrectionWaitTime += Time.unscaledDeltaTime * 0.5f;
            Time.timeScale = resurrectionWaitTime;
            if (resurrectionWaitTime >= 1f)
            {
                Time.timeScale = timescale;
                Oninvincibility(true);
                resurrectionWaitTime = 0f;
                resurrectionWait = false;
            }
        }

        if (giant)
        {
            giantEffectTime += Time.unscaledDeltaTime;
            if (giantEffectTime >= itemEffectTime)
            {
                ChangeScale(normalScale, false);
                Oninvincibility(true);
            }
        }

        if (gameObject.name == "Angel")
        {
            if (onMagnet && onAngelMagnet)
            {
                if (!giant)
                {
                    magnet.SetActive(true);
                    OnOffMagnet(true, maxMagnetRadius, true);
                    magnetEffectTime += Time.unscaledDeltaTime;
                    if (magnetEffectTime >= itemEffectTime)
                    {
                        OnOffMagnet(true, angelMagnetRadius, false);
                    }
                }
                else
                {
                    magnet.SetActive(true);
                    OnOffMagnet(true, angelMagnetRadius * 0.5f, true);
                    magnetEffectTime += Time.unscaledDeltaTime;
                    if (magnetEffectTime >= itemEffectTime)
                    {
                        OnOffMagnet(true, angelMagnetRadius * 0.5f, false);
                    }
                }
            }
            else
            {
                if (!giant)
                {
                    OnOffMagnet(true, angelMagnetRadius, false);
                }
                else
                {
                    OnOffMagnet(true, angelMagnetRadius * 0.5f, false);
                }
                magnetEffectTime = 0f;
            }
        }
        else
        {
            if (onMagnet)
            {
                if (!giant)
                {
                    magnet.SetActive(true);
                    OnOffMagnet(true, maxMagnetRadius, false);
                    magnetEffectTime += Time.unscaledDeltaTime;
                    if (magnetEffectTime >= itemEffectTime)
                    {
                        DisableMagnet(); // 마그넷 끄기 추가
                    }
                }
                else
                {
                    magnet.SetActive(true);
                    OnOffMagnet(true, angelMagnetRadius * 0.5f, false);
                    magnetEffectTime += Time.unscaledDeltaTime;
                    if (magnetEffectTime >= itemEffectTime)
                    {
                        DisableMagnet(); // 마그넷 끄기 추가
                    }
                }
            }
        }
    }

    public void PlayUnscaledAnimation(string animationName)
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime; // 타임스케일 영향 안 받음
        animator.SetTrigger("Resurrection");
        StartCoroutine(ResetAnimatorMode());
    }

    private IEnumerator ResetAnimatorMode()
    {
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.updateMode = AnimatorUpdateMode.Normal; // 다시 기본 설정으로 복구
    }

    public void DisableMagnet() // 일반 캐릭터에서 마그넷을 확실히 끄기
    {
        magnet.SetActive(false);
        onMagnet = false;
        magnetEffectTime = 0f;
    }

    public void ResurrectionWait(bool wait)
    {
        resurrectionWait = wait;
    }

    public void ChangeScale(Vector3 setScale, bool setBool)
    {
        audioManager.PlayerItemClipSound();
        transform.localScale = setScale;
        giant = setBool;
        giantEffectTime = 0f;
    }

    public void Oninvincibility(bool setinvi)
    {
        invincibility = setinvi;
    }

    public void OnDamageEffect(bool damageEffect)
    {
        DamageEffect = damageEffect;
    }

    public void OnOffMagnet(bool om, float radius, bool angelMagnet)
    {
        onMagnet = om;
        magnet.GetComponent<CircleCollider2D>().radius = radius;
        if (gameObject.name == "Angel")
        {
            onAngelMagnet = angelMagnet;
        }
    }

    public void ChangeMagnetSize(float radius)
    {
        magnet.GetComponent<CircleCollider2D>().radius = radius;
    }
}
