using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static readonly string scoreformat = "SCORE: {0}";

    [Tooltip("������ ǥ���� TextMeshProUGUI�� �����ϼ���.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("GameOver â�� �����ϼ���.")]
    public GameObject gameOverWindow;

    private void Start()
    {
        HideGameOver(); // ���� ���� �� GameOver â ��Ȱ��ȭ
    }

    // ���� �ؽ�Ʈ ������Ʈ
    public void UpdateScoreText(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format(scoreformat, newScore);
        }
        else
        {
            Debug.LogError("ScoreText�� ������� �ʾҽ��ϴ�!");
        }
    }

    // �Ͻ� ���� ����
    public void PauseOff()
    {
        Time.timeScale = 1;
        HideGameOver();
    }

    // ����� ��ư Ŭ�� ó��
    public void OnClickReStart()
    {
        Time.timeScale = 1; // �ð� �簳
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� ��ε�
    }

    // GameOver â ǥ��
    public void ShowGameOver()
    {
        if (gameOverWindow != null)
        {
            gameOverWindow.SetActive(true); // â Ȱ��ȭ
        }
        else
        {
            Debug.LogError("GameOverWindow�� ������� �ʾҽ��ϴ�!");
        }
    }

    // GameOver â ����
    public void HideGameOver()
    {
        if (gameOverWindow != null)
        {
            gameOverWindow.SetActive(false); // â ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("GameOverWindow�� ������� �ʾҽ��ϴ�!");
        }
    }
}
