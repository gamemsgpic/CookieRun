//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class UIManager : MonoBehaviour
//{
//    //public static readonly string scoreformat = "SCORE: {0}";

//    public TextMeshProUGUI scoreText;
//    public TextMeshProUGUI coinText;
//    public GameObject gameOverWindow;
//    public bool pause { get; private set; } = false;

//    private void Start()
//    {
//        HideGameOver(); // 게임 시작 시 GameOver 창 비활성화
//    }

//    // 점수 텍스트 업데이트
//    public void UpdateScoreText(int newScore)
//    {
//        if (scoreText != null)
//        {
//            scoreText.text = newScore.ToString();
//        }
//        else
//        {
//            Debug.LogError("ScoreText가 연결되지 않았습니다!");
//        }

//    }

//    public void UpdateCoinText(int newCoin)
//    {
//        if (coinText != null)
//        {
//            coinText.text = newCoin.ToString();
//        }
//        else
//        {
//            Debug.LogError("ScoreText가 연결되지 않았습니다!");
//        }

//    }

//    // 일시 정지 해제
//    public void PauseOff()
//    {
//        Time.currentTimeScale = 1;
//        pause = false;
//        HideGameOver();
//    }

//    // 재시작 버튼 클릭 처리
//    public void OnClickReStart()
//    {
//        Time.currentTimeScale = 1; // 시간 재개
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재로드
//    }

//    public void Pause()
//    {
//        ShowGameOver();
//        pause = true;
//        Time.currentTimeScale = 0;
//    }

//    // GameOver 창 표시
//    public void ShowGameOver()
//    {
//        if (gameOverWindow != null)
//        {
//            gameOverWindow.SetActive(true); // 창 활성화
//        }
//        else
//        {
//            Debug.LogError("GameOverWindow가 연결되지 않았습니다!");
//        }
//    }

//    // GameOver 창 숨김
//    public void HideGameOver()
//    {
//        if (gameOverWindow != null)
//        {
//            gameOverWindow.SetActive(false); // 창 비활성화
//        }
//        else
//        {
//            Debug.LogError("GameOverWindow가 연결되지 않았습니다!");
//        }
//    }
//}


using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class UIManager : MonoBehaviour
{
    public AudioManager audioManager;

    // 기존 UI 변수
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public GameObject gameOverWindow;
    public bool pause { get; private set; } = false;

    // 게임 오버 창에서 표시할 최종 점수, 코인, 경험치 UI
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalCoinText;
    public TextMeshProUGUI finalExpText;
    public GameObject ScoreBoardWindow;

    private int displayedScore = 0;
    private int targetScore = 0;
    private int displayedCoins = 0;
    private int targetCoins = 0;
    private int displayedExp = 0;
    private int targetExp = 0;

    private float animationDuration = 1f;

    public GameObject player;

    private void Start()
    {
        targetScore = 0;
        targetCoins = 0;
        HideScoreBoardWindow();
        HideGameOver();
    }

    public void AnimateFinalStats(int finalScore, int finalCoins)
    {
        targetScore = finalScore;
        targetCoins = finalCoins;
        targetExp = finalScore / 2;

        StopAllCoroutines();
        StartCoroutine(CoAnimateStats());
    }

    private IEnumerator CoAnimateStats()
    {
        float elapsedTime = 0f;
        int initialScore = displayedScore;
        int initialCoins = displayedCoins;
        int initialExp = displayedExp;


        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            displayedScore = (int)Mathf.Lerp(initialScore, targetScore, elapsedTime / animationDuration);
            displayedCoins = (int)Mathf.Lerp(initialCoins, targetCoins, elapsedTime / animationDuration);
            displayedExp = (int)Mathf.Lerp(initialExp, targetExp, elapsedTime / animationDuration);

            UpdateFinalStats(displayedScore, displayedCoins, displayedExp);

            yield return new WaitForEndOfFrame();
        }

        UpdateFinalStats(targetScore, targetCoins, targetExp);
    }





    private void UpdateFinalStats(int score, int coins, int exp)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = score.ToString("N0");
        }
        if (finalCoinText != null)
        {
            finalCoinText.text = coins.ToString("N0");
        }
        if (finalExpText != null)
        {
            finalExpText.text = exp.ToString("N0");
        }
    }

    public void UpdateScoreText(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString();
            targetScore = newScore;
        }
    }

    public void UpdateCoinText(int newCoin)
    {
        if (coinText != null)
        {
            coinText.text = newCoin.ToString();
            targetCoins = newCoin;
        }
    }

    public void HideScoreBoardWindow()
    {
        if (ScoreBoardWindow != null) ScoreBoardWindow.SetActive(false);
    }

    public void ShowScoreBoardWindow()
    {
        if (ScoreBoardWindow != null)
        {
            ScoreBoardWindow.SetActive(true);
            audioManager.PlayerScoreBoardSound();
            AnimateFinalStats(targetScore, targetCoins);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverWindow != null)
        {

            gameOverWindow.SetActive(true);
        }
    }

    public void HideGameOver()
    {
        if (gameOverWindow != null)
        {
            gameOverWindow.SetActive(false);
        }
    }

    public void OnClickReStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeMainMenuScene()
    {
        if (player != null)
        {
            var playerState = player.GetComponent<PlayerState>();
            GameData.AddCrystal(playerState.crystal);
        }
        GameData.UpdateBestScore(targetScore);
        GameData.AddCoin(targetCoins);
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        ShowGameOver();
        pause = true;
        if (player != null)
        {
            var playerState = player.GetComponent<PlayerState>();
            playerState.StopAllAni(0f);
        }
        Time.timeScale = 0;
    }

    public void PauseOff()
    {
        if (player != null)
        {
            var playerState = player.GetComponent<PlayerState>();
            var playerMovement = player.GetComponent<PlayerMovement>();
            playerState.StopAllAni(1f);
            Time.timeScale = playerState.currentTimeScale;
        }
        pause = false;
        HideGameOver();
    }
}


