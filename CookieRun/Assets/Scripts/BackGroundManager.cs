using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public GameObject backGround;
    public Sprite[] backGroundSprites;
    private SpriteRenderer backGroundRenderer;
    public GameObject backGroundBottom;
    public Sprite[] backGroundBottomSprites;
    private SpriteRenderer backGroundBottomRenderer;

    private int currentWave;

    private bool shutDownSprite = false;
    private bool shutUpSprite = false;

    private float shutDownTime = 0f;
    private float shutUpTime = 0f;
    private float successTime = 4f;

    private void Start()
    {
        backGroundRenderer = backGround.GetComponent<SpriteRenderer>();
        backGroundBottomRenderer = backGroundBottom.GetComponent<SpriteRenderer>();
        ChangeBackGroundSprite(0);
    }

    private void Update()
    {
        if (shutDownSprite)
        {
            shutDownTime += Time.unscaledDeltaTime;
            backGroundRenderer.color = Color.Lerp(Color.white, Color.black, shutDownTime / successTime);
            backGroundBottomRenderer.color = Color.Lerp(Color.white, Color.black, shutDownTime / successTime);
            if (shutDownTime > successTime)
            {
                shutUpSprite = true;
            }
        }

        if (shutUpSprite)
        {
            ChangeBackGroundSprite(currentWave);
            shutUpTime += Time.unscaledDeltaTime;
            backGroundRenderer.color = Color.Lerp(Color.black, Color.white, shutUpTime / successTime);
            backGroundBottomRenderer.color = Color.Lerp(Color.black, Color.white, shutUpTime / successTime);
            if (shutUpTime > successTime)
            {
                shutDownTime = 0;
                shutUpTime = 0;
                shutDownSprite = false;
                shutUpSprite = false;
                backGroundRenderer.color = Color.white;
                backGroundBottomRenderer.color = Color.white;
            }
        }
    }

    public void SaveWave(int wave)
    {
        currentWave = wave;
    }

    public void ChangeBackGroundSprite(int wave)
    {
        int index = wave - 1; // wave가 1부터 시작한다고 가정

        if (index < 0 || index >= backGroundSprites.Length || index >= backGroundBottomSprites.Length)
        {
            //Debug.LogWarning("ChangeBackGroundSprite: Index out of range. Wave: " + wave);
            return;
        }

        backGroundRenderer.sprite = backGroundSprites[index];
        backGroundBottomRenderer.sprite = backGroundBottomSprites[index];
    }

    public void StartLight()
    {
        shutDownSprite = true;
    }

}
