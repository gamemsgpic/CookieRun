using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionUI : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public Image characterImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterLevelText;
    public TextMeshProUGUI characterHpText;
    public TextMeshProUGUI characterAbilityText;
    public Button prevButton;
    public Button nextButton;
    public Button equipButton;
    public TextMeshProUGUI equipButtonText;
    public TextMeshProUGUI upgradeCostText;

    private List<CharacterTable> characterList;

    private void Start()
    {
        if (characterInfo == null)
        {
            characterInfo = FindObjectOfType<CharacterInfo>();
            if (characterInfo == null)
            {
                Debug.LogError("[CharacterSelectionUI] characterInfo�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
                return;
            }
        }

        LoadCharacterData();
        ApplyGameDataToCharacter();
        UpdateCharacterUI();

        prevButton.onClick.AddListener(GetPreviousCharacter);
        nextButton.onClick.AddListener(GetNextCharacter);
        equipButton.onClick.AddListener(EquipCharacter);
    }

    private void LoadCharacterData()
    {
        characterList = CharacterTableObjectSC.GetBaseCharacters();
        if (characterList == null || characterList.Count == 0)
        {
            Debug.LogError("[CharacterSelectionUI] �⺻ ĳ���� �����͸� �ε��� �� �����ϴ�.");
            characterList = new List<CharacterTable>();
            return;
        }
        Debug.Log($"[CharacterSelectionUI] {characterList.Count}���� �⺻ ĳ���� ������ �ε� �Ϸ�.");
    }

    private void ApplyGameDataToCharacter()
    {
        // GameData���� �ҷ��� �� ����
        characterInfo.SetCharacterData(
            GameData.characterId,
            GameData.characterName,
            GameData.characterLevel,
            GameData.characterHp,
            GameData.characterCost,
            GameData.characterUpgradeCost,
            GameData.characterExplain_ID,
            GameData.characterValue,
            GameData.characterAbility,
            GameData.characterImage
        );
    }

    private void GetNextCharacter()
    {
        int index = characterList.FindIndex(c => c.Name == characterInfo.currentCharacterName);
        if (index != -1)
        {
            index = (index + 1) % characterList.Count;
            characterInfo.SetCharacterData(characterList[index]);
            UpdateCharacterUI();
        }
    }

    private void GetPreviousCharacter()
    {
        int index = characterList.FindIndex(c => c.Name == characterInfo.currentCharacterName);
        if (index != -1)
        {
            index = (index - 1 + characterList.Count) % characterList.Count;
            characterInfo.SetCharacterData(characterList[index]);
            UpdateCharacterUI();
        }
    }

    private void EquipCharacter()
    {
        var selectedCharacter = characterList.Find(c => c.Name == characterInfo.currentCharacterName);
        if (selectedCharacter == null)
        {
            Debug.LogError($"[CharacterSelectionUI] {characterInfo.currentCharacterName} �����͸� ã�� �� �����ϴ�.");
            return;
        }

        // GameData�� ���õ� ĳ���� ���� ����
        GameData.EquipCharacter(
            selectedCharacter.ID, selectedCharacter.Name, selectedCharacter.Level,
            selectedCharacter.Hp, selectedCharacter.Cost, selectedCharacter.UpgradeCost,
            selectedCharacter.Explain_ID, selectedCharacter.Value, selectedCharacter.Ability, selectedCharacter.Image
        );

        Debug.Log($"[CharacterSelectionUI] {selectedCharacter.Name} (���� {characterInfo.currentCharacterLevel}) ���� �Ϸ�.");
        UpdateCharacterUI();
    }

    public void UpdateCharacterUI()
    {
        if (GameData.characterLevel <= 0)
        {
            Debug.LogWarning("[CharacterSelectionUI] �߸��� ���� �� ����, 1�� ����");
            GameData.characterLevel = 1;
        }

        var currentCharacter = CharacterTableObjectSC.GetCharacterData(characterInfo.currentCharacterName, characterInfo.currentCharacterLevel);
        if (currentCharacter == null)
        {
            Debug.LogError($"[CharacterSelectionUI] {characterInfo.currentCharacterName} (���� {characterInfo.currentCharacterLevel}) �����͸� ã�� �� �����ϴ�.");
            return;
        }

        characterNameText.text = currentCharacter.Name;
        characterLevelText.text = $"���� {currentCharacter.Level}";
        characterHpText.text = currentCharacter.Hp.ToString();
        characterAbilityText.text = currentCharacter.Ability;

        // **�̹��� �ε� (��� Ȯ�� �� ����� �߰�)**
        string imagePath = currentCharacter.Image?.Trim().Replace(".png", ""); // �� üũ
        if (string.IsNullOrEmpty(imagePath))
        {
            Debug.LogError($"[CharacterSelectionUI] �̹��� ��ΰ� ����ֽ��ϴ�. ({currentCharacter.Name})");
            return;
        }

        characterImage.sprite = Resources.Load<Sprite>(imagePath);

        if (characterImage.sprite == null)
        {
            Debug.LogError($"[CharacterSelectionUI] ��������Ʈ �ε� ����: {imagePath}");
        }

        upgradeCostText.text = $"{currentCharacter.UpgradeCost:N0}";
        equipButtonText.text = (GameData.characterName == currentCharacter.Name) ? "������" : "����";
    }

}
