using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorManager : MonoBehaviour
{
    public Sprite[] TutorSprites;
    private Image image;
    private int currentIndex = 0;

    private void Start()
    {
        image = GetComponent<Image>();
        currentIndex = 0;
        image.sprite = TutorSprites[currentIndex];
    }

    public void NextPage()
    {
        currentIndex++;
        if (currentIndex >= TutorSprites.Length)
        {
            currentIndex = TutorSprites.Length - 1;
        }

        image.sprite = TutorSprites[currentIndex];
    }

    public void PreviousPage()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = 0;
        }

        image.sprite = TutorSprites[currentIndex];
    }


}
