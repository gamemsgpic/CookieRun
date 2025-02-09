using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab; // 아이템 슬롯 프리팹
    public Transform contentPanel; // Grid Layout Group이 있는 Content 오브젝트
    public ScrollRect scrollRect; // Scroll View의 Scroll Rect
    public int totalSlots = 50; // 초기 슬롯 개수
    public static List<Treasure> allTreasures = new List<Treasure>();

    private EquipmentSlot selectedSlot; // 선택된 장비 슬롯
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    void Start()
    {
        GenerateEmptySlots();
    }

    private void Awake()
    {
        LoadTreasuresFromCSV(); // 게임 시작 시 CSV에서 데이터 로드

        if (allTreasures.Count == 0)
        {
            Debug.LogError("[InventoryManager] Treasure 데이터를 불러오지 못했습니다.");
        }
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

    private void LoadTreasuresFromCSV()
    {
        allTreasures = CsvManager.Load<Treasure>("TreasureTable").ToList(); // `IEnumerable<T>` → `List<T>` 변환

        if (allTreasures == null || allTreasures.Count == 0)
        {
            Debug.LogError("[InventoryManager] Treasure 데이터를 불러오지 못했습니다.");
        }
        else
        {
            Debug.Log($"[InventoryManager] {allTreasures.Count}개의 Treasure 데이터 로드 완료.");
        }
    }

    public static Treasure GetTreasureById(string treasureId)
    {
        if (allTreasures == null || allTreasures.Count == 0)
        {
            Debug.LogError("[InventoryManager] Treasure 데이터가 아직 로드되지 않았습니다!");
            return null;
        }

        Treasure foundTreasure = allTreasures.Find(t => t.treasureID.ToString() == treasureId);
        if (foundTreasure == null)
        {
            Debug.LogWarning($"[InventoryManager] ID {treasureId}에 해당하는 Treasure를 찾을 수 없습니다.");
        }
        return foundTreasure;
    }


    public void SetSelectedSlot(EquipmentSlot slot)
    {
        selectedSlot = slot;
        Debug.Log($"[InventoryManager] 선택된 슬롯: {slot.name}");
    }

    public EquipmentSlot GetSelectedSlot()
    {
        return selectedSlot;
    }

    public void ClearSelectedSlot()
    {
        selectedSlot = null;
        Debug.Log("[InventoryManager] 선택된 슬롯 초기화됨.");
    }

    public void SaveEquipmentSlotsToGameData()
    {
        if (equipmentSlots == null || equipmentSlots.Count == 0)
        {
            Debug.LogError("[InventoryManager] 저장할 슬롯이 없습니다!");
            return;
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            Treasure equippedItem = equipmentSlots[i].GetEquippedItem();
            string itemId = (equippedItem != null) ? equippedItem.treasureID.ToString() : "None";
            GameData.SetEquipmentSlot(i, itemId);
        }

        Debug.Log("[InventoryManager] 현재 슬롯 정보를 GameData에 저장 완료.");
    }

    public void ApplyLoadedEquipmentSlots()
    {
        if (equipmentSlots == null || equipmentSlots.Count == 0)
        {
            Debug.LogError("[InventoryManager] 장비 슬롯이 설정되지 않음!");
            return;
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            string loadedItemId = GameData.GetEquipmentSlot(i);
            if (!string.IsNullOrEmpty(loadedItemId) && loadedItemId != "None")
            {
                Treasure treasure = GetTreasureById(loadedItemId);
                if (treasure != null)
                {
                    // 슬롯의 자식 오브젝트(EquipmentTreasure)에도 반영
                    equipmentSlots[i].ReceiveTreasureData(treasure);
                    Debug.Log($"[InventoryManager] 슬롯 {i}에 {treasure.treasureName} 반영 완료");
                }
                else
                {
                    Debug.LogWarning($"[InventoryManager] ID {loadedItemId}에 해당하는 Treasure를 찾을 수 없음");
                }
            }
        }
    }

}
