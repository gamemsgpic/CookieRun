using UnityEngine;

public class SliderSizeSet : MonoBehaviour
{
    private RectTransform parentSlider; // 부모(HpSlider)의 RectTransform
    private RectTransform myRectTransform; // 현재 오브젝트의 RectTransform

    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();

        if (transform.parent == null)
        {
            Debug.LogError($"{gameObject.name}의 부모가 없습니다.");
            return;
        }

        parentSlider = transform.parent.GetComponent<RectTransform>();

        if (parentSlider == null)
        {
            Debug.LogError($"{gameObject.name}의 부모에 RectTransform이 없습니다.");
            return;
        }

        // 부모(HpSlider)의 크기 가져오기
        Vector2 sliderSize = parentSlider.sizeDelta;

        // 본인(Fill Area 또는 Handle Slide Area)의 크기 조정
        myRectTransform.sizeDelta = new Vector2(sliderSize.x, myRectTransform.sizeDelta.y);
    }
}
