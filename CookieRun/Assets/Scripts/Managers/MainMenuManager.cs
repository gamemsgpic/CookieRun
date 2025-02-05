using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Inventory.SetActive(false);
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
    }
}
