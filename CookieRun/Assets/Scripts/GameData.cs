using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameData
{
    [Serializable]
    public struct EquipmentSlotData
    {
        public string treasureID;
        public string treasureName;
        public int treasureValue;
        public float treasureRadius;
        public float treasureSpeed;
        public string treasurePath;

        public EquipmentSlotData(string id, string name, int value, float radius, float speed, string path)
        {
            treasureID = id;
            treasureName = name;
            treasureValue = value;
            treasureRadius = radius;
            treasureSpeed = speed;
            treasurePath = path;
        }
    }

    // 캐릭터 정보 저장 변수
    public static string characterId { get; private set; } = "1";
    public static string characterName { get; private set; } = "BraveCookie";
    public static int characterLevel { get; private set; } = 1;
    public static int characterHp { get; private set; } = 100;
    public static int characterCost { get; private set; } = 1000;
    public static int characterUpgradeCost { get; private set; } = 500;
    public static int characterExplain_ID { get; private set; } = 1;
    public static int characterValue { get; private set; } = 10;
    public static string characterImage { get; private set; } = "Character/BraveCookie";

    // 장비 슬롯 정보 (3개)
    private static EquipmentSlotData[] equippedSlots = new EquipmentSlotData[3];

    // 기타 게임 데이터
    public static int BestScore { get; private set; } = 0;
    public static int Coin { get; private set; } = 1000;
    public static int Gem { get; private set; } = 1000;
    public static bool IsDataLoaded { get; private set; } = false;

    /*** 캐릭터 정보 저장 및 불러오기 ***/
    public static void EquipCharacter(
        string id, string name, int level, int hp, int cost, int upgradeCost, int explainID, int value, string image)
    {
        characterId = id;
        characterName = name;
        characterLevel = level;
        characterHp = hp;
        characterCost = cost;
        characterUpgradeCost = upgradeCost;
        characterExplain_ID = explainID;
        characterValue = value;
        characterImage = image;
        SaveGameData();
    }

    /*** 장비 슬롯 저장 및 불러오기 ***/
    public static void SetEquipmentSlot(int slotIndex, EquipmentSlotData data)
    {
        if (slotIndex < 0 || slotIndex >= equippedSlots.Length) return;
        equippedSlots[slotIndex] = data;
        SaveGameData();
    }

    public static EquipmentSlotData GetEquipmentSlot(int slotIndex)
    {
        return (slotIndex >= 0 && slotIndex < equippedSlots.Length) ? equippedSlots[slotIndex] : new EquipmentSlotData("None", "", 0, 0f, 0f, "");
    }

    /*** 게임 데이터 저장 ***/
    public static void SaveGameData()
    {
        PlayerPrefs.SetString("Character_ID", characterId);
        PlayerPrefs.SetString("Character_Name", characterName);
        PlayerPrefs.SetInt("Character_Level", characterLevel);
        PlayerPrefs.SetInt("Character_Hp", characterHp);
        PlayerPrefs.SetInt("Character_Cost", characterCost);
        PlayerPrefs.SetInt("Character_UpgradeCost", characterUpgradeCost);
        PlayerPrefs.SetInt("Character_Explain_ID", characterExplain_ID);
        PlayerPrefs.SetInt("Character_Value", characterValue);
        PlayerPrefs.SetString("Character_Image", characterImage);

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            PlayerPrefs.SetString($"EquipmentSlot_{i}_ID", equippedSlots[i].treasureID);
            PlayerPrefs.SetString($"EquipmentSlot_{i}_Name", equippedSlots[i].treasureName);
            PlayerPrefs.SetInt($"EquipmentSlot_{i}_Value", equippedSlots[i].treasureValue);
            PlayerPrefs.SetFloat($"EquipmentSlot_{i}_Radius", equippedSlots[i].treasureRadius);
            PlayerPrefs.SetFloat($"EquipmentSlot_{i}_Speed", equippedSlots[i].treasureSpeed);
            PlayerPrefs.SetString($"EquipmentSlot_{i}_Path", equippedSlots[i].treasurePath);
        }

        PlayerPrefs.SetInt("BestScore", BestScore);
        PlayerPrefs.SetInt("Coin", Coin);
        PlayerPrefs.SetInt("Gem", Gem);
        PlayerPrefs.Save();

        SaveGameDataToCSV();
    }

    /*** CSV 파일 저장 ***/
    public static void SaveGameDataToCSV()
    {
        string savePath = Application.persistentDataPath + "/Save/GameData.csv";
        List<string> csvLines = new List<string>
        {
            "CharacterID,CharacterName,CharacterLevel,CharacterHp,CharacterCost,CharacterUpgradeCost,CharacterExplainID,CharacterValue,CharacterImage",
            $"{characterId},{characterName},{characterLevel},{characterHp},{characterCost},{characterUpgradeCost},{characterExplain_ID},{characterValue},{characterImage}",

            "BestScore,Coin,Gem",
            $"{BestScore},{Coin},{Gem}",

            "EquipmentSlot1,EquipmentSlot2,EquipmentSlot3",
            $"{equippedSlots[0].treasureID},{equippedSlots[1].treasureID},{equippedSlots[2].treasureID}"
        };

        File.WriteAllLines(savePath, csvLines);
    }

    /*** 게임 데이터 불러오기 ***/
    public static void LoadGameData()
    {
        characterId = PlayerPrefs.GetString("Character_ID", "1");
        characterName = PlayerPrefs.GetString("Character_Name", "BraveCookie");
        characterLevel = PlayerPrefs.GetInt("Character_Level", 1);
        characterHp = PlayerPrefs.GetInt("Character_Hp", 100);
        characterCost = PlayerPrefs.GetInt("Character_Cost", 1000);
        characterUpgradeCost = PlayerPrefs.GetInt("Character_UpgradeCost", 500);
        characterExplain_ID = PlayerPrefs.GetInt("Character_Explain_ID", 1);
        characterValue = PlayerPrefs.GetInt("Character_Value", 10);
        characterImage = PlayerPrefs.GetString("Character_Image", "Character/BraveCookie");

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            equippedSlots[i] = new EquipmentSlotData(
                PlayerPrefs.GetString($"EquipmentSlot_{i}_ID", "None"),
                PlayerPrefs.GetString($"EquipmentSlot_{i}_Name", ""),
                PlayerPrefs.GetInt($"EquipmentSlot_{i}_Value", 0),
                PlayerPrefs.GetFloat($"EquipmentSlot_{i}_Radius", 0f),
                PlayerPrefs.GetFloat($"EquipmentSlot_{i}_Speed", 0f),
                PlayerPrefs.GetString($"EquipmentSlot_{i}_Path", "")
            );
        }

        IsDataLoaded = true;
    }
}
