using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Inventory.SetActive(false);
        GameData.LoadGameData(); // 게임 시작 시 데이터 불러오기
        SceneManager.sceneUnloaded += OnSceneChanged; // 씬 변경 시 자동 저장
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
        Debug.Log("호출됨");
    }

    private void OnApplicationQuit()
    {
        GameData.SaveGameData(); // 게임 종료 시 데이터 저장
    }

    public void OnExitButtonClick()
    {
        Debug.Log("[UIManager] 게임 종료 버튼 클릭됨, 데이터 저장 후 종료.");
        GameData.SaveGameData(); // 게임 데이터 저장
        Application.Quit(); // 게임 종료 (빌드된 상태에서만 동작)
    }

    private void OnSceneChanged(Scene scene)
    {
        Debug.Log($"[UIManager] 씬 변경 감지 ({scene.name}) → 데이터 저장");
        GameData.SaveGameData();
    }

    public void OnSaveButtonClick()
    {
        Debug.Log("[UIManager] 수동 저장 버튼 클릭됨");
        GameData.SaveGameData();
    }
}
