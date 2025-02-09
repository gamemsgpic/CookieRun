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
            Debug.LogError("[CharacterInfo] �⺻ ĳ���� �����͸� �ҷ��� �� �����ϴ�.");
            return;
        }

        // ù ��° ĳ���͸� �⺻���� ����
        currentCharacterName = characterList[0].Name;
        currentCharacterLevel = characterList[0].Level;
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

        // GameData�� ���õ� ĳ���� ���� ���� (Name�� Level�� ����)
        GameData.EquipCharacter(upgradedCharacter.Name, upgradedCharacter.Level);
    }

}
