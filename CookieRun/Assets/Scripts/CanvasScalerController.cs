using UnityEngine;

public class CanvasScalerController : MonoBehaviour
{
    public Canvas canvas;
    public float referenceWidth = 1920f;
    public float referenceHeight = 1080f;

    void Start()
    {
        AdjustCanvasScale();
    }

    void AdjustCanvasScale()
    {
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            // ȭ�� ���� ���
            float screenRatio = (float)Screen.width / Screen.height;
            float referenceRatio = referenceWidth / referenceHeight;

            // ���� ȭ�� ������ ���� ���� �� �� ������ ����
            float scaleFactor = screenRatio / referenceRatio;

            canvasRect.sizeDelta = new Vector2(referenceWidth, referenceHeight);
            canvasRect.localScale = Vector3.one * scaleFactor * 0.01f; // ������ ũ��� ����
        }
    }
}
