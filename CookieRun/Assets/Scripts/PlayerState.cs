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
    [SerializeField] private Slider hpSlider; // 인스펙터에서 슬라이더 연결

    private void Start()
    {
        // HP 초기화
        hp = 30f;

        // 슬라이더 초기화
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

    // HP 감소
    public void ReduceHp(float amount)
    {
        hp -= amount;
        if (hp < 0) hp = 0;

        UpdateHpSlider();
    }

    // HP 증가
    public void IncreaseHp(float amount)
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

    // 슬라이더 업데이트
    private void UpdateHpSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.value = hp;
        }
    }
}
