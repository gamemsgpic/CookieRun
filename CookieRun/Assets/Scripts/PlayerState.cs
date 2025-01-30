using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public GameObject magnet;
    public float itemEffectTime = 3f;
    public float currentEffectTime = 0f;
    public bool onMagnet { get; set; } = false;
    public bool giant { get; private set; } = false;

    public float maxHp = 100f;
    public float upWave = 30f;
    public float hp { get; set; }
    public float wave { get; private set; }
    public int resurrection = 3;
    public int score { get; private set; }
    public int coins { get; private set; }
    public int currentWave { get; private set; } = 1;
    public bool onDeath { get; private set; } = false;
    public Vector3 normalScale {  get; private set; }
    public UIManager uiManager;
    [SerializeField] private Slider hpSlider; // �ν����Ϳ��� �����̴� ����
    [SerializeField] private Slider waveSlider; // �ν����Ϳ��� �����̴� ����

    private void Start()
    {
        normalScale = transform.localScale;

        OnOffMagnet(false);
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
            currentEffectTime += Time.deltaTime;
            if (currentEffectTime >= itemEffectTime)
            {
                ChangeScale(normalScale, false);
            }
        }

        // [�ڼ� ������ ȿ�� ����] onMagnet ���¸� �ùٸ��� ����
        if (onMagnet)
        {
            magnet.SetActive(true);
            currentEffectTime += Time.deltaTime;
            if (currentEffectTime >= itemEffectTime)
            {
                OnOffMagnet(false);
            }
        }
        else
        {
            magnet.SetActive(false);
            currentEffectTime = 0f; // �ð� �ʱ�ȭ (���� �ð��� �������� �ʵ���)
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
                uiManager.AnimateFinalStats(score, coins);
                uiManager.ShowScoreBoardWindow();
                Time.timeScale = 0f;
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

    public void OnOffMagnet(bool om)
    {
        onMagnet = om;
        currentEffectTime = 0f;
    }

    public void ChangeScale(Vector3 setscals, bool setbool)
    {
        transform.localScale = setscals;
        giant = setbool;
        currentEffectTime = 0f;
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
//    public float currentEffectTime = 0f;
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
//            currentEffectTime += Time.deltaTime;
//            if (currentEffectTime >= itemEffectTime)
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
//        currentEffectTime = 0f;

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
