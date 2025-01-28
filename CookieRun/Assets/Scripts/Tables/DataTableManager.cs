//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public static class DataTableManager
//{
//    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

//    static DataTableManager()
//    {
//        var itemTable = new ItemTable();
//        itemTable.Load(DataTableIds.Item);
//        tables.Add(DataTableIds.Item, itemTable);

//#if UNITY_EDITOR
//        foreach (var id in DataTableIds.String)
//        {
//            var table = new StringTable();
//            table.Load(id);
//            tables.Add(id, table);
//        }
//#else
//        var table = new StringTable();
//        var stringTableId = DataTableIds.String[(int)Variables.currentLang];
//        table.Load(stringTableId);
//        tables.Add(stringTableId, table);
//#endif

//    }

//    public static StringTable StringTable
//    {
//        get
//        {
//            return Get<StringTable>(DataTableIds.String[(int)Variables.currentLang]);
//        }
//    }

//    public static ItemTable ItemTable
//    {
//        get
//        {
//            return Get<ItemTable>(DataTableIds.Item);
//        }
//    }


//    public static T Get<T>(string id) where T : DataTable
//    {
//        if (!tables.ContainsKey(id))
//        {
//            Debug.LogError("테이블 없음");
//            return null;
//        }
//        return tables[id] as T;
//    }
//}
