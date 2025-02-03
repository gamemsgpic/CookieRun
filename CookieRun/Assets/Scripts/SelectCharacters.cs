using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacters : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject currentCharacter;

    private void Start()
    {
        currentCharacter = characters[0];
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
        Time.timeScale = 1f;
        characters[0].SetActive(true);
        characters[1].SetActive(false);
        characters[2].SetActive(false);
        currentCharacter = characters[0];
    }

    public void SelectBrave()
    {
        Time.timeScale = 1f;
        characters[0].SetActive(false);
        characters[1].SetActive(true);
        characters[2].SetActive(false);
        currentCharacter = characters[1];
    }

    public void SelectZombie()
    {
        Time.timeScale = 1f;
        characters[0].SetActive(false);
        characters[1].SetActive(false);
        characters[2].SetActive(true);
        currentCharacter = characters[2];
    }

    public void Jump()
    {
        currentCharacter.GetComponent<PlayerMovement>().ButtonJump();
    }

    public void ButtonDown()
    {
        currentCharacter.GetComponent<PlayerSlide>().OnSlideButtonDown();
    }

    public void ButtonUp()
    {
        currentCharacter.GetComponent<PlayerSlide>().OnSlideButtonUp();
    }
}
