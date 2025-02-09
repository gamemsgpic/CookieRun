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
            Debug.LogError("[CharacterInfo] CSV �����͸� �ҷ��� �� ���ų� ��� �ֽ��ϴ�.");
            return;
        }

        Debug.Log($"[CharacterInfo] CSV ������ �ε� �Ϸ�: {records.Count()} ���� ĳ����");

        foreach (var record in records)
        {
            if (record.Level == 1)
                characterBaseTable[record.Name] = record; // ���� 1 ĳ���͸� ����
        }
    }

    public static List<CharacterTable> GetBaseCharacters()
    {
        if (characterBaseTable == null || characterBaseTable.Count == 0)
        {
            Debug.LogWarning("[CharacterTableObjectSC] �⺻ ĳ���� ���̺��� �ʱ�ȭ���� �ʾ� �ʱ�ȭ �õ�.");
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
