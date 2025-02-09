using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureItem : MonoBehaviour
{
    public Treasure treasureData; // ������ ������
    public Button button; // Ŭ�� �̺�Ʈ ó��
    public TextMeshProUGUI equipStatusText; // "������" �ؽ�Ʈ (�ڽ� ������Ʈ)

    private InventoryUI inventoryUI;
    private Image treasureImage; // �ڽ� ������Ʈ Treasure�� Image

    public void Setup(Treasure data, InventoryUI ui)
    {
        treasureData = data;
        inventoryUI = ui;

        // ��ư �̺�Ʈ �߰� (Ŭ�� �� ������ ����)
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("[TreasureItem] ��ư ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }
        button.onClick.AddListener(OnItemClicked);

        // �ڽ� ������Ʈ(Treasure)���� Image ������Ʈ ã��
        Treasure treasureComponent = GetComponentInChildren<Treasure>();

        if (treasureComponent == null)
        {
            Debug.LogError("[TreasureItem] �ڽ� ������Ʈ���� Treasure ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        // ��������Ʈ ���� (�ڽ� ������Ʈ�� �̹���)
        treasureImage = treasureComponent.GetComponent<Image>();
        if (treasureImage != null)
        {
            treasureImage.sprite = treasureData.treasureIcon;
        }
        else
        {
            Debug.LogError("[TreasureItem] Treasure�� Image�� ã�� �� �����ϴ�!");
        }
    }

    private void OnItemClicked()
    {
        Debug.Log($"[TreasureItem] {treasureData.treasureName} Ŭ����");
        inventoryUI.SelectItem(this);
    }

    public void SetEquipStatus(bool isEquipped)
    {
        if (equipStatusText != null)
        {
            equipStatusText.gameObject.SetActive(isEquipped);
        }
    }

    public void SelectItemEffect(bool isSelected)
    {
        // �ƿ����� ȿ�� ���� (����� ����)
        Outline outline = GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>(); // ������ �߰�
        }

        outline.effectColor = isSelected ? Color.yellow : new Color(0, 0, 0, 0); // ����� or ����
        outline.enabled = isSelected; // ���� �� Ȱ��ȭ
    }
}
