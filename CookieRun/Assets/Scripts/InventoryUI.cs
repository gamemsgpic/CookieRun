using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform inventoryGrid; // �������� ��ġ�� �θ� (Grid Layout Group�� ����� ������Ʈ)
    public GameObject treasureItemPrefab; // ������ ������
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>(); // ���� ���� 3��

    private EquipmentSlot selectedSlot; // ���� ���õ� ����
    private TreasureItem selectedItem;
    private Dictionary<int, TreasureItem> uniqueTreasures = new Dictionary<int, TreasureItem>();

    private void Start()
    {
        GenerateInitialTreasures(); // CSV �����͸� ������� ������ ����
    }

    // CSV �����͸� ������� �������� �����ϴ� �޼���
    private void GenerateInitialTreasures()
    {
        var records = CsvManager.Load<TreasureTable>("TreasureTable");
        if (records == null)
        {
            Debug.LogError("[InventoryUI] CSV �����͸� �ҷ����� ���߽��ϴ�.");
            return;
        }

        foreach (var data in records)
        {
            AddTreasureToInventory(data);
        }
    }

    // �������� �κ��丮�� �߰��ϴ� �޼���
    public void AddTreasureToInventory(TreasureTable data)
    {
        if (uniqueTreasures.ContainsKey(data.Id)) return;

        if (treasureItemPrefab == null || inventoryGrid == null)
        {
            Debug.LogError("[InventoryUI] treasureItemPrefab �Ǵ� inventoryGrid�� �������� ����!");
            return;
        }

        GameObject newItem = Instantiate(treasureItemPrefab, inventoryGrid);

        // TreasureItem�� �ڽ� ������Ʈ�� �ִ� ��츦 ����Ͽ� GetComponentInChildren ���
        TreasureItem treasureItem = newItem.GetComponentInChildren<TreasureItem>();
        if (treasureItem == null)
        {
            Debug.LogError("[InventoryUI] TreasureItem �����տ��� TreasureItem ������Ʈ�� ã�� �� ����!");
            return;
        }

        // Treasure ������Ʈ ã��
        Treasure treasure = newItem.GetComponentInChildren<Treasure>();
        if (treasure == null)
        {
            Debug.LogError("[InventoryUI] TreasureItem �����տ��� Treasure ������Ʈ�� ã�� �� ����!");
            return;
        }

        // Treasure ������ ����
        treasure.treasureID = data.Id;
        treasure.treasureName = data.Name;
        treasure.treasureVelue = data.Velue;
        treasure.treasureRadius = data.Radius;
        treasure.treasureSpeed = data.Speed;
        treasure.treasurePath = data.Path;

        // ������ ���� (Resources �������� ������)
        treasure.treasureIcon = Resources.Load<Sprite>(data.Path);
        if (treasure.treasureIcon == null)
        {
            Debug.LogError($"[InventoryUI] ��������Ʈ�� �ҷ��� �� ����: {data.Path}");
        }

        // TreasureItem ����
        treasureItem.Setup(treasure, this);

        uniqueTreasures.Add(data.Id, treasureItem);
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryGrid.GetComponent<RectTransform>());

        Debug.Log($"[InventoryUI] Treasure �߰� �Ϸ�: {treasure.treasureName} (ID: {treasure.treasureID})");
    }

    // �������� �����ϴ� �޼���
    public void SelectItem(TreasureItem item)
    {
        if (selectedItem != null)
        {
            selectedItem.SelectItemEffect(false); // ���� ���� ����
        }

        selectedItem = item;
        selectedItem.SelectItemEffect(true); // �� ������ ����
        Debug.Log($"[InventoryUI] {selectedItem.treasureData.treasureName} ���õ�");
    }

    // �������� �����ϴ� �޼���
    public void EquipSelectedItem()
    {
        if (selectedSlot == null)
        {
            Debug.LogError("[InventoryUI] ���õ� ������ �����ϴ�! ���� ������ Ŭ���ϼ���.");
            return;
        }

        if (selectedItem == null)
        {
            Debug.LogError("[InventoryUI] ���õ� �������� �����ϴ�!");
            return;
        }

        // ������ ������ �ڽ� EquipmentTreasure�� ������ ����
        selectedSlot.equipmentTreasure.ApplyTreasureData(selectedItem.treasureData);

        Debug.Log($"[InventoryUI] {selectedItem.treasureData.treasureName} ������");
    }

    public void OpenInventoryForSlot(EquipmentSlot slot)
    {
        if (slot == null)
        {
            Debug.LogError("[InventoryUI] ������ ������ �����ϴ�!");
            return;
        }

        selectedSlot = slot; // ������ ������ ����
        Debug.Log($"[InventoryUI] ���� {selectedSlot.slotIndex} ���õ�");
        gameObject.SetActive(true); // �κ��丮 UI Ȱ��ȭ
    }

    // �������� �����ϴ� �޼���
    public void UnequipSelectedSlot()
    {
        if (selectedSlot == null)
        {
            Debug.LogError("[InventoryUI] ������ ������ ���õ��� �ʾҽ��ϴ�!");
            return;
        }

        selectedSlot.UnequipTreasure();
        Debug.Log("[InventoryUI] ���õ� ������ ���������� ������.");
    }

    public void ResetSelectedSlot()
    {
        selectedSlot = null; // �κ��丮�� ���� ���� ������ �ʱ�ȭ
        Debug.Log("[InventoryUI] �κ��丮 ����, selectedSlot �ʱ�ȭ��.");
    }
}
