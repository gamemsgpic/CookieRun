using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab; // 아이템 슬롯 프리팹
    public Transform contentPanel; // Grid Layout Group이 있는 Content 오브젝트
    public ScrollRect scrollRect; // Scroll View의 Scroll Rect
    public int totalSlots = 50; // 초기 슬롯 개수

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

        // 강제 레이아웃 업데이트 (UI 크기 자동 조정)
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void AddSlots(int count)
    {
        totalSlots += count;
        GenerateEmptySlots();
    }
}
