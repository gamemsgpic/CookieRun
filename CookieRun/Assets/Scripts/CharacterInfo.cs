using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public string currentCharacterName;
    public int currentCharacterLevel;
    private List<CharacterTable> characterList;
    private CharacterSelectionUI characterSelectionUI;

    private void Start()
    {
        characterSelectionUI = GetComponent<CharacterSelectionUI>();
        LoadCharacterData(); // GameData에서 캐릭터 정보 불러오기
    }

    private void LoadCharacterData()
    {
        // GameData에서 캐릭터 정보 불러오기
        currentCharacterName = GameData.characterName;
        currentCharacterLevel = GameData.characterLevel;

        // 데이터가 없으면 기본값 설정
        if (string.IsNullOrEmpty(currentCharacterName) || currentCharacterLevel <= 0)
        {
            InitializeCharacterData();
        }

        // UI 갱신
        characterSelectionUI?.UpdateCharacterUI();
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

        // GameData에 저장
        GameData.EquipCharacter(
            characterList[0].Id, characterList[0].Name, characterList[0].Level,
            characterList[0].Hp, characterList[0].Cost, characterList[0].UpgradeCost,
            characterList[0].Explain_ID, characterList[0].Value, characterList[0].Image
        );
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

        // GameData 업데이트
        GameData.EquipCharacter(
            upgradedCharacter.ID, upgradedCharacter.Name, upgradedCharacter.Level,
            upgradedCharacter.Hp, upgradedCharacter.Cost, upgradedCharacter.UpgradeCost,
            upgradedCharacter.Explain_ID, upgradedCharacter.Value, upgradedCharacter.Image);

        // UI 업데이트
        characterSelectionUI?.UpdateCharacterUI();
    }
}
