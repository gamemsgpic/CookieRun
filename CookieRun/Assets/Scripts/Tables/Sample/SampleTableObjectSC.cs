using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SampleTableObjectSC : MonoBehaviour
{
    Dictionary<int, SampleTable> tables;
    public int Id = 100;
    public string Name;
    public int Score;
    public int Coin;
    public string Path;

    private void Start()
    {
        if (tables == null) Init();
        ApplyTableData(Id);
    }

    private void OnValidate()
    {
        if (tables == null) Init();
        ApplyTableData(Id);

        // 부모(ApplyItemMagnet)의 SetItem()을 호출하여 즉시 데이터 적용
        ApplyItemMagnet parent = GetComponentInParent<ApplyItemMagnet>();
        if (parent != null)
        {
            parent.SetItem();
        }
    }

    public void Init()
    {
        if (tables != null) return;

        tables = new Dictionary<int, SampleTable>();
        var records = CsvManager.Load<SampleTable>("SampleTable");

        if (records == null || !records.Any())
        {
            Debug.LogError("[TableObjectSC] CSV 파일을 찾을 수 없거나 비어 있습니다.");
            return;
        }

        List<SampleTable> loadedTables = records.ToList();
        for (int i = 0; i < loadedTables.Count; ++i)
        {
            int getID = loadedTables[i].Id;
            tables[getID] = loadedTables[i];
        }

        ApplyItemMagnet parent = GetComponentInParent<ApplyItemMagnet>();
        if (parent != null)
        {
            parent.SetItem();
        }
    }

    private void ApplyTableData(int id)
    {
        if (!tables.ContainsKey(id)) return;

        Name = tables[id].Name;
        Score = tables[id].Score;
        Coin = tables[id].Coin;
        Path = tables[id].Path;
    }
}
