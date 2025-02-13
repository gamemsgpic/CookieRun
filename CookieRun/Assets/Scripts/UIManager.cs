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
//        HideGameOver(); // ���� ���� �� GameOver â ��Ȱ��ȭ
//    }

//    // ���� �ؽ�Ʈ ������Ʈ
//    public void UpdateScoreText(int newScore)
//    {
//        if (scoreText != null)
//        {
//            scoreText.text = newScore.ToString();
//        }
//        else
//        {
//            Debug.LogError("ScoreText�� ������� �ʾҽ��ϴ�!");
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
//            Debug.LogError("ScoreText�� ������� �ʾҽ��ϴ�!");
//        }

//    }

//    // �Ͻ� ���� ����
//    public void PauseOff()
//    {
//        Time.currentTimeScale = 1;
//        pause = false;
//        HideGameOver();
//    }

//    // ����� ��ư Ŭ�� ó��
//    public void OnClickReStart()
//    {
//        Time.currentTimeScale = 1; // �ð� �簳
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� ��ε�
//    }

//    public void Pause()
//    {
//        ShowGameOver();
//        pause = true;
//        Time.currentTimeScale = 0;
//    }

//    // GameOver â ǥ��
//    public void ShowGameOver()
//    {
//        if (gameOverWindow != null)
//        {
//            gameOverWindow.SetActive(true); // â Ȱ��ȭ
//        }
//        else
//        {
//            Debug.LogError("GameOverWindow�� ������� �ʾҽ��ϴ�!");
//        }
//    }

//    // GameOver â ����
//    public void HideGameOver()
//    {
//        if (gameOverWindow != null)
//        {
//            gameOverWindow.SetActive(false); // â ��Ȱ��ȭ
//        }
//        else
//        {
//            Debug.LogError("GameOverWindow�� ������� �ʾҽ��ϴ�!");
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

    // ���� UI ����
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public GameObject gameOverWindow;
    public bool pause { get; private set; } = false;

    // ���� ���� â���� ǥ���� ���� ����, ����, ����ġ UI
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


