using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("UIManager를 인스펙터에서 연결하세요.")]
    public UIManager uiManager;

    [Tooltip("PlayerState를 인스펙터에서 연결하세요.")]
    public PlayerState player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (uiManager == null)
        {
            Debug.LogError("UIManager가 연결되지 않았습니다.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("PlayerState가 연결되지 않았습니다.");
            return;
        }

        // 초기 점수 설정
        uiManager.UpdateScoreText(player.score);
    }

    public void UpdateScore(int currentScore)
    {
        if (uiManager != null)
        {
            uiManager.UpdateScoreText(currentScore);
        }
        else
        {
            Debug.LogError("UIManager가 null입니다.");
        }
    }
}
