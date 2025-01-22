using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public static readonly string scoreformat = "SCORE: {0}";

    public TextMeshProUGUI scoreText;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = string.Format(scoreformat, newScore);
    }
}
