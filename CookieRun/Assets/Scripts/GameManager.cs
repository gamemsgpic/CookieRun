using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("UIManager를 인스펙터에서 연결하세요.")]
    public UIManager uiManager;

    [Tooltip("PlayerState를 인스펙터에서 연결하세요.")]
    public PlayerState player;

    private void Awake()
    {
        Time.timeScale = 1; // 게임이 시작될 때 항상 정상 속도로 설정

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 GameManager 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 GameManager 제거
        }
    }

    private void Start()
    {
        if (uiManager == null)
        {
            Debug.LogWarning("UIManager가 연결되지 않았습니다. 동적으로 찾습니다.");
            FindUIManager();
        }

        if (player == null)
        {
            Debug.LogWarning("PlayerState가 연결되지 않았습니다. 동적으로 찾습니다.");
            FindPlayerState();
        }

        if (uiManager != null && player != null)
        {
            // 초기 점수 설정
            uiManager.UpdateScoreText(player.score);
        }

        if (uiManager != null && player != null)
        {
            // 초기 점수 설정
            uiManager.UpdateCoinText(player.coins);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 전환 후 UIManager와 PlayerState를 다시 참조
        FindUIManager();
        FindPlayerState();

    }

    private void FindUIManager()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager를 찾을 수 없습니다!");
            }
        }
    }

    private void FindPlayerState()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerState>();
            if (player == null)
            {
                Debug.LogError("PlayerState를 찾을 수 없습니다!");
            }
        }
    }

    public void UpdateScore(int currentScore)
    {
        if (uiManager != null)
        {
            uiManager.UpdateScoreText(currentScore);
        }
        else
        {
            Debug.LogError("UIManager가 null입니다. 점수 업데이트에 실패했습니다.");
        }
    }

    public void UpdateCoin(int currentCoin)
    {
        if (uiManager != null)
        {
            uiManager.UpdateCoinText(currentCoin);
        }
        else
        {
            Debug.LogError("UIManager가 null입니다. 점수 업데이트에 실패했습니다.");
        }
    }

    public void ShowGameOver()
    {
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
            Time.timeScale = 0f; // 게임 멈춤
        }
        else
        {
            Debug.LogError("UIManager가 null입니다. GameOver 화면을 표시할 수 없습니다.");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // 게임 재개
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재로드
    }
}
