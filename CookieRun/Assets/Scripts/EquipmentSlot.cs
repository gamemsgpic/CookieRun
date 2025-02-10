using UnityEngine;
using static GameData;

public class EquipmentSlot : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public EquipmentTreasure equipmentTreasure; // UI + 데이터 보관하는 오브젝트
    public int slotIndex;

    private void Start()
    {
        if (equipmentTreasure == null)
        {
            Debug.LogError($"[EquipmentSlot] {gameObject.name}의 EquipmentTreasure가 설정되지 않았습니다!");
        }

        LoadEquipmentFromGameData(); // GameData에서 데이터 로드
    }

    /// **GameData에서 데이터를 불러와 EquipmentTreasure에 적용**
    private void LoadEquipmentFromGameData()
    {
        EquipmentSlotData equippedData = GameData.GetEquipmentSlot(slotIndex);

        if (!string.IsNullOrEmpty(equippedData.treasureName))
        {
            Treasure treasure = InventoryManager.GetTreasureById(equippedData.treasureID.ToString());
            if (treasure != null)
            {
                equipmentTreasure.ApplyTreasureData(treasure);
            }
            else
            {
                Debug.LogWarning($"[EquipmentSlot] 슬롯 {slotIndex}의 아이템 {equippedData.treasureID}을 찾을 수 없음");
            }
        }
    }

    /// **새로운 아이템을 장착**
    public void EquipTreasure(Treasure newTreasure)
    {
        if (newTreasure == null) return;

        equipmentTreasure.ApplyTreasureData(newTreasure);

        GameData.SetEquipmentSlot(slotIndex, new EquipmentSlotData(
            newTreasure.treasureID,
            newTreasure.treasureName,
            newTreasure.treasureVelue,
            newTreasure.treasureRadius,
            newTreasure.treasureSpeed,
            newTreasure.treasurePath
        ));

        GameData.SaveGameData();
    }

    /// **장착 해제 (빈 슬롯으로 초기화)**
    public void UnequipTreasure()
    {
        equipmentTreasure.ClearTreasure();

        // GameData에서도 제거
        GameData.SetEquipmentSlot(slotIndex, new EquipmentSlotData());
        GameData.SaveGameData();
    }

    /// **현재 장착된 아이템 반환**
    public Treasure GetEquippedItem()
    {
        return equipmentTreasure.treasureData;
    }

    public void OnSlotClicked()
    {
        inventoryUI.OpenInventoryForSlot(this);
    }
}
