using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    [Serializable]
    public struct EquipmentSlotData
    {
        public int treasureID;
        public string treasureName;
        public int treasureVelue;
        public float treasureRadius;
        public float treasureSpeed;
        public string treasurePath;

        public EquipmentSlotData(int id, string name, int velue, float radius, float speed, string path)
        {
            treasureID = id;
            treasureName = name;
            treasureVelue = velue;
            treasureRadius = radius;
            treasureSpeed = speed;
            treasurePath = path;
        }
    }

    public static int characterId;
    public static string characterName;
    public static int characterLevel;
    public static int characterHp;
    public static int characterCost;
    public static int characterUpgradeCost;
    public static int characterExplain_ID;
    public static int characterValue;
    public static string characterAbility;
    public static string characterImage;

    public static EquipmentSlotData[] equippedSlots = new EquipmentSlotData[3];

    public static void EquipCharacter(int id, string name, int level, int hp, int cost, int upgradeCost, int explainID, int value, string ability, string image)
    {
        characterId = id;
        characterName = name;
        characterLevel = level;
        characterHp = hp;
        characterCost = cost;
        characterUpgradeCost = upgradeCost;
        characterExplain_ID = explainID;
        characterValue = value;
        characterAbility = ability;
        characterImage = image;
        SaveGameData();
    }

    public static void SetEquipmentSlot(int slotIndex, EquipmentSlotData slotData)
    {
        if (slotIndex < 0 || slotIndex >= equippedSlots.Length) return;
        equippedSlots[slotIndex] = slotData;
        SaveGameData();
    }

    public static EquipmentSlotData GetEquipmentSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= equippedSlots.Length)
            return new EquipmentSlotData(0, "", 0, 0f, 0f, "");

        return equippedSlots[slotIndex];
    }


    public static void SaveGameData()
    {
        PlayerPrefs.SetInt("CharacterId", characterId);
        PlayerPrefs.SetString("CharacterName", characterName);
        PlayerPrefs.SetInt("CharacterLevel", characterLevel);
        PlayerPrefs.SetInt("CharacterHp", characterHp);
        PlayerPrefs.SetInt("CharacterCost", characterCost);
        PlayerPrefs.SetInt("CharacterUpgradeCost", characterUpgradeCost);
        PlayerPrefs.SetInt("CharacterExplainID", characterExplain_ID);
        PlayerPrefs.SetInt("CharacterValue", characterValue);
        PlayerPrefs.SetString("CharacterAbility", characterAbility);
        PlayerPrefs.SetString("CharacterImage", characterImage);

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            PlayerPrefs.SetInt($"EquipmentSlot_{i}_ID", equippedSlots[i].treasureID);
            PlayerPrefs.SetString($"EquipmentSlot_{i}_Name", equippedSlots[i].treasureName);
            PlayerPrefs.SetInt($"EquipmentSlot_{i}_Value", equippedSlots[i].treasureVelue);
            PlayerPrefs.SetFloat($"EquipmentSlot_{i}_Radius", equippedSlots[i].treasureRadius);
            PlayerPrefs.SetFloat($"EquipmentSlot_{i}_Speed", equippedSlots[i].treasureSpeed);
            PlayerPrefs.SetString($"EquipmentSlot_{i}_Path", equippedSlots[i].treasurePath);
        }

        PlayerPrefs.Save();
        SaveGameDataToCSV();
    }

    public static void SaveGameDataToCSV()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "Save");
        string filePath = Path.Combine(directoryPath, "GameData.csv");

        // 디렉터리 확인 및 생성
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            List<string> csvLines = new List<string>
        {
            "CharacterId,CharacterName,CharacterLevel,CharacterHp,CharacterCost,CharacterUpgradeCost,CharacterExplainID,CharacterValue,CharacterImage",
            $"{characterId},{characterName},{characterLevel},{characterHp},{characterCost},{characterUpgradeCost},{characterExplain_ID},{characterValue},{characterImage}"
        };

            // 장비 슬롯 데이터 추가 (장비 슬롯 개수 확인 후 추가)
            if (equippedSlots.Length >= 3)
            {
                csvLines.Add("EquipmentSlot1,EquipmentSlot2,EquipmentSlot3");
                csvLines.Add(
                    $"{equippedSlots[0].treasureID},{equippedSlots[1].treasureID},{equippedSlots[2].treasureID}"
                );
            }
            else
            {
                Debug.LogError("장비 슬롯 개수가 부족합니다.");
            }

            File.WriteAllLines(filePath, csvLines);
        }
        catch (Exception ex)
        {
            Debug.LogError("파일 저장 중 오류 발생: " + ex.Message);
        }
    }

    public static void LoadGameData()
    {
        characterId = PlayerPrefs.GetInt("CharacterId", 0);
        characterName = PlayerPrefs.GetString("CharacterName", "");
        characterLevel = PlayerPrefs.GetInt("CharacterLevel", 1);
        characterHp = PlayerPrefs.GetInt("CharacterHp", 100);
        characterCost = PlayerPrefs.GetInt("CharacterCost", 0);
        characterUpgradeCost = PlayerPrefs.GetInt("CharacterUpgradeCost", 0);
        characterExplain_ID = PlayerPrefs.GetInt("CharacterExplainID", 0);
        characterValue = PlayerPrefs.GetInt("CharacterValue", 0);
        characterAbility = PlayerPrefs.GetString("CharacterAbility", "");
        characterImage = PlayerPrefs.GetString("CharacterImage", "");

        // **데이터가 비어 있으면 기본 캐릭터("용기쿠키") 설정**
        if (string.IsNullOrEmpty(characterName))
        {
            SetDefaultCharacter();
        }

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            equippedSlots[i].treasureID = PlayerPrefs.GetInt($"EquipmentSlot_{i}_ID", 0);
            equippedSlots[i].treasureName = PlayerPrefs.GetString($"EquipmentSlot_{i}_Name", "");
            equippedSlots[i].treasureVelue = PlayerPrefs.GetInt($"EquipmentSlot_{i}_Value", 0);
            equippedSlots[i].treasureRadius = PlayerPrefs.GetFloat($"EquipmentSlot_{i}_Radius", 0);
            equippedSlots[i].treasureSpeed = PlayerPrefs.GetFloat($"EquipmentSlot_{i}_Speed", 0);
            equippedSlots[i].treasurePath = PlayerPrefs.GetString($"EquipmentSlot_{i}_Path", "");
        }
    }

    public static void ResetSaveGameData()
    {
        characterId = 0;
        characterName = "";
        characterLevel = 1;
        characterHp = 100;
        characterCost = 0;
        characterUpgradeCost = 0;
        characterExplain_ID = 0;
        characterValue = 0;
        characterAbility = "";
        characterImage = "";

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            equippedSlots[i] = new EquipmentSlotData(0, "", 0, 0f, 0f, "");
        }

        SaveGameData();
    }


    public static void SetDefaultCharacter()
    {
        var defaultCharacter = CharacterTableObjectSC.GetCharacterData("BraveCookie", 1);

        if (defaultCharacter != null)
        {
            characterId = defaultCharacter.ID;
            characterName = defaultCharacter.Name;
            characterLevel = defaultCharacter.Level;
            characterHp = defaultCharacter.Hp;
            characterCost = defaultCharacter.Cost;
            characterUpgradeCost = defaultCharacter.UpgradeCost;
            characterExplain_ID = defaultCharacter.Explain_ID;
            characterValue = defaultCharacter.Value;
            characterAbility = defaultCharacter.Ability;
            characterImage = defaultCharacter.Image;
        }
        else
        {
            Debug.LogError("[GameData] 기본 캐릭터 데이터를 찾을 수 없습니다! (용기쿠키 레벨 1)");
        }

        SaveGameData();
    }
}
