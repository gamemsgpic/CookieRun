using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform inventoryGrid; // 아이템이 배치될 부모 (Grid Layout Group이 적용된 오브젝트)
    public GameObject treasureItemPrefab; // 아이템 프리팹
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>(); // 장착 슬롯 3개

    private EquipmentSlot selectedSlot; // 현재 선택된 슬롯
    private TreasureItem selectedItem;
    private Dictionary<int, TreasureItem> uniqueTreasures = new Dictionary<int, TreasureItem>();

    private void Start()
    {
        GenerateInitialTreasures(); // CSV 데이터를 기반으로 아이템 생성
    }

    // CSV 데이터를 기반으로 아이템을 생성하는 메서드
    private void GenerateInitialTreasures()
    {
        var records = CsvManager.Load<TreasureTable>("TreasureTable");
        if (records == null)
        {
            Debug.LogError("[InventoryUI] CSV 데이터를 불러오지 못했습니다.");
            return;
        }

        foreach (var data in records)
        {
            AddTreasureToInventory(data);
        }
    }

    // 아이템을 인벤토리에 추가하는 메서드
    public void AddTreasureToInventory(TreasureTable data)
    {
        if (uniqueTreasures.ContainsKey(data.Id)) return;

        if (treasureItemPrefab == null || inventoryGrid == null)
        {
            Debug.LogError("[InventoryUI] treasureItemPrefab 또는 inventoryGrid가 설정되지 않음!");
            return;
        }

        GameObject newItem = Instantiate(treasureItemPrefab, inventoryGrid);

        // TreasureItem이 자식 오브젝트에 있는 경우를 고려하여 GetComponentInChildren 사용
        TreasureItem treasureItem = newItem.GetComponentInChildren<TreasureItem>();
        if (treasureItem == null)
        {
            Debug.LogError("[InventoryUI] TreasureItem 프리팹에서 TreasureItem 컴포넌트를 찾을 수 없음!");
            return;
        }

        // Treasure 컴포넌트 찾기
        Treasure treasure = newItem.GetComponentInChildren<Treasure>();
        if (treasure == null)
        {
            Debug.LogError("[InventoryUI] TreasureItem 프리팹에서 Treasure 컴포넌트를 찾을 수 없음!");
            return;
        }

        // Treasure 데이터 적용
        treasure.treasureID = data.Id;
        treasure.treasureName = data.Name;
        treasure.treasureVelue = data.Velue;
        treasure.treasureRadius = data.Radius;
        treasure.treasureSpeed = data.Speed;
        treasure.treasurePath = data.Path;

        // 아이콘 설정 (Resources 폴더에서 가져옴)
        treasure.treasureIcon = Resources.Load<Sprite>(data.Path);
        if (treasure.treasureIcon == null)
        {
            Debug.LogError($"[InventoryUI] 스프라이트를 불러올 수 없음: {data.Path}");
        }

        // TreasureItem 설정
        treasureItem.Setup(treasure, this);

        uniqueTreasures.Add(data.Id, treasureItem);
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryGrid.GetComponent<RectTransform>());

        Debug.Log($"[InventoryUI] Treasure 추가 완료: {treasure.treasureName} (ID: {treasure.treasureID})");
    }

    // 아이템을 선택하는 메서드
    public void SelectItem(TreasureItem item)
    {
        if (selectedItem != null)
        {
            selectedItem.SelectItemEffect(false); // 기존 선택 해제
        }

        selectedItem = item;
        selectedItem.SelectItemEffect(true); // 새 아이템 선택
        Debug.Log($"[InventoryUI] {selectedItem.treasureData.treasureName} 선택됨");
    }

    // 아이템을 장착하는 메서드
    public void EquipSelectedItem()
    {
        if (selectedSlot == null)
        {
            Debug.LogError("[InventoryUI] 선택된 슬롯이 없습니다! 먼저 슬롯을 클릭하세요.");
            return;
        }

        if (selectedItem == null)
        {
            Debug.LogError("[InventoryUI] 선택된 아이템이 없습니다!");
            return;
        }

        // 선택한 슬롯의 자식 EquipmentTreasure에 데이터 적용
        selectedSlot.equipmentTreasure.ApplyTreasureData(selectedItem.treasureData);

        Debug.Log($"[InventoryUI] {selectedItem.treasureData.treasureName} 장착됨");
    }

    public void OpenInventoryForSlot(EquipmentSlot slot)
    {
        if (slot == null)
        {
            Debug.LogError("[InventoryUI] 선택한 슬롯이 없습니다!");
            return;
        }

        selectedSlot = slot; // 선택한 슬롯을 저장
        Debug.Log($"[InventoryUI] 슬롯 {selectedSlot.slotIndex} 선택됨");
        gameObject.SetActive(true); // 인벤토리 UI 활성화
    }

    // 아이템을 해제하는 메서드
    public void UnequipSelectedSlot()
    {
        if (selectedSlot == null)
        {
            Debug.LogError("[InventoryUI] 해제할 슬롯이 선택되지 않았습니다!");
            return;
        }

        selectedSlot.UnequipTreasure();
        Debug.Log("[InventoryUI] 선택된 슬롯이 정상적으로 해제됨.");
    }

    public void ResetSelectedSlot()
    {
        selectedSlot = null; // 인벤토리를 닫을 때만 슬롯을 초기화
        Debug.Log("[InventoryUI] 인벤토리 닫힘, selectedSlot 초기화됨.");
    }
}
