using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextOutline : MonoBehaviour
{
    public TextMeshProUGUI tmpText;

    void Start()
    {
        if (tmpText == null) tmpText = GetComponent<TextMeshProUGUI>();

        // 아웃라인 너비 설정
        tmpText.fontMaterial.SetFloat("_OutlineWidth", 0.3f);

        // 아웃라인 색상 설정 (검정)
        tmpText.fontMaterial.SetColor("_OutlineColor", Color.black);
    }
}
