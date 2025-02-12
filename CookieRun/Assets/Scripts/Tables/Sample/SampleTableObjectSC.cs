using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SampleTableObjectSC : MonoBehaviour
{
    Dictionary<int, SampleTable> tables;
    public int ID = 100;
    public string Name;
    public int Score;
    public int Coin;
    public int Path;
    public int Heal;
    public int Value;

    private void Start()
    {
        if (tables == null) Init();
        ApplyTableData(ID);
    }

    private void OnValidate()
    {
        if (tables == null) Init();
        ApplyTableData(ID);

        // �θ�(ApplyItemMagnet)�� SetItem()�� ȣ���Ͽ� ��� ������ ����
        ApplyItemMagnet parent = GetComponentInParent<ApplyItemMagnet>();
        if (parent != null)
        {
            //parent.SetItem();
        }
    }

    public void Init()
    {
        if (tables != null) return;

        tables = new Dictionary<int, SampleTable>();
        var records = CsvManager.Load<SampleTable>("SampleTable");

        if (records == null || !records.Any())
        {
            Debug.LogError("[TableObjectSC] CSV ������ ã�� �� ���ų� ��� �ֽ��ϴ�.");
            return;
        }

        List<SampleTable> loadedTables = records.ToList();
        for (int i = 0; i < loadedTables.Count; ++i)
        {
            int getID = loadedTables[i].ID;
            tables[getID] = loadedTables[i];
        }

        ApplyItemMagnet parent = GetComponentInParent<ApplyItemMagnet>();
        if (parent != null)
        {
            //parent.SetItem();
        }
    }

    private void ApplyTableData(int id)
    {
        if (!tables.ContainsKey(id)) return;

        Name = tables[id].Name;
        Score = tables[id].Score;
        Coin = tables[id].Coin;
        Path = tables[id].Path;
        Heal = tables[id].Heal;
        Value = tables[id].Value;
    }
}
