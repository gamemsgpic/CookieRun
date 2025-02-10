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
            inventoryUI.OpenInventoryForSlot(this);
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

        // 자식 오브젝트의 Treasure 스크립트에도 데이터 적용
        Treasure childTreasure = equipmentTreasure.GetComponent<Treasure>();
        if (childTreasure != null)
        {
            ApplyTreasureDataToChild(childTreasure, treasureData);
        }
        else
        {
            Debug.LogError($"[EquipmentSlot] 슬롯 {slotIndex}에서 EquipmentTreasure의 자식 Treasure 컴포넌트를 찾을 수 없음!");
        }

        GameData.SetEquipmentSlot(slotIndex, treasureData.treasureID.ToString());
        GameData.SaveGameData();
    }

    private void ApplyTreasureDataToChild(Treasure childTreasure, Treasure data)
    {
        childTreasure.treasureID = data.treasureID;
        childTreasure.treasureName = data.treasureName;
        childTreasure.treasureVelue = data.treasureVelue;
        childTreasure.treasureRadius = data.treasureRadius;
        childTreasure.treasureSpeed = data.treasureSpeed;
        childTreasure.treasurePath = data.treasurePath;
        childTreasure.treasureIcon = data.treasureIcon;

        Debug.Log($"[EquipmentSlot] 슬롯 {slotIndex}의 자식 Treasure 오브젝트에 데이터 적용 완료.");
    }


    private void ApplyTreasureDataToChild(Treasure childTreasure, Treasure data)
    {
        childTreasure.treasureID = data.treasureID;
        childTreasure.treasureName = data.treasureName;
        childTreasure.treasureVelue = data.treasureVelue;
        childTreasure.treasureRadius = data.treasureRadius;
        childTreasure.treasureSpeed = data.treasureSpeed;
        childTreasure.treasurePath = data.treasurePath;
        childTreasure.treasureIcon = data.treasureIcon;

        Debug.Log($"[EquipmentSlot] 슬롯 {slotIndex}의 자식 Treasure 오브젝트에 데이터 적용 완료.");
    }

    public void ClearSlot()
    {
        equippedTreasureData = null;
        if (equipmentTreasure != null)
        {
            equipmentTreasure.ClearTreasure();

            // 자식 오브젝트의 Treasure 정보 초기화
            Treasure childTreasure = equipmentTreasure.GetComponent<Treasure>();
            if (childTreasure != null)
            {
                ClearChildTreasureData(childTreasure);
            }
        }

        GameData.SetEquipmentSlot(slotIndex, "None");
        GameData.SaveGameData();
    }

    private void ClearChildTreasureData(Treasure childTreasure)
    {
        childTreasure.treasureID = 0;
        childTreasure.treasureName = "";
        childTreasure.treasureVelue = 0;
        childTreasure.treasureRadius = 0f;
        childTreasure.treasureSpeed = 0f;
        childTreasure.treasurePath = "";
        childTreasure.treasureIcon = null;

        Debug.Log($"[EquipmentSlot] 슬롯 {slotIndex}의 자식 Treasure 오브젝트 데이터 초기화 완료.");
    }

    public Treasure GetEquippedItem()
    {
        return equippedTreasureData;
    }
}
