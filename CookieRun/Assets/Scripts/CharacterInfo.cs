using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public int currentCharacterId;
    public string currentCharacterName;
    public int currentCharacterLevel;
    public int currentCharacterHp;
    public int currentCharacterCost;
    public int currentCharacterUpgradeCost;
    public int currentCharacterExplain_ID;
    public int currentCharacterValue;
    public string currentCharacterAbility;
    public string currentCharacterImage;

    private List<CharacterTable> characterList;
    private CharacterSelectionUI characterSelectionUI;

    private void Start()
    {
        InitializeCharacterData();

        characterSelectionUI = GetComponent<CharacterSelectionUI>();

        if (string.IsNullOrEmpty(GameData.characterName))
        {
            GameData.SetDefaultCharacter(); // �⺻ ĳ����(�����Ű) ����
        }

        // **UI ������Ʈ**
        UpdateCharacterInfoFromGameData();
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
        SetCharacterData(characterList[0]);
    }

    public void SetCharacterData(CharacterTable character)
    {
        currentCharacterId = character.ID;
        currentCharacterName = character.Name;
        currentCharacterLevel = character.Level;
        currentCharacterHp = character.Hp;
        currentCharacterCost = character.Cost;
        currentCharacterUpgradeCost = character.UpgradeCost;
        currentCharacterExplain_ID = character.Explain_ID;
        currentCharacterValue = character.Value;
        currentCharacterAbility = character.Ability;
        currentCharacterImage = character.Image;
    }

    public void SetCharacterData(
        int id, string name, int level, int hp, int cost, int upgradeCost, int explainId, int value, string ability, string image)
    {
        currentCharacterId = id;
        currentCharacterName = name;
        currentCharacterLevel = level;
        currentCharacterHp = hp;
        currentCharacterCost = cost;
        currentCharacterUpgradeCost = upgradeCost;
        currentCharacterExplain_ID = explainId;
        currentCharacterValue = value;
        currentCharacterAbility = ability;
        currentCharacterImage = image;
    }

    public void UpgradeCharacter()
    {
        if (currentCharacterLevel < 8)
        {
            currentCharacterLevel++;
        }

        // ĳ���� ������ ����
        var upgradedCharacter = CharacterTableObjectSC.GetCharacterData(currentCharacterName, currentCharacterLevel);
        if (upgradedCharacter == null)
        {
            Debug.LogError($"[CharacterInfo] {currentCharacterName} (���� {currentCharacterLevel}) �����͸� ã�� �� �����ϴ�.");
            return;
        }

        // GameData�� ������Ʈ
        GameData.EquipCharacter(
            upgradedCharacter.ID, upgradedCharacter.Name, upgradedCharacter.Level,
            upgradedCharacter.Hp, upgradedCharacter.Cost, upgradedCharacter.UpgradeCost,
            upgradedCharacter.Explain_ID, upgradedCharacter.Value, upgradedCharacter.Ability, upgradedCharacter.Image
        );

        // UI ����
        if (characterSelectionUI != null)
        {
            characterSelectionUI.UpdateCharacterUI();
        }

        Debug.Log($"[CharacterInfo] {currentCharacterName}��(��) ���� {currentCharacterLevel}�� ���׷��̵��.");
    }


    private void UpdateCharacterInfoFromGameData()
    {
        currentCharacterId = GameData.characterId;
        currentCharacterName = GameData.characterName;
        currentCharacterLevel = GameData.characterLevel;
        currentCharacterHp = GameData.characterHp;
        currentCharacterCost = GameData.characterCost;
        currentCharacterUpgradeCost = GameData.characterUpgradeCost;
        currentCharacterExplain_ID = GameData.characterExplain_ID;
        currentCharacterValue = GameData.characterValue;
        currentCharacterAbility = GameData.characterAbility;
        currentCharacterImage = GameData.characterImage;

        // **UI ������Ʈ**
        if (characterSelectionUI != null)
        {
            characterSelectionUI.UpdateCharacterUI();
        }
        else
        {
            Debug.LogWarning("[CharacterInfo] CharacterSelectionUI�� ������� �ʾҽ��ϴ�!");
        }
    }

}
