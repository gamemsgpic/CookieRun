using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public float hp { get; private set; }
    public int score { get; private set; }
    public int coins { get; private set; }

    public bool onDeath { get; private set; } = false;
    public UIManager uiManager;
    [SerializeField] private Slider hpSlider; // �ν����Ϳ��� �����̴� ����

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
    }

    private void Update()
    {
        if (hp <= 0)
        {
            onDeath = true;
            uiManager.ShowGameOver();
            Time.timeScale = 0f;
        }
    }

    // HP ����
    public void ReduceHp(float amount)
    {
        hp -= amount;
        if (hp < 0) hp = 0;

        UpdateHpSlider();
    }

    // HP ����
    public void IncreaseHp(float amount)
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
}
