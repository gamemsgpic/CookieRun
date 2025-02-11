//using System;
//using System.IO;
//using System.Collections.Generic;
//using UnityEngine;

//[Serializable]
//public static class GameData
//{
//    [Serializable]
//    public struct EquipmentSlotData
//    {
//        public int treasureID;
//        public string treasureName;
//        public int treasureVelue;
//        public float treasureRadius;
//        public float treasureSpeed;
//        public string treasurePath;

//        public EquipmentSlotData(int id, string name, int velue, float radius, float speed, string path)
//        {
//            treasureID = id;
//            treasureName = name;
//            treasureVelue = velue;
//            treasureRadius = radius;
//            treasureSpeed = speed;
//            treasurePath = path;
//        }
//    }

//    public static int characterId;
//    public static string characterName;
//    public static int characterLevel;
//    public static int characterHp;
//    public static int characterCost;
//    public static int characterUpgradeCost;
//    public static int characterExplain_ID;
//    public static int characterValue;
//    public static string characterAbility;
//    public static string characterImage;

//    public static EquipmentSlotData[] equippedSlots = new EquipmentSlotData[3];

//    public static void EquipCharacter(int id, string name, int level, int hp, int cost, int upgradeCost, int explainID, int value, string ability, string image)
//    {
//        characterId = id;
//        characterName = name;
//        characterLevel = level;
//        characterHp = hp;
//        characterCost = cost;
//        characterUpgradeCost = upgradeCost;
//        characterExplain_ID = explainID;
//        characterValue = value;
//        characterAbility = ability;
//        characterImage = image;
//        SaveGameData();
//    }

//    public static void SetEquipmentSlot(int slotIndex, EquipmentSlotData slotData)
//    {
//        if (slotIndex < 0 || slotIndex >= equippedSlots.Length) return;
//        equippedSlots[slotIndex] = slotData;
//        SaveGameData();
//    }

//    public static EquipmentSlotData GetEquipmentSlot(int slotIndex)
//    {
//        if (slotIndex < 0 || slotIndex >= equippedSlots.Length)
//            return new EquipmentSlotData(0, "", 0, 0f, 0f, "");

//        return equippedSlots[slotIndex];
//    }


//    public static void SaveGameData()
//    {
//        PlayerPrefs.SetInt("CharacterId", characterId);
//        PlayerPrefs.SetString("CharacterName", characterName);
//        PlayerPrefs.SetInt("CharacterLevel", characterLevel);
//        PlayerPrefs.SetInt("CharacterHp", characterHp);
//        PlayerPrefs.SetInt("CharacterCost", characterCost);
//        PlayerPrefs.SetInt("CharacterUpgradeCost", characterUpgradeCost);
//        PlayerPrefs.SetInt("CharacterExplainID", characterExplain_ID);
//        PlayerPrefs.SetInt("CharacterValue", characterValue);
//        PlayerPrefs.SetString("CharacterAbility", characterAbility);
//        PlayerPrefs.SetString("CharacterImage", characterImage);

//        for (int i = 0; i < equippedSlots.Length; i++)
//        {
//            PlayerPrefs.SetInt($"EquipmentSlot_{i}_ID", equippedSlots[i].treasureID);
//            PlayerPrefs.SetString($"EquipmentSlot_{i}_Name", equippedSlots[i].treasureName);
//            PlayerPrefs.SetInt($"EquipmentSlot_{i}_Value", equippedSlots[i].treasureVelue);
//            PlayerPrefs.SetFloat($"EquipmentSlot_{i}_Radius", equippedSlots[i].treasureRadius);
//            PlayerPrefs.SetFloat($"EquipmentSlot_{i}_Speed", equippedSlots[i].treasureSpeed);
//            PlayerPrefs.SetString($"EquipmentSlot_{i}_Path", equippedSlots[i].treasurePath);
//        }

//        PlayerPrefs.Save();
//        SaveGameDataToCSV();
//    }

//    public static void SaveGameDataToCSV()
//    {
//        string directoryPath = Path.Combine(Application.persistentDataPath, "Save");
//        string filePath = Path.Combine(directoryPath, "GameData.csv");

//        // ���͸� Ȯ�� �� ����
//        if (!Directory.Exists(directoryPath))
//        {
//            Directory.CreateDirectory(directoryPath);
//        }

