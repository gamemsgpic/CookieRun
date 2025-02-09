using UnityEngine;
using UnityEngine.UI;

public class EquipmentTreasure : MonoBehaviour
{
    private Image treasureImage; // public �����ϰ� private���� ����
    private Treasure treasureData; // ���� ������ ������ ������

    private void Start()
    {
        treasureImage.color = new Color(1, 1, 1, 0);
    }

    private void Awake()
    {
        // �ڽ��� ���� Image ������Ʈ�� �ڵ����� ã��
        treasureImage = GetComponent<Image>();

        if (treasureImage == null)
        {
            Debug.LogError("[EquipmentTreasure] Image ������Ʈ�� �����ϴ�! EquipmentTreasure�� Image�� �߰��ϼ���.");
        }
    }

    public void ApplyTreasureData(Treasure data)
    {
        if (data == null)
        {
            Debug.LogError("[EquipmentTreasure] ������ �����Ͱ� null�Դϴ�!");
            return;
        }

        treasureData = data;

        // ��������Ʈ ����
        if (treasureImage != null)
        {
            treasureImage.sprite = treasureData.treasureIcon;
            treasureImage.color = new Color(1, 1, 1, 1);
        }

        Debug.Log($"[EquipmentTreasure] {treasureData.treasureName} ��������Ʈ �����");
    }


    public void ClearTreasure()
    {
        treasureData = null;

        if (treasureImage != null)
        {
            treasureImage.sprite = null; // ��������Ʈ ����
            treasureImage.color = new Color(1, 1, 1, 0); // �̹����� ���İ��� 0���� ���� (����)
        }

        Debug.Log("[EquipmentTreasure] ���� ������, UI �ʱ�ȭ �Ϸ�");
    }
}
