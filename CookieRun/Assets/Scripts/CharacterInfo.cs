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
        LoadCharacterData(); // GameData���� ĳ���� ���� �ҷ�����
    }

    private void LoadCharacterData()
    {
        // GameData���� ĳ���� ���� �ҷ�����
        currentCharacterName = GameData.characterName;
        currentCharacterLevel = GameData.characterLevel;

        // �����Ͱ� ������ �⺻�� ����
        if (string.IsNullOrEmpty(currentCharacterName) || currentCharacterLevel <= 0)
        {
            InitializeCharacterData();
        }

        // UI ����
        characterSelectionUI?.UpdateCharacterUI();
    }

    private void InitializeCharacterData()
    {
        characterList = CharacterTableObjectSC.GetBaseCharacters();

        if (characterList == null || characterList.Count == 0)
        {
            Debug.LogError("[CharacterInfo] �⺻ ĳ���� �����͸� �ҷ��� �� �����ϴ�.");
            return;
        }

        // ù ��° ĳ���͸� �⺻���� ����
        currentCharacterName = characterList[0].Name;
        currentCharacterLevel = characterList[0].Level;

        // GameData�� ����
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
            Debug.LogError($"[CharacterInfo] {currentCharacterName} (���� {currentCharacterLevel}) �����͸� ã�� �� �����ϴ�.");
            return;
        }

        // GameData ������Ʈ
        GameData.EquipCharacter(
            upgradedCharacter.ID, upgradedCharacter.Name, upgradedCharacter.Level,
            upgradedCharacter.Hp, upgradedCharacter.Cost, upgradedCharacter.UpgradeCost,
            upgradedCharacter.Explain_ID, upgradedCharacter.Value, upgradedCharacter.Image);

        // UI ������Ʈ
        characterSelectionUI?.UpdateCharacterUI();
    }
}
