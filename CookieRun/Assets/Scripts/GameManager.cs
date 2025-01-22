using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("UIManager�� �ν����Ϳ��� �����ϼ���.")]
    public UIManager uiManager;

    [Tooltip("PlayerState�� �ν����Ϳ��� �����ϼ���.")]
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
            Debug.LogError("UIManager�� ������� �ʾҽ��ϴ�.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("PlayerState�� ������� �ʾҽ��ϴ�.");
            return;
        }

        // �ʱ� ���� ����
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
            Debug.LogError("UIManager�� null�Դϴ�.");
        }
    }
}
