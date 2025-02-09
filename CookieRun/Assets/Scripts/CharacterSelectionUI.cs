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

        // `GameData`에서 불러온 값 적용
        characterInfo.currentCharacterName = GameData.EquippedCharacterName;
        characterInfo.currentCharacterLevel = GameData.EquippedCharacterLevel;

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

    private void InitializeCharacterSelection()
    {
        // 게임 데이터에서 현재 착용 중인 캐릭터 불러오기
        characterInfo.currentCharacterName = GameData.EquippedCharacterName;
        characterInfo.currentCharacterLevel = GameData.EquippedCharacterLevel;

        // 만약 데이터가 없거나 레벨이 0이면 기본값으로 설정
        if (string.IsNullOrEmpty(characterInfo.currentCharacterName))
        {
            characterInfo.currentCharacterName = characterList[0].Name;
        }

        if (characterInfo.currentCharacterLevel <= 0)
        {
            characterInfo.currentCharacterLevel = 1;
            Debug.LogWarning("[CharacterSelectionUI] 저장된 레벨이 0 이하 → 기본값 1로 변경");
        }
    }

    private void GetNextCharacter()
    {
        int index = characterList.FindIndex(c => c.Name == characterInfo.currentCharacterName);
        if (index != -1)
        {
            index = (index + 1) % characterList.Count;
            characterInfo.currentCharacterName = characterList[index].Name;

            // 기존 업그레이드된 레벨 유지
            characterInfo.currentCharacterLevel = GameData.GetCharacterLevel(characterInfo.currentCharacterName);
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

            // 기존 업그레이드된 레벨 유지
            characterInfo.currentCharacterLevel = GameData.GetCharacterLevel(characterInfo.currentCharacterName);
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
        GameData.EquipCharacter(selectedCharacter.Name, characterInfo.currentCharacterLevel);

        Debug.Log($"[CharacterSelectionUI] {selectedCharacter.Name} (레벨 {characterInfo.currentCharacterLevel}) 착용 완료.");

        UpdateCharacterUI();
    }

    private void UpdateCharacterUI()
    {
        var currentCharacter = CharacterTableObjectSC.GetCharacterData(characterInfo.currentCharacterName, characterInfo.currentCharacterLevel);
        if (currentCharacter == null)
        {
            Debug.LogError($"[CharacterSelectionUI] {characterInfo.currentCharacterName} (레벨 {characterInfo.currentCharacterLevel}) 데이터를 찾을 수 없습니다.");
            return;
        }

        characterNameText.text = currentCharacter.Name;
        characterLevelText.text = $"레벨 {currentCharacter.Level}";

        // 이미지 로드 (경로 정리)
        string imagePath = currentCharacter.Image.Trim().Replace(".png", "");
        characterImage.sprite = Resources.Load<Sprite>(imagePath);

        if (characterImage.sprite == null)
        {
            Debug.LogError($"[CharacterSelectionUI] 스프라이트 로드 실패: {imagePath}");
        }

        upgradeCostText.text = $"{currentCharacter.UpgradeCost:N0}";
        equipButtonText.text = (GameData.EquippedCharacterName == currentCharacter.Name) ? "착용중" : "착용";
    }
}
