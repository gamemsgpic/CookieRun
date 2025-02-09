using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public string currentCharacterName;
    public int currentCharacterLevel;
    private List<CharacterTable> characterList;

    private void Start()
    {
        InitializeCharacterData();
    }

    private void InitializeCharacterData()
    {
        characterList = CharacterTableObjectSC.GetBaseCharacters();

        if (characterList == null || characterList.Count == 0)
        {
            Debug.LogError("[CharacterInfo] 기본 캐릭터 데이터를 불러올 수 없습니다.");
            return;
        }

        // 첫 번째 캐릭터를 기본으로 설정
        currentCharacterName = characterList[0].Name;
        currentCharacterLevel = characterList[0].Level;
    }

    public void UpgradeCharacter()
    {
        currentCharacterLevel++;

        var upgradedCharacter = CharacterTableObjectSC.GetCharacterData(currentCharacterName, currentCharacterLevel);

        if (upgradedCharacter == null)
        {
            Debug.LogError($"[CharacterInfo] {currentCharacterName} (레벨 {currentCharacterLevel}) 데이터를 찾을 수 없습니다.");
            return;
        }

        // GameData에 선택된 캐릭터 정보 저장 (Name과 Level을 전달)
        GameData.EquipCharacter(upgradedCharacter.Name, upgradedCharacter.Level);
    }

}
