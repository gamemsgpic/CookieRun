using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacters : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject currentCharacter;
    public UIManager uiManager;

    private void Start()
    {
        if (GameData.characterName == "BraveCookie")
        {
            SelectBrave();
        }
        else if (GameData.characterName == "AngelCookie")
        {
            SelectAngel();
        }
        else if (GameData.characterName == "ZombieCookie")
        {
            SelectZombie();
        }

        SetCurrentPlayer();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAngel();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectBrave();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectZombie();
        }
    }

    public void SelectAngel()
    {
        if (!uiManager.pause)
        {
            Time.timeScale = 1f;
            characters[0].SetActive(true);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            currentCharacter = characters[0];
            SetCurrentPlayer();
        }
    }

    public void SelectBrave()
    {
        if (!uiManager.pause)
        {
            Time.timeScale = 1f;
            characters[0].SetActive(false);
            characters[1].SetActive(true);
            characters[2].SetActive(false);
            currentCharacter = characters[1];
            SetCurrentPlayer();
        }
    }

    public void SelectZombie()
    {
        if (!uiManager.pause)
        {
            Time.timeScale = 1f;
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(true);
            currentCharacter = characters[2];
            SetCurrentPlayer();
        }
    }

    public void Jump()
    {
        currentCharacter.GetComponent<PlayerMovement>().ButtonJump();
        currentCharacter.GetComponent<PlayerMovement>().SetJumpKeyHeld(true);
    }

    public void UpJumpButton(bool isHeld)
    {
        currentCharacter.GetComponent<PlayerMovement>().SetJumpKeyHeld(false);

    }

    public void ButtonDown()
    {
        currentCharacter.GetComponent<PlayerSlide>().OnSlideButtonDown();
    }

    public void ButtonUp()
    {
        currentCharacter.GetComponent<PlayerSlide>().OnSlideButtonUp();
    }

    private void SetCurrentPlayer()
    {
        uiManager.player = currentCharacter;
    }
}
