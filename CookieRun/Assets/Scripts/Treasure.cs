using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Treasure : MonoBehaviour
{
    public int treasureID; // ������ ���� ID
    public string treasureName; // ������ �̸�
    public int treasureVelue; // ������ ��
    public float treasureRadius;
    public float treasureSpeed;
    public string treasurePath; // Resources ���� ���
    public Sprite treasureIcon; // ������ ������

    // CSV �����͸� �޾� ������ �Ӽ��� �����ϴ� �޼���
    public void Setup(TreasureTable data)
    {
        if (data == null)
        {
            Debug.LogError("[Treasure] data�� null�̹Ƿ� ������ �� ����!");
            return;
        }

        treasureID = data.Id;
        treasureName = data.Name;
        treasureVelue = data.Velue;
        treasureRadius = data.Radius;
        treasureSpeed = data.Speed;
        treasurePath = data.Path;

        // ������ ���� (Resources �������� �ҷ�����)
        treasureIcon = Resources.Load<Sprite>(treasurePath);
        if (treasureIcon == null)
        {
            Debug.LogError($"[Treasure] {treasurePath}���� ��������Ʈ�� ã�� �� ����!");
        }
    }


    // Resources �������� ��������Ʈ �ε�
    public void LoadSpriteFromResources()
    {
        if (!string.IsNullOrEmpty(treasurePath))
        {
            treasureIcon = Resources.Load<Sprite>(treasurePath);
            if (treasureIcon == null)
            {
                Debug.LogError($"[Treasure] ��������Ʈ �ε� ����: {treasurePath} (Resources ���� Ȯ�� �ʿ�)");
            }
        }
    }
}
