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
        if (tables == null) Init(); // tables가 null이면 강제로 초기화
        ApplyTableData(Id);
    }

    private void OnValidate()
    {
        if (tables == null) Init(); // tables가 null이면 강제로 초기화
        ApplyTableData(Id);
    }

    public void Init()
    {
        if (tables != null) return; // 이미 초기화되었으면 건너뜀

        tables = new Dictionary<int, SampleTable>();
        var records = CsvManager.Load<SampleTable>("SampleTable"); // 확장자 없이 로드

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
