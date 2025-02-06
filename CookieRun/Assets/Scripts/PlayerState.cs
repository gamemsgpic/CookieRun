using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public GameObject magnet;
    public MapManager mapManager;
    private Animator animator;
    public float itemEffectTime = 3f;
    public float maxHp = 100f;
    public float upWave = 30f;
    public float hp { get; set; }
    private float wave = 0f;
    private bool oneCall = true;
    private bool oneCall2 = true;
    public int resurrection = 0;
    public int score { get; private set; }
    public int coins { get; private set; }
    public int currentWave { get; private set; } = 1;
    public bool onDeath { get; private set; } = false;
    public bool DamageEffect { get; private set; } = false;

    public float speedUpControl = 0.1f;

    private float normalTimeScale = 1f;
    public float timeScale { get; private set; } = 1f;
    public UIManager uiManager;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider waveSlider;

    private PlayerItemEffects itemEffects;

    private void Start()
    {
        animator = GetComponent<Animator>();
        itemEffects = GetComponent<PlayerItemEffects>();
        itemEffects.InitializeEffects(magnet);

        hp = maxHp;
        wave = 0f;
        currentWave = 1;

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

        timeScale = 1f;
        Time.timeScale = normalTimeScale;

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        itemEffects.UpdateItemEffects(timeScale);

        if (currentWave <= 4)
        {
            wave += Time.deltaTime;
            UpdateWaveSlider();
            if (wave >= upWave)
            {
                currentWave++;
                wave = 0f;
                mapManager.StartWave(currentWave);
                timeScale += speedUpControl;
                Time.timeScale = timeScale;
            }
        }
        else
        {
            wave = upWave;
        }

        if (hp > -1f && Time.timeScale > 0f) // 타임스케일이 0이면 HP 감소 중지
        {
            float baseHpDecreaseRate = 1f; // 기본 HP 감소 속도
            float adjustedDeltaTime = Time.unscaledDeltaTime * (1f / Time.timeScale); // 보정된 DeltaTime

            hp -= baseHpDecreaseRate * adjustedDeltaTime;
            UpdateHpSlider();
        }

        if (hp <= 0)
        {
            if (resurrection > 0)
            {
                resurrection--;
                itemEffects.ResurrectionWait(true);
                hp = maxHp;
                UpdateHpSlider();
                return;
            }
            else
            {
                if (oneCall)
                {
                    Time.timeScale = 1f;
                    oneCall = false;
                }
                    SetOnDeath(true);
                Time.timeScale -= Time.deltaTime;
                animator.SetTrigger("Dead");

                if (Time.timeScale <= 0f)
                {
                   
                        Time.timeScale = 0f;
                }
            }
        }
    }

    public void MinusHp(float amount)
    {
        hp -= amount;
        if (hp < 0) hp = 0;
        UpdateHpSlider();
    }

    public void PlusHp(float amount)
    {
        hp += amount;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        UpdateHpSlider();
    }

    public void SetOnDeath(bool death)
    {
        onDeath = death;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void OnDamageEffect(bool damageEffect)
    {
        DamageEffect = damageEffect;
        itemEffects.OnDamageEffect(damageEffect);
    }

    public void Oninvincibility(bool setinvi)
    {
        itemEffects.Oninvincibility(setinvi);
    }

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

    public void PlayHitAni()
    {
        animator.SetTrigger("Hit");
    }

    public void ShowScoreBoard()
    {
        if (oneCall2)
        {
            uiManager.ShowScoreBoardWindow();
            Debug.Log("된다");
            oneCall2 = false;
        }
    }

    public void StopAllAni(float vel)
    {
        animator.speed = vel;
    }
}