//        try
//        {
//            List<string> csvLines = new List<string>
//        {
//            "CharacterId,CharacterName,CharacterLevel,CharacterHp,CharacterCost,CharacterUpgradeCost,CharacterExplainID,CharacterValue,CharacterImage",
//            $"{characterId},{characterName},{characterLevel},{characterHp},{characterCost},{characterUpgradeCost},{characterExplain_ID},{characterValue},{characterImage}"
//        };

//            // ��� ���� ������ �߰� (��� ���� ���� Ȯ�� �� �߰�)
//            if (equippedSlots.Length >= 3)
//            {
//                csvLines.Add("EquipmentSlot1,EquipmentSlot2,EquipmentSlot3");
//                csvLines.Add(
//                    $"{equippedSlots[0].treasureID},{equippedSlots[1].treasureID},{equippedSlots[2].treasureID}"
//                );
//            }
//            else
//            {
//                Debug.LogError("��� ���� ������ �����մϴ�.");
//            }

//            File.WriteAllLines(filePath, csvLines);
//        }
//        catch (Exception ex)
//        {
//            Debug.LogError("���� ���� �� ���� �߻�: " + ex.Message);
//        }
//    }

//    public static void LoadGameData()
//    {
//        if (PlayerPrefs.HasKey("CharacterId"))
//        {
//            characterId = PlayerPrefs.GetInt("CharacterId");
//        }

//        if (PlayerPrefs.HasKey("CharacterName"))
//        {
//            characterName = PlayerPrefs.GetString("CharacterName");
//        }

//        if (PlayerPrefs.HasKey("CharacterLevel")) // �����Ͱ� ���� ���� �ҷ�����
//        {
//            characterLevel = PlayerPrefs.GetInt("CharacterLevel");
//        }

//        if (PlayerPrefs.HasKey("CharacterHp"))
//        {
//            characterHp = PlayerPrefs.GetInt("CharacterHp");
//        }

//        if (PlayerPrefs.HasKey("CharacterCost"))
//        {
//            characterCost = PlayerPrefs.GetInt("CharacterCost");
//        }

//        if (PlayerPrefs.HasKey("CharacterUpgradeCost"))
//        {
//            characterUpgradeCost = PlayerPrefs.GetInt("CharacterUpgradeCost");
//        }

//        if (PlayerPrefs.HasKey("CharacterExplainID"))
//        {
//            characterExplain_ID = PlayerPrefs.GetInt("CharacterExplainID");
//        }

//        if (PlayerPrefs.HasKey("CharacterValue"))
//        {
//            characterValue = PlayerPrefs.GetInt("CharacterValue");
//        }

//        if (PlayerPrefs.HasKey("CharacterAbility"))
//        {
//            characterAbility = PlayerPrefs.GetString("CharacterAbility");
//        }

//        if (PlayerPrefs.HasKey("CharacterImage"))
//        {
//            characterImage = PlayerPrefs.GetString("CharacterImage");
//        }

//        // **�����Ͱ� ��� ������ �⺻ ĳ����("�����Ű") ����**
//        if (string.IsNullOrEmpty(characterName))
//        {
//            SetDefaultCharacter();
//        }

//        for (int i = 0; i < equippedSlots.Length; i++)
//        {
//            if (PlayerPrefs.HasKey($"EquipmentSlot_{i}_ID"))
//            {
//                equippedSlots[i].treasureID = PlayerPrefs.GetInt($"EquipmentSlot_{i}_ID");
//            }

//            if (PlayerPrefs.HasKey($"EquipmentSlot_{i}_Name"))
//            {
//                equippedSlots[i].treasureName = PlayerPrefs.GetString($"EquipmentSlot_{i}_Name");
//            }

//            if (PlayerPrefs.HasKey($"EquipmentSlot_{i}_Value"))
//            {
//                equippedSlots[i].treasureVelue = PlayerPrefs.GetInt($"EquipmentSlot_{i}_Value");
//            }

//            if (PlayerPrefs.HasKey($"EquipmentSlot_{i}_Radius"))
//            {
//                equippedSlots[i].treasureRadius = PlayerPrefs.GetFloat($"EquipmentSlot_{i}_Radius");
//            }

