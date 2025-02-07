using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableObjectSC : MonoBehaviour
{
    Dictionary<int, SampleTable> tables;
    public int Id = 100;
    public string Name;
    public int Score;
    public float Coin;
    public bool Flag;
    public string Path;

    private void Start()
    {
        if (tables == null) Init(); // tables�� null�̸� ������ �ʱ�ȭ
        ApplyTableData(Id);
    }

    private void OnValidate()
    {
        if (tables == null) Init(); // tables�� null�̸� ������ �ʱ�ȭ
        ApplyTableData(Id);
    }

    public void Init()
    {
        if (tables != null) return; // �̹� �ʱ�ȭ�Ǿ����� �ǳʶ�

        tables = new Dictionary<int, SampleTable>();
        var records = CsvManager.Load<SampleTable>("SampleTable"); // Ȯ���� ���� �ε�

        if (records == null || !records.Any())
        {
            Debug.LogError("[TableObjectSC] CSV ������ ã�� �� ���ų� ��� �ֽ��ϴ�.");
            return;
        }

        List<SampleTable> loadedTables = records.ToList();
        for (int i = 0; i < loadedTables.Count; ++i)
        {
            int getID = loadedTables[i].Id;
            tables[getID] = loadedTables[i];
        }
    }

    private void ApplyTableData(int id)
    {
        if (!tables.ContainsKey(id)) return;

        Name = tables[id].Name;
        Score = tables[id].Score;
        Coin = tables[id].Coin;
        Flag = tables[id].Flag;
        Path = tables[id].Path;
    }
}
