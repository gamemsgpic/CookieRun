using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public EquipmentTreasure equipmentTreasure;
    private Treasure equippedTreasureData;
    public int slotIndex;

    private void Start()
    {
        if (equipmentTreasure == null)
            Debug.LogError($"[EquipmentSlot] {gameObject.name}의 EquipmentTreasure가 설정되지 않았습니다!");

        if (GameData.IsDataLoaded)
            LoadEquipmentFromGameData();
        else
            Invoke(nameof(LoadEquipmentFromGameData), 0.1f);
    }

    private void LoadEquipmentFromGameData()
    {
        if (!GameData.IsDataLoaded)
        {
            Debug.LogWarning("[EquipmentSlot] GameData가 아직 로드되지 않았습니다. 0.2초 후 다시 시도...");
            Invoke(nameof(LoadEquipmentFromGameData), 0.2f);
            return;
        }

        string equippedItemId = GameData.GetEquipmentSlot(slotIndex);
        if (!string.IsNullOrEmpty(equippedItemId) && equippedItemId != "None")
        {
            Treasure treasure = InventoryManager.GetTreasureById(equippedItemId);
            if (treasure != null)
            {
                ReceiveTreasureData(treasure);
            }
            else
            {
                Debug.LogWarning($"[EquipmentSlot] 슬롯 {slotIndex}의 아이템 {equippedItemId}을 찾을 수 없음");
            }
        }
    }




    public void OnSlotClicked()
    {
        if (inventoryUI != null)
        {
            inventoryUI.OpenInventoryForSlot(this); // 슬롯을 선택하면 InventoryUI의 selectedSlot을 설정
            Debug.Log($"[EquipmentSlot] 슬롯 {slotIndex} 선택됨");
        }
    }

    public void ReceiveTreasureData(Treasure treasureData)
    {
        if (treasureData == null) return;
        equippedTreasureData = treasureData;

        // 자식 오브젝트 (EquipmentTreasure)에 데이터 적용
        if (equipmentTreasure != null)
        {
            equipmentTreasure.ApplyTreasureData(treasureData);
        }

        GameData.SetEquipmentSlot(slotIndex, treasureData.treasureID.ToString());
        GameData.SaveGameData();
    }


    public void ClearSlot()
    {
        equippedTreasureData = null;
        equipmentTreasure.ClearTreasure();
        GameData.SetEquipmentSlot(slotIndex, "None");
        GameData.SaveGameData();
    }

    public Treasure GetEquippedItem()
    {
        return equippedTreasureData;
    }

}
