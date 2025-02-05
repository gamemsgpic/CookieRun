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

            // 화면 비율 계산
            float screenRatio = (float)Screen.width / Screen.height;
            float referenceRatio = referenceWidth / referenceHeight;

            // 현재 화면 비율과 기준 비율 비교 후 스케일 조정
            float scaleFactor = screenRatio / referenceRatio;

            canvasRect.sizeDelta = new Vector2(referenceWidth, referenceHeight);
            canvasRect.localScale = Vector3.one * scaleFactor * 0.01f; // 적절한 크기로 조정
        }
    }
}
