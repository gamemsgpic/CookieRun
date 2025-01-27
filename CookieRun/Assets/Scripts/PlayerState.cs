using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public float wave { get; private set; }
    public float upWave = 30f;
    public float hp { get; private set; }
    public int score { get; private set; }
    public int coins { get; private set; }
    public int currentWave { get; private set; } = 1;
    public bool onDeath { get; private set; } = false;
    public UIManager uiManager;
    [SerializeField] private Slider hpSlider; // �ν����Ϳ��� �����̴� ����
    [SerializeField] private Slider waveSlider; // �ν����Ϳ��� �����̴� ����

    private void Start()
    {
        // HP �ʱ�ȭ
        hp = 30f;

        // �����̴� �ʱ�ȭ
        if (hpSlider != null)
        {
            hpSlider.maxValue = hp;
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
        if (currentWave <= 4)
        {
            wave += Time.deltaTime;
            UpdateWaveSlider();
            if (wave >= upWave)
            {
                currentWave++;
                wave = 0f;
            }
        }
        else
        {
            wave = upWave;
        }

        if (hp <= 0)
        {
            onDeath = true;
            uiManager.ShowGameOver();
            Time.timeScale = 0f;
        }
        else
        {
            
            hp -= Time.deltaTime;
            UpdateHpSlider();
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
