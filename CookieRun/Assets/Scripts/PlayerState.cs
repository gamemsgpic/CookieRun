using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public GameObject magnet;
    public float itemEffectTime = 3f;
    public float giantEffectTime = 0f;
    public float magnetEffectTime = 0f;
    public bool onMagnet { get; set; } = false;
    public bool onAngelMagnet { get; set; } = false;
    public bool giant { get; private set; } = false;
    public bool invincibility { get; private set; } = false; // ���� ���� ����

    public float maxHp = 100f;
    public float upWave = 30f;
    public float hp { get; set; }
    public float wave { get; private set; }
    private int oneCall = 1;
    public int resurrection = 0;
    public int score { get; private set; }
    public int coins { get; private set; }
    public int currentWave { get; private set; } = 1;
    public bool onDeath { get; private set; } = false;
    public Vector3 normalScale { get; private set; }
    private float magnetRadius = 0f;
    public float maxMagnetRadius = 6f;
    public float angelMagnetRadius = 3f;
    public UIManager uiManager;
    [SerializeField] private Slider hpSlider; // �ν����Ϳ��� �����̴� ����
    [SerializeField] private Slider waveSlider; // �ν����Ϳ��� �����̴� ����

    private void Start()
    {
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

        if (gameObject.name == "Zombie")
        {
            resurrection = 2;
        }
        else
        {
            resurrection = 0;
        }
        // HP �ʱ�ȭ
        hp = maxHp;
        wave = 0f;

        currentWave = 1;

        // �����̴� �ʱ�ȭ
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;
        }

        if (waveSlider != null)
        {
            waveSlider.maxValue = upWave;
            waveSlider.value = wave;
        }
    }

    private void Update()
    {
        if (giant)
        {
            giantEffectTime += Time.deltaTime;
            if (giantEffectTime >= itemEffectTime)
            {
                ChangeScale(normalScale, false);
                Oninvincibility(true);
            }
        }

        // [�ڼ� ������ ȿ�� ����] onMagnet ���¸� �ùٸ��� ����
        if (gameObject.name != "Angel")
        {
            if (onMagnet)
            {
                magnet.SetActive(true);
                magnetEffectTime += Time.deltaTime;
                if (magnetEffectTime >= itemEffectTime)
                {
                    OnOffMagnet(true, maxMagnetRadius, false);
                }
            }
            else
            {
                magnet.SetActive(false);
                magnetEffectTime = 0f; // �ð� �ʱ�ȭ (���� �ð��� �������� �ʵ���)
            }
        }
        else
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
                    magnetEffectTime = 0f;
                }
                else
                {
                    OnOffMagnet(true, angelMagnetRadius * 0.5f, false);
                    magnetEffectTime = 0f;
                }
            }
        }


        // [���̺� �����̴��� ������ ���� ���� ����]
        if (currentWave <= 4)
        {
            wave += Time.deltaTime; // ���� �ӵ��� ���������� ����
            UpdateWaveSlider();
            if (wave >= upWave)
            {
                currentWave++;
                wave = 0f; // ���̺갡 ������ �� �ʱ�ȭ (���� �� ���� ����)
            }
        }
        else
        {
            wave = upWave;
        }

        // [HP�� ���ڱ� Ȯ �������� ���� ����]
        if (hp > 0)
        {
            hp -= Time.deltaTime * 5f; // �ڿ������� �����ϵ��� �ӵ� ����
            UpdateHpSlider();
        }

        // [��Ȱ�� ������� �ʴ� ���� �ذ�]
        if (hp <= 0)
        {
            if (resurrection > 0)
            {
                resurrection--;
                hp = maxHp; // HP ���� ȸ��
                UpdateHpSlider();
                return; // ��Ȱ �� �߰����� HP ���� ����
            }
            else
            {
                onDeath = true;
                Debug.Log("���� ����: " + score + " | ���� ����: " + coins);
                Time.timeScale = 0f;
                if (oneCall >= 1)
                {
                    uiManager.ShowScoreBoardWindow();
                    oneCall--;
                }
            }
        }
    }



    // HP ����
    public void MinusHp(float amount)
    {
        hp -= amount;
        if (hp < 0) hp = 0;

        UpdateHpSlider();
    }

    // HP ����
    public void PlusHp(float amount)
    {
        hp += amount;
        UpdateHpSlider();
    }

    // ���� ����
    public void AddScore(int amount)
    {
        score += amount;
    }

    // ���� ����
    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void OnOffMagnet(bool om, float radius, bool angelmagnet)
    {
        if (gameObject.name != "Angel")
        {
            onMagnet = om;
            magnetEffectTime = 0f;
        }
        else
        {
            onMagnet = om;
            magnet.GetComponent<CircleCollider2D>().radius = radius;
            onAngelMagnet = angelmagnet;
        }
    }

    public void ChangeScale(Vector3 setscals, bool setbool)
    {
        transform.localScale = setscals;
        giant = setbool;
        giantEffectTime = 0f;
    }

    public void Oninvincibility(bool setinvi)
    {
        invincibility = setinvi;
    }

    // �����̴� ������Ʈ
    private void UpdateHpSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.value = hp;
        }
    }

    private void UpdateWaveSlider()
    {
        if (waveSlider != null)
        {
            waveSlider.value = wave;
        }
    }

}




