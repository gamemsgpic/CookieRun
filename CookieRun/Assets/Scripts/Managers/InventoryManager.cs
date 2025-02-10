using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameData;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab; // ������ ���� ������
    public Transform contentPanel; // Grid Layout Group�� �ִ� Content ������Ʈ
    public ScrollRect scrollRect; // Scroll View�� Scroll Rect
    public int totalSlots = 50; // �ʱ� ���� ����
    public static List<Treasure> allTreasures = new List<Treasure>();

    private EquipmentSlot selectedSlot; // ���õ� ��� ����
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    void Start()
    {
        GenerateEmptySlots();
    }

    private void Awake()
    {
        LoadTreasuresFromCSV(); // ���� ���� �� CSV���� ������ �ε�

        if (allTreasures.Count == 0)
        {
            Debug.LogError("[InventoryManager] Treasure �����͸� �ҷ����� ���߽��ϴ�.");
        }
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

    private void LoadTreasuresFromCSV()
    {
        allTreasures = CsvManager.Load<Treasure>("TreasureTable").ToList(); // `IEnumerable<T>` �� `List<T>` ��ȯ

        if (allTreasures == null || allTreasures.Count == 0)
        {
            Debug.LogError("[InventoryManager] Treasure �����͸� �ҷ����� ���߽��ϴ�.");
        }
        else
        {
            Debug.Log($"[InventoryManager] {allTreasures.Count}���� Treasure ������ �ε� �Ϸ�.");
        }
    }

    public static Treasure GetTreasureById(string treasureId)
    {
        if (allTreasures == null || allTreasures.Count == 0)
        {
            Debug.LogError("[InventoryManager] Treasure �����Ͱ� ���� �ε���� �ʾҽ��ϴ�!");
            return null;
        }

        Treasure foundTreasure = allTreasures.Find(t => t.treasureID.ToString() == treasureId);
        if (foundTreasure == null)
        {
            Debug.LogWarning($"[InventoryManager] ID {treasureId}�� �ش��ϴ� Treasure�� ã�� �� �����ϴ�.");
        }
        return foundTreasure;
    }


    public void SetSelectedSlot(EquipmentSlot slot)
    {
        selectedSlot = slot;
        Debug.Log($"[InventoryManager] ���õ� ����: {slot.name}");
    }

    public EquipmentSlot GetSelectedSlot()
    {
        return selectedSlot;
    }

    public void ClearSelectedSlot()
    {
        selectedSlot = null;
        Debug.Log("[InventoryManager] ���õ� ���� �ʱ�ȭ��.");
    }

    public void SaveEquipmentSlotsToGameData()
    {
        if (equipmentSlots == null || equipmentSlots.Count == 0)
        {
            Debug.LogError("[InventoryManager] ������ ������ �����ϴ�!");
            return;
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            Treasure equippedItem = equipmentSlots[i].GetEquippedItem();
            EquipmentSlotData slotData = (equippedItem != null)
                ? new EquipmentSlotData(
                    equippedItem.treasureID,
                    equippedItem.treasureName,
                    equippedItem.treasureVelue,
                    equippedItem.treasureRadius,
                    equippedItem.treasureSpeed,
                    equippedItem.treasurePath)
                : new EquipmentSlotData(0, "", 0, 0f, 0f, "");

            GameData.SetEquipmentSlot(i, slotData);
        }

        Debug.Log("[InventoryManager] ���� ���� ������ GameData�� ���� �Ϸ�.");
    }

    public void ApplyLoadedEquipmentSlots()
    {
        if (equipmentSlots == null || equipmentSlots.Count == 0)
        {
            Debug.LogError("[InventoryManager] ��� ������ �������� ����!");
            return;
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            EquipmentSlotData loadedItemData = GameData.GetEquipmentSlot(i);
            if (!string.IsNullOrEmpty(loadedItemData.treasureName))
            {
                Treasure treasure = GetTreasureById(loadedItemData.treasureID.ToString());
                if (treasure != null)
                {
                    equipmentSlots[i].EquipTreasure(treasure);
                    Debug.Log($"[InventoryManager] ���� {i}�� {treasure.treasureName} �ݿ� �Ϸ�");
                }
                else
                {
                    Debug.LogWarning($"[InventoryManager] ID {loadedItemData.treasureID}�� �ش��ϴ� Treasure�� ã�� �� ����");
                }
            }
        }
    }

}
