using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "GameData.json");

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

    public static int bestScore = 0; // 최고 점수
    public static int coin = 0; // 누적 코인
    public static int crystal = 0; // 누적 코인


    public static EquipmentSlotData[] equippedSlots = new EquipmentSlotData[3];

    // DTO(Data Transfer Object) 역할을 하는 내부 클래스
    [Serializable]
    private class GameDataDTO
    {
        public int characterId;
        public string characterName;
        public int characterLevel;
        public int characterHp;
        public int characterCost;
        public int characterUpgradeCost;
        public int characterExplain_ID;
        public int characterValue;
        public string characterAbility;
        public string characterImage;
        public int bestScore;
        public int coin;
        public int crystal;
        public EquipmentSlotData[] equippedSlots;
    }

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
        if (slotIndex < 0 || slotIndex >= equippedSlots.Length)
        {
            return;
        }
        equippedSlots[slotIndex] = slotData;
        SaveGameData();
    }

    public static EquipmentSlotData GetEquipmentSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= equippedSlots.Length)
        {
            return new EquipmentSlotData(0, "", 0, 0f, 0f, "");
        }

        return equippedSlots[slotIndex];
    }

    public static void SaveGameData()
    {
        try
        {
            GameDataDTO dto = new GameDataDTO
            {
                characterId = characterId,
                characterName = characterName,
                characterLevel = characterLevel,
                characterHp = characterHp,
                characterCost = characterCost,
                characterUpgradeCost = characterUpgradeCost,
                characterExplain_ID = characterExplain_ID,
                characterValue = characterValue,
                characterAbility = characterAbility,
                characterImage = characterImage,
                equippedSlots = equippedSlots,
                bestScore = bestScore,
                coin = coin,
                crystal = crystal
            };

            string json = JsonUtility.ToJson(dto, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"게임 데이터 저장 완료: {savePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"게임 데이터 저장 중 오류 발생: {ex.Message}");
        }

        SaveGameDataToCSV();
    }

    public static void SaveGameDataToCSV()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "Save");
        string filePath = Path.Combine(directoryPath, "GameData.csv");

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
            Debug.LogError("CSV 저장 중 오류 발생: " + ex.Message);
        }
    }

    public static void LoadGameData()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                GameDataDTO dto = JsonUtility.FromJson<GameDataDTO>(json);

                characterId = dto.characterId;
                characterName = dto.characterName;
                characterLevel = dto.characterLevel > 0 ? dto.characterLevel : 1;
                characterHp = dto.characterHp;
                characterCost = dto.characterCost;
                characterUpgradeCost = dto.characterUpgradeCost;
                characterExplain_ID = dto.characterExplain_ID;
                characterValue = dto.characterValue;
                characterAbility = dto.characterAbility;
                characterImage = dto.characterImage;
                equippedSlots = dto.equippedSlots;
                bestScore = dto.bestScore;
                coin = dto.coin;
                crystal = dto.crystal;

                Debug.Log("게임 데이터 불러오기 완료.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"게임 데이터 불러오기 오류: {ex.Message}");
            }
        }

        if (string.IsNullOrEmpty(characterName))
        {
            Debug.Log("[GameData] 저장된 캐릭터 데이터 없음, 기본 캐릭터 설정");
            SetDefaultCharacter();
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
        bestScore = 0;
        coin = 1000;
        crystal = 0;
        equippedSlots = new EquipmentSlotData[3];

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
            bestScore = 0;
            coin = 1000;
            crystal = 0;
        }
        else
        {
            Debug.LogError("[GameData] 기본 캐릭터 데이터를 찾을 수 없습니다! (용기쿠키 레벨 1)");
        }

        SaveGameData();
    }

    public static EquipmentSlotData ConvertTreasureToSlotData(Treasure treasure)
    {
        return new EquipmentSlotData(
            treasure.treasureID,
            treasure.treasureName,
            treasure.treasureVelue,
            treasure.treasureRadius,
            treasure.treasureSpeed,
            treasure.treasurePath
        );
    }

    public static void UpdateBestScore(int newScore)
    {
        if (newScore > bestScore)
        {
            bestScore = newScore;
            Debug.Log($"[GameData] 새로운 최고 점수: {bestScore}");
            SaveGameData(); // 새로운 최고 점수 저장
        }
    }

    public static void AddCoin(int amount)
    {
        coin += amount;
        Debug.Log($"[GameData] 코인 추가: {amount}, 현재 코인: {coin}");
        SaveGameData(); // 코인 저장
    }

    public static void AddCrystal(int amount)
    {
        crystal += amount;
        Debug.Log($"[GameData] 코인 추가: {amount}, 현재 코인: {crystal}");
        SaveGameData(); // 코인 저장
    }

    public static bool SpendCoin(int amount)
    {
        if (coin >= amount)
        {
            coin -= amount;
            Debug.Log($"[GameData] 코인 사용: {amount}, 남은 코인: {coin}");
            SaveGameData(); // 코인 저장
            return true;
        }
        else
        {
            Debug.LogWarning("[GameData] 코인이 부족합니다!");
            return false;
        }
    }
}