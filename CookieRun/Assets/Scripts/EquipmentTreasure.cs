using UnityEngine;
using UnityEngine.UI;

public class EquipmentTreasure : MonoBehaviour
{
    public Treasure treasureData; // 현재 장착된 아이템 데이터
    private Image treasureImage; // 아이템 이미지

    private void Awake()
    {
        treasureImage = GetComponent<Image>();

        if (treasureImage == null)
        {
            Debug.LogError("[EquipmentTreasure] Image 컴포넌트가 없습니다! EquipmentTreasure에 Image를 추가하세요.");
        }
    }

    private void Start()
    {
        treasureImage.color = new Color(1, 1, 1, 0); // 초기에는 투명 처리
    }

    /// **아이템 장착 시 데이터 적용**
    public void ApplyTreasureData(Treasure data)
    {
        if (data == null)
        {
            Debug.LogError("[EquipmentTreasure] 적용할 데이터가 없습니다!");
            return;
        }

        treasureData = data;

        if (treasureImage != null)
        {
            treasureImage.sprite = treasureData.treasureIcon;
            treasureImage.color = new Color(1, 1, 1, 1);
        }

        Debug.Log($"[EquipmentTreasure] {treasureData.treasureName} 스프라이트 적용됨");

        // **데이터 값들 적용 (GameData에서 불러온 값이 제대로 반영)**
        treasureData.treasureID = data.treasureID;
        treasureData.treasureName = data.treasureName;
        treasureData.treasureVelue = data.treasureVelue;
        treasureData.treasureRadius = data.treasureRadius;
        treasureData.treasureSpeed = data.treasureSpeed;
        treasureData.treasurePath = data.treasurePath;
    }

    /// **아이템 장착 해제**
    public void ClearTreasure()
    {
        treasureData = null;

        if (treasureImage != null)
        {
            treasureImage.sprite = null; // 스프라이트 제거
            treasureImage.color = new Color(1, 1, 1, 0); // 투명 처리
        }

        Debug.Log("[EquipmentTreasure] 장착 해제됨, UI 초기화 완료");
    }
}
