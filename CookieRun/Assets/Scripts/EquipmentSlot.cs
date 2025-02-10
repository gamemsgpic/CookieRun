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
            inventoryUI.OpenInventoryForSlot(this);
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

        // �ڽ� ������Ʈ�� Treasure ��ũ��Ʈ���� ������ ����
        Treasure childTreasure = equipmentTreasure.GetComponent<Treasure>();
        if (childTreasure != null)
        {
            ApplyTreasureDataToChild(childTreasure, treasureData);
        }
        else
        {
            Debug.LogError($"[EquipmentSlot] ���� {slotIndex}���� EquipmentTreasure�� �ڽ� Treasure ������Ʈ�� ã�� �� ����!");
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

        Debug.Log($"[EquipmentSlot] ���� {slotIndex}�� �ڽ� Treasure ������Ʈ�� ������ ���� �Ϸ�.");
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

        Debug.Log($"[EquipmentSlot] ���� {slotIndex}�� �ڽ� Treasure ������Ʈ�� ������ ���� �Ϸ�.");
    }

    public void ClearSlot()
    {
        equippedTreasureData = null;
        if (equipmentTreasure != null)
        {
            equipmentTreasure.ClearTreasure();

            // �ڽ� ������Ʈ�� Treasure ���� �ʱ�ȭ
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

        Debug.Log($"[EquipmentSlot] ���� {slotIndex}�� �ڽ� Treasure ������Ʈ ������ �ʱ�ȭ �Ϸ�.");
    }

    public Treasure GetEquippedItem()
    {
        return equippedTreasureData;
    }
}
