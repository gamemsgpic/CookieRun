using UnityEngine;
using UnityEngine.UI;

public class EquipmentTreasure : MonoBehaviour
{
    public Treasure treasureData; // ���� ������ ������ ������
    private Image treasureImage; // ������ �̹���

    private void Awake()
    {
        treasureImage = GetComponent<Image>();

        if (treasureImage == null)
        {
            Debug.LogError("[EquipmentTreasure] Image ������Ʈ�� �����ϴ�! EquipmentTreasure�� Image�� �߰��ϼ���.");
        }
    }

    private void Start()
    {
        treasureImage.color = new Color(1, 1, 1, 0); // �ʱ⿡�� ���� ó��
    }

    /// **������ ���� �� ������ ����**
    public void ApplyTreasureData(Treasure data)
    {
        if (data == null)
        {
            Debug.LogError("[EquipmentTreasure] ������ �����Ͱ� �����ϴ�!");
            return;
        }

        treasureData = data;

        if (treasureImage != null)
        {
            treasureImage.sprite = treasureData.treasureIcon;
            treasureImage.color = new Color(1, 1, 1, 1);
        }

        Debug.Log($"[EquipmentTreasure] {treasureData.treasureName} ��������Ʈ �����");

        // **������ ���� ���� (GameData���� �ҷ��� ���� ����� �ݿ�)**
        treasureData.treasureID = data.treasureID;
        treasureData.treasureName = data.treasureName;
        treasureData.treasureVelue = data.treasureVelue;
        treasureData.treasureRadius = data.treasureRadius;
        treasureData.treasureSpeed = data.treasureSpeed;
        treasureData.treasurePath = data.treasurePath;
    }

    /// **������ ���� ����**
    public void ClearTreasure()
    {
        treasureData = null;

        if (treasureImage != null)
        {
            treasureImage.sprite = null; // ��������Ʈ ����
            treasureImage.color = new Color(1, 1, 1, 0); // ���� ó��
        }

        Debug.Log("[EquipmentTreasure] ���� ������, UI �ʱ�ȭ �Ϸ�");
    }
}
