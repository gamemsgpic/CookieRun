using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //public static readonly string scoreformat = "SCORE: {0}";

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public GameObject gameOverWindow;
    public bool pause { get; private set; } = false;

    private void Start()
    {
        HideGameOver(); // 게임 시작 시 GameOver 창 비활성화
    }

    // 점수 텍스트 업데이트
    public void UpdateScoreText(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString();
        }
        else
        {
            Debug.LogError("ScoreText가 연결되지 않았습니다!");
        }

    }

    public void UpdateCoinText(int newCoin)
    {
        if (coinText != null)
        {
            coinText.text = newCoin.ToString();
        }
        else
        {
            Debug.LogError("ScoreText가 연결되지 않았습니다!");
        }

    }

    // 일시 정지 해제
    public void PauseOff()
    {
        Time.timeScale = 1;
        pause = false;
        HideGameOver();
    }

    // 재시작 버튼 클릭 처리
    public void OnClickReStart()
    {
        Time.timeScale = 1; // 시간 재개
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재로드
    }

    public void Pause()
    {
        ShowGameOver();
        pause = true;
        Time.timeScale = 0;
    }

    // GameOver 창 표시
    public void ShowGameOver()
    {
        if (gameOverWindow != null)
        {
            gameOverWindow.SetActive(true); // 창 활성화
        }
        else
        {
            Debug.LogError("GameOverWindow가 연결되지 않았습니다!");
        }
    }

    // GameOver 창 숨김
    public void HideGameOver()
    {
        if (gameOverWindow != null)
        {
            gameOverWindow.SetActive(false); // 창 비활성화
        }
        else
        {
            Debug.LogError("GameOverWindow가 연결되지 않았습니다!");
        }
    }
}
