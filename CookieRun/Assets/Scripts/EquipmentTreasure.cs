using UnityEngine;
using UnityEngine.UI;

public class EquipmentTreasure : MonoBehaviour
{
    private Image treasureImage; // public 제거하고 private으로 변경
    private Treasure treasureData; // 현재 장착된 아이템 데이터

    private void Start()
    {
        treasureImage.color = new Color(1, 1, 1, 0);
    }

    private void Awake()
    {
        // 자신이 가진 Image 컴포넌트를 자동으로 찾기
        treasureImage = GetComponent<Image>();

        if (treasureImage == null)
        {
            Debug.LogError("[EquipmentTreasure] Image 컴포넌트가 없습니다! EquipmentTreasure에 Image를 추가하세요.");
        }
    }

    public void ApplyTreasureData(Treasure data)
    {
        if (data == null)
        {
            Debug.LogError("[EquipmentTreasure] 장착할 데이터가 null입니다!");
            return;
        }

        treasureData = data;

        // 스프라이트 변경
        if (treasureImage != null)
        {
            treasureImage.sprite = treasureData.treasureIcon;
            treasureImage.color = new Color(1, 1, 1, 1);
        }

        Debug.Log($"[EquipmentTreasure] {treasureData.treasureName} 스프라이트 적용됨");
    }


    public void ClearTreasure()
    {
        treasureData = null;

        if (treasureImage != null)
        {
            treasureImage.sprite = null; // 스프라이트 제거
            treasureImage.color = new Color(1, 1, 1, 0); // 이미지의 알파값을 0으로 설정 (투명)
        }

        Debug.Log("[EquipmentTreasure] 장착 해제됨, UI 초기화 완료");
    }
}
