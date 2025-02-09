using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreasureTableTableObjectSC : MonoBehaviour
{
    Dictionary<int, TreasureTable> tables;
    public int Id = 100;
    public string Name;
    public int Velue;
    public float Radius;
    public float Speed;
    public string Path;

    private void Start()
    {
        if (tables == null) Init();
        ApplyTableData(Id);
    }

    private void Init()
    {
        if (tables != null) return;

        tables = new Dictionary<int, TreasureTable>();
        var records = CsvManager.Load<TreasureTable>("TreasureTable");

        if (records == null || !records.Any())
        {
            Debug.LogError("[TableObjectSC] CSV 파일을 찾을 수 없거나 비어 있습니다.");
            return;
        }

        List<TreasureTable> loadedTables = records.ToList();
        foreach (var table in loadedTables)
        {
            tables[table.Id] = table;
        }
    }

    private void ApplyTableData(int id)
    {
        if (!tables.ContainsKey(id)) return;

        Name = tables[id].Name;
        Velue = tables[id].Velue;
        Radius = tables[id].Radius;
        Speed = tables[id].Speed;
        Path = tables[id].Path;
    }
}
