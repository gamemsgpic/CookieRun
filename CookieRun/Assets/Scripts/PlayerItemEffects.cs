using UnityEngine;

public class PlayerItemEffects : MonoBehaviour
{
    public GameObject magnet;
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

    public void UpdateItemEffects()
    {
        if (resurrectionWait)
        {
            resurrectionWaitTime += Time.unscaledDeltaTime;
            Time.timeScale = 0f;
            if (resurrectionWaitTime >= 2f)
            {
                Time.timeScale = 1f;
                resurrectionWaitTime = 0f;
                resurrectionWait = false;
            }
        }

        if (giant)
        {
            giantEffectTime += Time.deltaTime;
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
                    magnetEffectTime += Time.deltaTime;
                    if (magnetEffectTime >= itemEffectTime)
                    {
                        OnOffMagnet(true, angelMagnetRadius, false);
                    }
                }
                else
                {
                    magnet.SetActive(true);
                    OnOffMagnet(true, angelMagnetRadius * 0.5f, true);
                    magnetEffectTime += Time.deltaTime;
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
                    magnetEffectTime += Time.deltaTime;
                    if (magnetEffectTime >= itemEffectTime)
                    {
                        DisableMagnet(); // ���׳� ���� �߰�
                    }
                }
                else
                {
                    magnet.SetActive(true);
                    OnOffMagnet(true, angelMagnetRadius * 0.5f, false);
                    magnetEffectTime += Time.deltaTime;
                    if (magnetEffectTime >= itemEffectTime)
                    {
                        DisableMagnet(); // ���׳� ���� �߰�
                    }
                }
            }
        }
    }

    public void DisableMagnet() // �Ϲ� ĳ���Ϳ��� ���׳��� Ȯ���� ����
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
