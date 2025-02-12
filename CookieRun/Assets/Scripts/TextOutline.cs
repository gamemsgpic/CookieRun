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

        // �ƿ����� �ʺ� ����
        tmpText.fontMaterial.SetFloat("_OutlineWidth", 0.3f);

        // �ƿ����� ���� ���� (����)
        tmpText.fontMaterial.SetColor("_OutlineColor", Color.black);
    }
}
