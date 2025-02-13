using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject magnet;
    public BackGroundManager backGroundManager;
    public MapManager mapManager;
    private Animator animator;
    public float itemEffectTime = 3f;
    private float maxHp;
    public float[] upWave;
    public float waveLength;
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

    private bool oneCallSpeedUp = true;

    public float speedUpControl = 0.2f;

    public float minusHpValue;

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
            waveLength = upWave[0];
            waveSlider.maxValue = waveLength;
            waveSlider.value = wave;
        }


        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Awake()
    {
        Time.timeScale = normalTimeScale;
    }

    private void Update()
    {
        itemEffects.UpdateItemEffects(currentTimeScale);



        if (currentWave <= 4)
        {

            wave += Time.deltaTime;
            UpdateWaveSlider();
            if (wave >= waveLength)
            {
                currentWave++;
                if (currentWave > 4)
                {
                    currentWave = 4;
                    oneCallSpeedUp = false;
                }
                mapManager.StartWave(currentWave);
                backGroundManager.SaveWave(currentWave);
                backGroundManager.StartLight();

                wave = 0f;
                SetWaveLength(currentWave);
                waveSlider.maxValue = waveLength;
            }
        }

        if (hp > -1f && Time.timeScale > 0f) // 타임스케일이 0이면 HP 감소 중지
        {
            hp -= Time.unscaledDeltaTime * minusHpValue;
            UpdateHpSlider();
        }

        if (hp <= 0)
        {
            if (resurrection > 0 && !uiManager.pause)
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
                if (Time.timeScale != 0f)
                {
                    Time.timeScale -= Time.deltaTime;
                }
                animator.SetTrigger("Dead");

                if (Time.timeScale <= 0f)
                {

                    Time.timeScale = 0f;
                }
            }
        }
    }

    private void SetWaveLength(int wave)
    {
        if (wave < 1)
        {
            wave = 1;
        }

        if (wave == 1)
        {
            waveLength = upWave[0];
        }
        else if (wave == 2)
        {
            waveLength = upWave[1];
        }
        else if (wave == 3)
        {
            waveLength = upWave[2];
        }
        else if (wave == 4)
        {
            waveLength = upWave[3];
        }
        else if (wave > 4)
        {
            wave = 4;
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
        if (oneCallSpeedUp)
        {
            currentTimeScale += speedUpControl;
            Time.timeScale = currentTimeScale;
        }
    }

    public void PlusHp(float amount)
    {
        hp += amount;
        if (hp > maxHp)
        {
            hp = maxHp;
            audioManager.PlayerPotionSound();
        }
        UpdateHpSlider();
    }

    public void PlusCrystal(int amount)
    {
        audioManager.PlayerItemClipSound();
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