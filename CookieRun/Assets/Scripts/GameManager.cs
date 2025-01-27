using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("UIManager�� �ν����Ϳ��� �����ϼ���.")]
    public UIManager uiManager;

    [Tooltip("PlayerState�� �ν����Ϳ��� �����ϼ���.")]
    public PlayerState player;

    private void Awake()
    {
        Time.timeScale = 1; // ������ ���۵� �� �׻� ���� �ӵ��� ����

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� GameManager ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� GameManager ����
        }
    }

    private void Start()
    {
        if (uiManager == null)
        {
            Debug.LogWarning("UIManager�� ������� �ʾҽ��ϴ�. �������� ã���ϴ�.");
            FindUIManager();
        }

        if (player == null)
        {
            Debug.LogWarning("PlayerState�� ������� �ʾҽ��ϴ�. �������� ã���ϴ�.");
            FindPlayerState();
        }

        if (uiManager != null && player != null)
        {
            // �ʱ� ���� ����
            uiManager.UpdateScoreText(player.score);
        }

        if (uiManager != null && player != null)
        {
            // �ʱ� ���� ����
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
        // �� ��ȯ �� UIManager�� PlayerState�� �ٽ� ����
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
                Debug.LogError("UIManager�� ã�� �� �����ϴ�!");
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
                Debug.LogError("PlayerState�� ã�� �� �����ϴ�!");
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
            Debug.LogError("UIManager�� null�Դϴ�. ���� ������Ʈ�� �����߽��ϴ�.");
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
            Debug.LogError("UIManager�� null�Դϴ�. ���� ������Ʈ�� �����߽��ϴ�.");
        }
    }

    public void ShowGameOver()
    {
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
            Time.timeScale = 0f; // ���� ����
        }
        else
        {
            Debug.LogError("UIManager�� null�Դϴ�. GameOver ȭ���� ǥ���� �� �����ϴ�.");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // ���� �簳
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� ��ε�
    }
}
