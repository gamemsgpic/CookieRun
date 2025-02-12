using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public GameObject magnet;
    public MapManager mapManager;
    private Animator animator;
    public float itemEffectTime = 3f;
    private float maxHp;
    public float upWave;
    public float hp { get; set; }
    private float wave = 0f;
    private bool oneCall = true;
    private bool oneCall2 = true;
    public int resurrection = 0;
    public int resurrectionHp = 40;
    public int score { get; private set; }
    public int coins { get; private set; }
    public int crystal { get; private set; } = 0;
    public int currentWave { get; private set; } = 1;
    public bool onDeath { get; private set; } = false;
    public bool DamageEffect { get; private set; } = false;


    public float speedUpControl = 0.4f;

    private float normalTimeScale = 0.8f;
    public float currentTimeScale { get; private set; } = 0.8f;
    public UIManager uiManager;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider waveSlider;

    private PlayerItemEffects itemEffects;

    private float baseHpDecreaseRate;
    private float adjustedDeltaTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        itemEffects = GetComponent<PlayerItemEffects>();
        itemEffects.InitializeEffects(magnet);

        SetGameDataToPlayerState();
        Debug.Log($"{hp}");
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

        Time.timeScale = normalTimeScale;

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        itemEffects.UpdateItemEffects(currentTimeScale);



        if (currentWave <= 3)
        {

            wave += Time.deltaTime;
            UpdateWaveSlider();
            if (wave >= upWave)
            {
                currentWave++;
                if (currentWave > 3)
                {
                    currentWave = 3;
                }
                mapManager.StartWave(currentWave);


                wave = 0f;
            }
        }

        if (hp > -1f && Time.timeScale > 0f) // 타임스케일이 0이면 HP 감소 중지
        {
            hp -= Time.unscaledDeltaTime * 2f;
            UpdateHpSlider();
        }

        if (hp <= 0)
        {
            if (resurrection > 0)
            {
                resurrection--;
                itemEffects.ResurrectionWait(true);
                hp = resurrectionHp;
                UpdateHpSlider();
                return;
            }
            else
            {
                if (oneCall)
                {
                    Time.timeScale = currentTimeScale;
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
        if (hp < 0)
        {
            hp = 0;
        }
        UpdateHpSlider();
    }

    public void SpeedUP()
    {
        currentTimeScale += speedUpControl;
        Time.timeScale = currentTimeScale;
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

    public void PlusCrystal(int amount)
    {
        crystal += amount;
        Debug.Log("다이아 먹음");
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
            Debug.Log($"{hp}");
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

    private void SetGameDataToPlayerState()
    {
        hp = GameData.characterHp;
        maxHp = 200;
    }
}