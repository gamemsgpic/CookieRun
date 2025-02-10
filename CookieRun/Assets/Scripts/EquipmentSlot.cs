using UnityEngine;
using static GameData;

public class EquipmentSlot : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public EquipmentTreasure equipmentTreasure; // UI + ������ �����ϴ� ������Ʈ
    public int slotIndex;

    private void Start()
    {
        if (equipmentTreasure == null)
        {
            Debug.LogError($"[EquipmentSlot] {gameObject.name}�� EquipmentTreasure�� �������� �ʾҽ��ϴ�!");
        }

        LoadEquipmentFromGameData(); // GameData���� ������ �ε�
    }

    /// **GameData���� �����͸� �ҷ��� EquipmentTreasure�� ����**
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
                Debug.LogWarning($"[EquipmentSlot] ���� {slotIndex}�� ������ {equippedData.treasureID}�� ã�� �� ����");
            }
        }
    }

    /// **���ο� �������� ����**
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

    /// **���� ���� (�� �������� �ʱ�ȭ)**
    public void UnequipTreasure()
    {
        equipmentTreasure.ClearTreasure();

        // GameData������ ����
        GameData.SetEquipmentSlot(slotIndex, new EquipmentSlotData());
        GameData.SaveGameData();
    }

    /// **���� ������ ������ ��ȯ**
    public Treasure GetEquippedItem()
    {
        return equipmentTreasure.treasureData;
    }

    public void OnSlotClicked()
    {
        inventoryUI.OpenInventoryForSlot(this);
    }
}
