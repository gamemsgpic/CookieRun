using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    [Serializable]
    public struct EquipmentSlotData
    {
        public string ItemId;

        public EquipmentSlotData(string itemId)
        {
            ItemId = itemId;
        }
    }

    public static string EquippedCharacterName { get; private set; } = "BraveCookie";
    public static int EquippedCharacterLevel { get; private set; } = 1;
    public static Dictionary<string, int> CharacterLevels = new Dictionary<string, int>();
    public static int BestScore { get; private set; } = 0;
    public static int Coin { get; private set; } = 1000;
    public static int Gem { get; private set; } = 1000;
    public static bool IsDataLoaded { get; private set; } = false;

    private static EquipmentSlotData[] equippedSlots = new EquipmentSlotData[3];

    public static void EquipCharacter(string characterName, int level)
    {
        EquippedCharacterName = characterName;
        EquippedCharacterLevel = level;
        SetCharacterLevel(characterName, level);
        SaveGameData();
    }

    public static void SetCharacterLevel(string characterName, int level)
    {
        if (CharacterLevels.ContainsKey(characterName))
            CharacterLevels[characterName] = level;
        else
            CharacterLevels.Add(characterName, level);
    }

    public static int GetCharacterLevel(string characterName)
    {
        return CharacterLevels.ContainsKey(characterName) ? CharacterLevels[characterName] : 1;
    }

    public static void SetEquipmentSlot(int slotIndex, string itemId)
    {
        if (slotIndex < 0 || slotIndex >= equippedSlots.Length) return;
        equippedSlots[slotIndex] = new EquipmentSlotData(itemId);
        SaveGameData();
    }

    public static string GetEquipmentSlot(int slotIndex)
    {
        return (slotIndex >= 0 && slotIndex < equippedSlots.Length) ? equippedSlots[slotIndex].ItemId : "None";
    }

    public static void SaveGameData()
    {
        PlayerPrefs.SetString("EquippedCharacterName", EquippedCharacterName);
        PlayerPrefs.SetInt("EquippedCharacterLevel", EquippedCharacterLevel);

        foreach (var entry in CharacterLevels)
        {
            PlayerPrefs.SetInt($"CharacterLevel_{entry.Key}", entry.Value);
        }

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            PlayerPrefs.SetString($"EquipmentSlot_{i}", string.IsNullOrEmpty(equippedSlots[i].ItemId) ? "None" : equippedSlots[i].ItemId);
        }

        PlayerPrefs.SetInt("BestScore", BestScore);
        PlayerPrefs.SetInt("Coin", Coin);
        PlayerPrefs.SetInt("Gem", Gem);
        PlayerPrefs.Save();

        Debug.Log("[GameData] ������ ���� �Ϸ�.");

        SaveGameDataToCSV(); // CSV ���� �߰�
    }


    public static void SaveGameDataToCSV()
    {
        string savePath = Application.persistentDataPath + "/Save/GameData.csv";
        List<string> csvLines = new List<string>();

        // ĳ���� ���� ����
        csvLines.Add("EquippedCharacterName,EquippedCharacterLevel");
        csvLines.Add($"{EquippedCharacterName},{EquippedCharacterLevel}");

        // �� ĳ������ ���� ����
        foreach (var entry in CharacterLevels)
        {
            csvLines.Add($"{entry.Key},{entry.Value}");
        }

        // ���� �� �ڿ� ����
        csvLines.Add("BestScore,Coin,Gem");
        csvLines.Add($"{BestScore},{Coin},{Gem}");

        // ���� ���� ����
        csvLines.Add("EquipmentSlot1,EquipmentSlot2,EquipmentSlot3");
        csvLines.Add(
            $"{(string.IsNullOrEmpty(equippedSlots[0].ItemId) ? "None" : equippedSlots[0].ItemId)}," +
            $"{(string.IsNullOrEmpty(equippedSlots[1].ItemId) ? "None" : equippedSlots[1].ItemId)}," +
            $"{(string.IsNullOrEmpty(equippedSlots[2].ItemId) ? "None" : equippedSlots[2].ItemId)}"
        );

        try
        {
            File.WriteAllLines(savePath, csvLines);
            Debug.Log($"[GameData] CSV ���� �Ϸ�: {savePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GameData] CSV ���� �� ���� �߻�: {ex.Message}");
        }
    }




    public static void LoadGameData()
    {
        Debug.Log("[GameData] ����� �����͸� �ҷ����� ��...");

        LoadGameDataFromCSV(); // CSV ������ ���� �ε�

        EquippedCharacterName = PlayerPrefs.GetString("EquippedCharacterName", "BraveCookie");
        EquippedCharacterLevel = PlayerPrefs.GetInt("EquippedCharacterLevel", 1);

        string[] characterNames = { "BraveCookie", "AngelCookie", "ZombieCookie" };
        foreach (string name in characterNames)
        {
            int savedLevel = PlayerPrefs.GetInt($"CharacterLevel_{name}", -1);
            if (savedLevel >= 1)
                SetCharacterLevel(name, savedLevel);
            else
                SetCharacterLevel(name, 1);
        }

        for (int i = 0; i < equippedSlots.Length; i++)
            equippedSlots[i] = new EquipmentSlotData(PlayerPrefs.GetString($"EquipmentSlot_{i}", "None"));

        Debug.Log("[GameData] ��� ����� ������ �ε� �Ϸ�.");
        IsDataLoaded = true;
    }


    public static void LoadGameDataFromCSV()
    {
        string loadPath = Application.persistentDataPath + "/Save/GameData.csv";
        if (!File.Exists(loadPath))
        {
            Debug.LogWarning("[GameData] CSV ������ �������� ����. �� ������ �����մϴ�.");
            return;
        }

        string[] lines = File.ReadAllLines(loadPath);
        if (lines.Length < 2)
        {
            Debug.LogError("[GameData] CSV �����Ͱ� �ùٸ��� ����.");
            return;
        }

        // ĳ���� ������ �ε�
        string[] charData = lines[1].Split(',');
        if (charData.Length >= 2)
        {
            EquippedCharacterName = charData[0].Trim();
            EquippedCharacterLevel = int.TryParse(charData[1].Trim(), out int level) ? level : 1;
        }

        // ĳ���� ���� �ε�
        for (int i = 2; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            if (parts.Length == 2 && int.TryParse(parts[1], out int charLevel))
            {
                SetCharacterLevel(parts[0].Trim(), charLevel);
            }
        }

        // ���� �� �ڿ� �ε�
        if (lines.Length >= 6)
        {
            string[] scoreData = lines[5].Split(',');
            if (scoreData.Length == 3)
            {
                BestScore = int.TryParse(scoreData[0], out int bestScore) ? bestScore : 0;
                Coin = int.TryParse(scoreData[1], out int coin) ? coin : 1000;
                Gem = int.TryParse(scoreData[2], out int gem) ? gem : 1000;
            }
        }

        // ��� ���� �ε�
        if (lines.Length >= 8)
        {
            string[] slots = lines[7].Split(',');
            for (int i = 0; i < slots.Length && i < equippedSlots.Length; i++)
            {
                equippedSlots[i] = new EquipmentSlotData(slots[i].Trim() == "None" ? "" : slots[i].Trim());
            }
        }

        Debug.Log("[GameData] CSV ������ �ε� �Ϸ�.");
    }


}
