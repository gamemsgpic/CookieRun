using UnityEngine;

public class SliderSizeSet : MonoBehaviour
{
    private RectTransform parentSlider; // �θ�(HpSlider)�� RectTransform
    private RectTransform myRectTransform; // ���� ������Ʈ�� RectTransform

    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();

        if (transform.parent == null)
        {
            Debug.LogError($"{gameObject.name}�� �θ� �����ϴ�.");
            return;
        }

        parentSlider = transform.parent.GetComponent<RectTransform>();

        if (parentSlider == null)
        {
            Debug.LogError($"{gameObject.name}�� �θ� RectTransform�� �����ϴ�.");
            return;
        }

        // �θ�(HpSlider)�� ũ�� ��������
        Vector2 sliderSize = parentSlider.sizeDelta;

        // ����(Fill Area �Ǵ� Handle Slide Area)�� ũ�� ����
        myRectTransform.sizeDelta = new Vector2(sliderSize.x, myRectTransform.sizeDelta.y);
    }
}
