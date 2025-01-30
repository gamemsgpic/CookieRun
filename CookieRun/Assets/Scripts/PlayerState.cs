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
    [SerializeField] private Slider hpSlider; // 인스펙터에서 슬라이더 연결
    [SerializeField] private Slider waveSlider; // 인스펙터에서 슬라이더 연결

    private void Start()
    {
        normalScale = transform.localScale;

        OnOffMagnet(false);
        // HP 초기화
        hp = maxHp;
        wave = 0f;

        currentWave = 1;

        // 슬라이더 초기화
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

        // [자석 아이템 효과 종료] onMagnet 상태를 올바르게 제어
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
            currentEffectTime = 0f; // 시간 초기화 (이전 시간이 누적되지 않도록)
        }

        // [웨이브 슬라이더가 빠르게 차는 문제 수정]
        if (currentWave <= 4)
        {
            wave += Time.deltaTime; // 증가 속도를 안정적으로 조절
            UpdateWaveSlider();
            if (wave >= upWave)
            {
                currentWave++;
                wave = 0f; // 웨이브가 증가할 때 초기화 (이전 값 누적 방지)
            }
        }
        else
        {
            wave = upWave;
        }

        // [HP가 갑자기 확 떨어지는 문제 수정]
        if (hp > 0)
        {
            hp -= Time.deltaTime * 5f; // 자연스럽게 감소하도록 속도 조정
            UpdateHpSlider();
        }

        // [부활이 적용되지 않는 문제 해결]
        if (hp <= 0)
        {
            if (resurrection > 0)
            {
                resurrection--;
                hp = maxHp; // HP 완전 회복
                UpdateHpSlider();
                return; // 부활 후 추가적인 HP 감소 방지
            }
            else
            {
                onDeath = true;
                Debug.Log("최종 점수: " + score + " | 최종 코인: " + coins);
                uiManager.AnimateFinalStats(score, coins);
                uiManager.ShowScoreBoardWindow();
                Time.timeScale = 0f;
            }
        }
    }



    // HP 감소
    public void MinusHp(float amount)
    {
        hp -= amount;
        if (hp < 0) hp = 0;

        UpdateHpSlider();
    }

    // HP 증가
    public void PlusHp(float amount)
    {
        hp += amount;
        UpdateHpSlider();
    }

    // 점수 증가
    public void AddScore(int amount)
    {
        score += amount;
    }

    // 코인 증가
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
        
    // 슬라이더 업데이트
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
