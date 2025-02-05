using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab; // ������ ���� ������
    public Transform contentPanel; // Grid Layout Group�� �ִ� Content ������Ʈ
    public ScrollRect scrollRect; // Scroll View�� Scroll Rect
    public int totalSlots = 50; // �ʱ� ���� ����

    void Start()
    {
        GenerateEmptySlots();
    }

    public void GenerateEmptySlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentPanel);
            newSlot.name = "Slot_" + (i + 1);
        }

        // ���� ���̾ƿ� ������Ʈ (UI ũ�� �ڵ� ����)
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void AddSlots(int count)
    {
        totalSlots += count;
        GenerateEmptySlots();
    }
}
