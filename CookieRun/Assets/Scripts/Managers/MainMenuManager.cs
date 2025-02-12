using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject tutorWindow;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI crystalText;
    private int bestScore;
    private int coin;
    private int crystal;

    private void Start()
    {
        bestScore = GameData.bestScore;
        bestScoreText.text = bestScore.ToString();
        coin = GameData.coin;
        coinText.text = coin.ToString();
        crystal = GameData.crystal;
        crystalText.text = crystal.ToString();
        Inventory.SetActive(false);
        GameData.LoadGameData(); // ���� ���� �� ������ �ҷ�����
        SceneManager.sceneUnloaded += OnSceneChanged; // �� ���� �� �ڵ� ����
    }

    public GameObject Inventory;

    public void ChangeGameScene()
    {
        SceneManager.LoadScene(1);
    }
    
    public void OnInventory()
    {
        Inventory.SetActive(true);
    }

    public void OffInventory()
    {
        Inventory.SetActive(false);
        Debug.Log("ȣ���");
    }

    private void OnApplicationQuit()
    {
        GameData.SaveGameData(); // ���� ���� �� ������ ����
    }

    public void OnExitButtonClick()
    {
        Debug.Log("[UIManager] ���� ���� ��ư Ŭ����, ������ ���� �� ����.");
        GameData.SaveGameData(); // ���� ������ ����
        Application.Quit(); // ���� ���� (����� ���¿����� ����)
    }

    private void OnSceneChanged(Scene scene)
    {
        Debug.Log($"[UIManager] �� ���� ���� ({scene.name}) �� ������ ����");
        GameData.SaveGameData();
    }

    public void OnSaveButtonClick()
    {
        Debug.Log("[UIManager] ���� ���� ��ư Ŭ����");
        GameData.SaveGameData();
    }

    public void OnResetSave()
    {
        //GameData.ResetSaveGameData();
    }

    public void UpdateCoinText()
    {
        coin = GameData.coin;
        coinText.text = coin.ToString();
    }

    public void OnTutorWindow()
    {
        tutorWindow.SetActive(true);
    }

    public void OffTutorWindow()
    {
        tutorWindow.SetActive(false);
    }
}
