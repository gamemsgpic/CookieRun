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
    public Button prevButton;
    public Button nextButton;
    public Button equipButton;
    public TextMeshProUGUI equipButtonText;
    public TextMeshProUGUI upgradeCostText;

    private List<CharacterTable> characterList;

    private void Start()
    {
        characterInfo = GetComponent<CharacterInfo>();

        LoadCharacterData();

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
        UpdateCharacterUI();
    }

    private void GetNextCharacter()
    {
        int index = characterList.FindIndex(c => c.Name == characterInfo.currentCharacterName);
        if (index != -1)
        {
            index = (index + 1) % characterList.Count;
            characterInfo.currentCharacterName = characterList[index].Name;
            characterInfo.currentCharacterLevel = GameData.GetCharacterLevel(characterInfo.currentCharacterName);

            // GameData ������Ʈ
            GameData.SetCharacter(characterInfo.currentCharacterName, characterInfo.currentCharacterLevel);

            UpdateCharacterUI();
        }
    }

    private void GetPreviousCharacter()
    {
        int index = characterList.FindIndex(c => c.Name == characterInfo.currentCharacterName);
        if (index != -1)
        {
            index = (index - 1 + characterList.Count) % characterList.Count;
            characterInfo.currentCharacterName = characterList[index].Name;
            characterInfo.currentCharacterLevel = GameData.GetCharacterLevel(characterInfo.currentCharacterName);

            // GameData ������Ʈ
            GameData.SetCharacter(characterInfo.currentCharacterName, characterInfo.currentCharacterLevel);

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
        GameData.EquipCharacter(selectedCharacter.Id, selectedCharacter.Name, selectedCharacter.Level,
            selectedCharacter.Hp, selectedCharacter.Cost, selectedCharacter.UpgradeCost,
            selectedCharacter.Explain_ID, selectedCharacter.Value, selectedCharacter.Image);

        Debug.Log($"[CharacterSelectionUI] {selectedCharacter.Name} (���� {selectedCharacter.Level}) ���� �Ϸ�.");

        UpdateCharacterUI();
    }

    public void UpdateCharacterUI()
    {
        var currentCharacter = CharacterTableObjectSC.GetCharacterData(characterInfo.currentCharacterName, characterInfo.currentCharacterLevel);
        if (currentCharacter == null)
        {
            Debug.LogError($"[CharacterSelectionUI] {characterInfo.currentCharacterName} (���� {characterInfo.currentCharacterLevel}) �����͸� ã�� �� �����ϴ�.");
            return;
        }

        characterNameText.text = currentCharacter.Name;
        characterLevelText.text = $"���� {currentCharacter.Level}";

        string imagePath = currentCharacter.Image.Trim().Replace(".png", "");
        characterImage.sprite = Resources.Load<Sprite>(imagePath);

        if (characterImage.sprite == null)
        {
            Debug.LogError($"[CharacterSelectionUI] ��������Ʈ �ε� ����: {imagePath}");
        }

        upgradeCostText.text = $"{currentCharacter.UpgradeCost:N0}";
        equipButtonText.text = (GameData.characterName == currentCharacter.Name) ? "������" : "����";
    }
}
