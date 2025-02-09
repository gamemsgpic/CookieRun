using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterTableObjectSC : MonoBehaviour
{
    private static Dictionary<string, CharacterTable> characterBaseTable;
    private static CharacterTableObjectSC instance;

    public string Name;
    public int Level;
    public int Id;
    public int Hp;
    public int Cost;
    public int UpgradeCost;
    public int Explain_ID;
    public int Value;
    public string Image;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (characterBaseTable == null) Init();
    }

    public static void Init()
    {
        if (characterBaseTable != null) return;

        characterBaseTable = new Dictionary<string, CharacterTable>();
        var records = CsvManager.Load<CharacterTable>("CharacterTable");

        if (records == null || !records.Any())
        {
            Debug.LogError("[CharacterInfo] CSV 데이터를 불러올 수 없거나 비어 있습니다.");
            return;
        }

        Debug.Log($"[CharacterInfo] CSV 데이터 로드 완료: {records.Count()} 개의 캐릭터");

        foreach (var record in records)
        {
            if (record.Level == 1)
                characterBaseTable[record.Name] = record; // 레벨 1 캐릭터만 저장
        }
    }

    public static List<CharacterTable> GetBaseCharacters()
    {
        if (characterBaseTable == null || characterBaseTable.Count == 0)
        {
            Debug.LogWarning("[CharacterTableObjectSC] 기본 캐릭터 테이블이 초기화되지 않아 초기화 시도.");
            Init();
        }

        return characterBaseTable.Values.ToList();
    }

    public static CharacterTable GetCharacterData(string name, int level)
    {
        var allCharacters = CsvManager.Load<CharacterTable>("CharacterTable");
        return allCharacters.FirstOrDefault(c => c.Name == name && c.Level == level);
    }
}