//            if (PlayerPrefs.HasKey($"EquipmentSlot_{i}_Speed"))
//            {
//                equippedSlots[i].treasureSpeed = PlayerPrefs.GetFloat($"EquipmentSlot_{i}_Speed");
//            }

//            if (PlayerPrefs.HasKey($"EquipmentSlot_{i}_Path"))
//            {
//                equippedSlots[i].treasurePath = PlayerPrefs.GetString($"EquipmentSlot_{i}_Path");
//            }
//        }
//    }

//    public static void ResetSaveGameData()
//    {
//        characterId = 0;
//        characterName = "";
//        characterLevel = 1;
//        characterHp = 100;
//        characterCost = 0;
//        characterUpgradeCost = 0;
//        characterExplain_ID = 0;
//        characterValue = 0;
//        characterAbility = "";
//        characterImage = "";

//        for (int i = 0; i < equippedSlots.Length; i++)
//        {
//            equippedSlots[i] = new EquipmentSlotData(0, "", 0, 0f, 0f, "");
//        }

//        SaveGameData();
//    }


//    public static void SetDefaultCharacter()
//    {
//        var defaultCharacter = CharacterTableObjectSC.GetCharacterData("BraveCookie", 1);

//        if (defaultCharacter != null)
//        {
//            characterId = defaultCharacter.ID;
//            characterName = defaultCharacter.Name;
//            characterLevel = defaultCharacter.Level;
//            characterHp = defaultCharacter.Hp;
//            characterCost = defaultCharacter.Cost;
//            characterUpgradeCost = defaultCharacter.UpgradeCost;
//            characterExplain_ID = defaultCharacter.Explain_ID;
//            characterValue = defaultCharacter.Value;
//            characterAbility = defaultCharacter.Ability;
//            characterImage = defaultCharacter.Image;
//        }
//        else
//        {
//            Debug.LogError("[GameData] �⺻ ĳ���� �����͸� ã�� �� �����ϴ�! (�����Ű ���� 1)");
//        }

//        SaveGameData();
//    }

//    //public static void SetCurrenterState(int id, string name, int level, int hp, int cost, int upgradecost, int explainid, int velue, string avility, string image)
//    //{
//    //
//    //
//    //}
//}


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

    public static int bestScore = 0; // �ְ� ����
    public static int coin = 0; // ���� ����


    public static EquipmentSlotData[] equippedSlots = new EquipmentSlotData[3];

    // DTO(Data Transfer Object) ������ �ϴ� ���� Ŭ����
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
                coin = coin
            };

            string json = JsonUtility.ToJson(dto, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"���� ������ ���� �Ϸ�: {savePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"���� ������ ���� �� ���� �߻�: {ex.Message}");
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
                Debug.LogError("��� ���� ������ �����մϴ�.");
            }

            File.WriteAllLines(filePath, csvLines);
        }
        catch (Exception ex)
        {
            Debug.LogError("CSV ���� �� ���� �߻�: " + ex.Message);
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

                Debug.Log("���� ������ �ҷ����� �Ϸ�.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"���� ������ �ҷ����� ����: {ex.Message}");
            }
        }

        if (string.IsNullOrEmpty(characterName))
        {
            Debug.Log("[GameData] ����� ĳ���� ������ ����, �⺻ ĳ���� ����");
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
        }
        else
        {
            Debug.LogError("[GameData] �⺻ ĳ���� �����͸� ã�� �� �����ϴ�! (�����Ű ���� 1)");
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
            Debug.Log($"[GameData] ���ο� �ְ� ����: {bestScore}");
            SaveGameData(); // ���ο� �ְ� ���� ����
        }
    }

    public static void AddCoin(int amount)
    {
        coin += amount;
        Debug.Log($"[GameData] ���� �߰�: {amount}, ���� ����: {coin}");
        SaveGameData(); // ���� ����
    }

    public static bool SpendCoin(int amount)
    {
        if (coin >= amount)
        {
            coin -= amount;
            Debug.Log($"[GameData] ���� ���: {amount}, ���� ����: {coin}");
            SaveGameData(); // ���� ����
            return true;
        }
        else
        {
            Debug.LogWarning("[GameData] ������ �����մϴ�!");
            return false;
        }
    }
}
