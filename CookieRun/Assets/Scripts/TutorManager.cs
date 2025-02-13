using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorManager : MonoBehaviour
{
    public Sprite[] TutorSprites;
    public TextMeshProUGUI nextButtonText;
    public TextMeshProUGUI priviousButtonText;
    private Image image;
    private int currentIndex = 0;

    private void Start()
    {
        image = GetComponent<Image>();
        currentIndex = 0;
        image.sprite = TutorSprites[currentIndex];
    }

    private void OnEnable()
    {
        currentIndex = 0;
        priviousButtonText.color = Color.clear;
    }

    public void NextPage()
    {
        currentIndex++;
        priviousButtonText.color = Color.white;
        if (currentIndex == TutorSprites.Length - 1)
        {
            nextButtonText.color = Color.clear;
        }
        else if (currentIndex < TutorSprites.Length - 1)
        {
            nextButtonText.color = Color.white;
        }
        else if (currentIndex >= TutorSprites.Length)
        {
            currentIndex = TutorSprites.Length - 1;
        }

        image.sprite = TutorSprites[currentIndex];
    }

    public void PreviousPage()
    {
        currentIndex--;
        nextButtonText.color = Color.white;
        if (currentIndex == 0)
        {
            priviousButtonText.color = Color.clear;
        }
        else if (currentIndex > 0)
        {
            priviousButtonText.color = Color.white;
        }
        else if (currentIndex < 0)
        {
            currentIndex = 0;
        }

        image.sprite = TutorSprites[currentIndex];
    }


}
