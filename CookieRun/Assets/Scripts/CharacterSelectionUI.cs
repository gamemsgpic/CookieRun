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
                Debug.LogError("[CharacterSelectionUI] characterInfo가 초기화되지 않았습니다.");
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
            Debug.LogError("[CharacterSelectionUI] 기본 캐릭터 데이터를 로드할 수 없습니다.");
            characterList = new List<CharacterTable>();
            return;
        }
        Debug.Log($"[CharacterSelectionUI] {characterList.Count}개의 기본 캐릭터 데이터 로드 완료.");
    }

    private void ApplyGameDataToCharacter()
    {
        // GameData에서 불러온 값 적용
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
            Debug.LogError($"[CharacterSelectionUI] {characterInfo.currentCharacterName} 데이터를 찾을 수 없습니다.");
            return;
        }

        // GameData에 선택된 캐릭터 정보 저장
        GameData.EquipCharacter(
            selectedCharacter.ID, selectedCharacter.Name, selectedCharacter.Level,
            selectedCharacter.Hp, selectedCharacter.Cost, selectedCharacter.UpgradeCost,
            selectedCharacter.Explain_ID, selectedCharacter.Value, selectedCharacter.Ability, selectedCharacter.Image
        );

        Debug.Log($"[CharacterSelectionUI] {selectedCharacter.Name} (레벨 {characterInfo.currentCharacterLevel}) 착용 완료.");
        UpdateCharacterUI();
    }

    public void UpdateCharacterUI()
    {
        if (GameData.characterLevel <= 0)
        {
            Debug.LogWarning("[CharacterSelectionUI] 잘못된 레벨 값 감지, 1로 보정");
            GameData.characterLevel = 1;
        }

        var currentCharacter = CharacterTableObjectSC.GetCharacterData(characterInfo.currentCharacterName, characterInfo.currentCharacterLevel);
        if (currentCharacter == null)
        {
            Debug.LogError($"[CharacterSelectionUI] {characterInfo.currentCharacterName} (레벨 {characterInfo.currentCharacterLevel}) 데이터를 찾을 수 없습니다.");
            return;
        }

        characterNameText.text = currentCharacter.Name;
        characterLevelText.text = $"레벨 {currentCharacter.Level}";
        characterHpText.text = currentCharacter.Hp.ToString();
        characterAbilityText.text = currentCharacter.Ability;

        // **이미지 로드 (경로 확인 및 디버깅 추가)**
        string imagePath = currentCharacter.Image?.Trim().Replace(".png", ""); // 널 체크
        if (string.IsNullOrEmpty(imagePath))
        {
            Debug.LogError($"[CharacterSelectionUI] 이미지 경로가 비어있습니다. ({currentCharacter.Name})");
            return;
        }

        characterImage.sprite = Resources.Load<Sprite>(imagePath);

        if (characterImage.sprite == null)
        {
            Debug.LogError($"[CharacterSelectionUI] 스프라이트 로드 실패: {imagePath}");
        }

        upgradeCostText.text = $"{currentCharacter.UpgradeCost:N0}";
        equipButtonText.text = (GameData.characterName == currentCharacter.Name) ? "착용중" : "착용";
    }

}
