using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureItem : MonoBehaviour
{
    public Treasure treasureData; // 아이템 데이터
    public Button button; // 클릭 이벤트 처리
    public TextMeshProUGUI equipStatusText; // "장착중" 텍스트 (자식 오브젝트)

    private InventoryUI inventoryUI;
    private Image treasureImage; // 자식 오브젝트 Treasure의 Image

    public void Setup(Treasure data, InventoryUI ui)
    {
        treasureData = data;
        inventoryUI = ui;

        // 버튼 이벤트 추가 (클릭 시 아이템 선택)
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("[TreasureItem] 버튼 컴포넌트를 찾을 수 없습니다!");
            return;
        }
        button.onClick.AddListener(OnItemClicked);

        // 자식 오브젝트(Treasure)에서 Image 컴포넌트 찾기
        Treasure treasureComponent = GetComponentInChildren<Treasure>();

        if (treasureComponent == null)
        {
            Debug.LogError("[TreasureItem] 자식 오브젝트에서 Treasure 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        // 스프라이트 적용 (자식 오브젝트의 이미지)
        treasureImage = treasureComponent.GetComponent<Image>();
        if (treasureImage != null)
        {
            treasureImage.sprite = treasureData.treasureIcon;
        }
        else
        {
            Debug.LogError("[TreasureItem] Treasure의 Image를 찾을 수 없습니다!");
        }
    }

    private void OnItemClicked()
    {
        Debug.Log($"[TreasureItem] {treasureData.treasureName} 클릭됨");
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
        // 아웃라인 효과 적용 (노란색 강조)
        Outline outline = GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>(); // 없으면 추가
        }

        outline.effectColor = isSelected ? Color.yellow : new Color(0, 0, 0, 0); // 노란색 or 투명
        outline.enabled = isSelected; // 선택 시 활성화
    }
}