//using UnityEngine;
//using UnityEngine.UI;

//public class PlayerState : MonoBehaviour
//{
//    public GameObject magnet;
//    public float itemEffectTime = 3f;
//    public float magnetEffectTime = 0f;
//    public bool onMagnet { get; set; } = false;
//    private bool giant = false;

//    public float maxHp = 100f;
//    public float upWave = 30f;
//    public float hp { get; set; }
//    public float wave { get; private set; }
//    public int resurrection = 3;
//    public int score { get; private set; }
//    public int coins { get; private set; }
//    public int currentWave { get; private set; } = 1;
//    public bool onDeath { get; private set; } = false;
//    public UIManager uiManager;
//    [SerializeField] private Slider hpSlider;
//    [SerializeField] private Slider waveSlider;

//    private void Start()
//    {
//        OnOffMagnet(onMagnet);
//        hp = maxHp;
//        wave = 0f;
//        currentWave = 1;
//        resurrection = 3;

//        if (hpSlider != null)
//        {
//            hpSlider.maxValue = maxHp;
//            hpSlider.value = hp;
//        }

//        if (waveSlider != null)
//        {
//            waveSlider.maxValue = upWave;
//            waveSlider.value = wave;
//        }
//    }

//    private void Update()
//    {
//        if (onMagnet)
//        {
//            magnetEffectTime += Time.deltaTime;
//            if (magnetEffectTime >= itemEffectTime)
//            {
//                OnOffMagnet(false);
//            }
//        }

//        if (currentWave <= 4)
//        {
//            wave += Time.deltaTime;
//            UpdateWaveSlider();
//            if (wave >= upWave)
//            {
//                currentWave++;
//                wave = 0f;
//            }
//        }
//        else
//        {
//            wave = upWave;
//        }

//        if (hp > 0)
//        {
//            hp -= Time.deltaTime * 5f;
//            UpdateHpSlider();
//        }

//        if (hp <= 0)
//        {
//            if (resurrection > 0)
//            {
//                resurrection--;
//                hp = maxHp;
//                UpdateHpSlider();
//                return;
//            }
//            else
//            {
//                onDeath = true;
//                uiManager.ShowGameOver();
//                Time.timeScale = 0f;
//            }
//        }
//    }

//    public void MinusHp(float amount)
//    {
//        hp -= amount;
//        if (hp < 0) hp = 0;
//        UpdateHpSlider();
//    }

//    public void PlusHp(float amount)
//    {
//        hp += amount;
//        UpdateHpSlider();
//    }

//    public void AddScore(int amount)
//    {
//        score += amount;
//        uiManager.UpdateScoreText(score);
//    }

//    public void AddCoins(int amount)
//    {
//        coins += amount;
//        uiManager.UpdateCoinText(coins);
//    }

//    public void OnOffMagnet(bool om)
//    {
//        magnet.SetActive(om);
//        onMagnet = om;
//        magnetEffectTime = 0f;

//    }

//    private void UpdateHpSlider()
//    {
//        if (hpSlider != null)
//        {
//            hpSlider.value = hp;
//        }
//    }

//    private void UpdateWaveSlider()
//    {
//        if (waveSlider != null)
//        {
//            waveSlider.value = wave;
//        }
//    }
//}
