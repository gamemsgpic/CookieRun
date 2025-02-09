using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Treasure : MonoBehaviour
{
    public int treasureID; // 아이템 고유 ID
    public string treasureName; // 아이템 이름
    public int treasureVelue; // 아이템 값
    public float treasureRadius;
    public float treasureSpeed;
    public string treasurePath; // Resources 내부 경로
    public Sprite treasureIcon; // 아이템 아이콘

    // CSV 데이터를 받아 아이템 속성을 설정하는 메서드
    public void Setup(TreasureTable data)
    {
        if (data == null)
        {
            Debug.LogError("[Treasure] data가 null이므로 설정할 수 없음!");
            return;
        }

        treasureID = data.Id;
        treasureName = data.Name;
        treasureVelue = data.Velue;
        treasureRadius = data.Radius;
        treasureSpeed = data.Speed;
        treasurePath = data.Path;

        // 아이콘 설정 (Resources 폴더에서 불러오기)
        treasureIcon = Resources.Load<Sprite>(treasurePath);
        if (treasureIcon == null)
        {
            Debug.LogError($"[Treasure] {treasurePath}에서 스프라이트를 찾을 수 없음!");
        }
    }


    // Resources 폴더에서 스프라이트 로드
    public void LoadSpriteFromResources()
    {
        if (!string.IsNullOrEmpty(treasurePath))
        {
            treasureIcon = Resources.Load<Sprite>(treasurePath);
            if (treasureIcon == null)
            {
                Debug.LogError($"[Treasure] 스프라이트 로드 실패: {treasurePath} (Resources 내부 확인 필요)");
            }
        }
    }
}
