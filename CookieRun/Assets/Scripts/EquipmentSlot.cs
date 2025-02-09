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
            Debug.LogError($"[EquipmentSlot] {gameObject.name}�� EquipmentTreasure�� �������� �ʾҽ��ϴ�!");

        if (GameData.IsDataLoaded)
            LoadEquipmentFromGameData();
        else
            Invoke(nameof(LoadEquipmentFromGameData), 0.1f);
    }

    private void LoadEquipmentFromGameData()
    {
        if (!GameData.IsDataLoaded)
        {
            Debug.LogWarning("[EquipmentSlot] GameData�� ���� �ε���� �ʾҽ��ϴ�. 0.2�� �� �ٽ� �õ�...");
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
                Debug.LogWarning($"[EquipmentSlot] ���� {slotIndex}�� ������ {equippedItemId}�� ã�� �� ����");
            }
        }
    }




    public void OnSlotClicked()
    {
        if (inventoryUI != null)
        {
            inventoryUI.OpenInventoryForSlot(this); // ������ �����ϸ� InventoryUI�� selectedSlot�� ����
            Debug.Log($"[EquipmentSlot] ���� {slotIndex} ���õ�");
        }
    }

    public void ReceiveTreasureData(Treasure treasureData)
    {
        if (treasureData == null) return;
        equippedTreasureData = treasureData;

        // �ڽ� ������Ʈ (EquipmentTreasure)�� ������ ����
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
