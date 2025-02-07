using UnityEngine;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public static class CsvManager
{
    public static IEnumerable<T> Load<T>(string fileNameWithoutExtension)
    {
        // Resources에서 CSV 파일을 TextAsset으로 로드 (확장자 제거)
        TextAsset csvFile = Resources.Load<TextAsset>(fileNameWithoutExtension);
        if (csvFile == null)
        {
            Debug.LogError($"[CsvManager] 파일을 찾을 수 없습니다: {fileNameWithoutExtension}");
            return null;
        }

        using (var reader = new StringReader(csvFile.text))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    public static IEnumerable<T> LoadFromText<T>(string csvText)
    {
        using (var reader = new StringReader(csvText))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    public static List<T> ToList<T>(string keys) where T : IConvertible
    {
        List<T> result = new List<T>();
        if (!string.IsNullOrEmpty(keys))
        {
            List<string> strList = new List<string>();
            List<int> intList = new List<int>();

            if (IsListKeys<string>(keys, strList))
            {
                foreach (string record in strList)
                {
                    if (IsFormula(record, intList))
                    {
                        result.AddRange(intList.Cast<T>());
                        intList.Clear();
                    }
                }
            }
            else
            {
                if (IsFormula(keys, intList))
                {
                    result.AddRange(intList.Cast<T>());
                }
            }
        }
        return result;
    }

    private static bool IsFormula(string keys, List<int> output)
    {
        bool result = false;
        var keyList = keys.Split("#");

        if (keyList.Length > 1 && output != null)
        {
            int id = int.Parse(keyList[0]);
            var rangeFormula = keyList[1].Split("~");

            if (rangeFormula.Length > 1)
            {
                int increase = int.Parse(rangeFormula[0]);
                int maxCount = int.Parse(rangeFormula[1]);
                int size = id + (increase * maxCount);

                for (int i = id + increase; i <= size; i += increase)
                {
                    output.Add(i);
                }
                result = true;
            }
        }
        return result;
    }

    private static bool IsListKeys<T>(string keys, List<T> output)
    {
        bool result = false;
        var keyList = keys.Split('/');

        if (keyList.Length > 0 && keyList[0].Length > 0)
        {
            for (int i = 0; i < keyList.Length; ++i)
            {
                output.Add((T)Convert.ChangeType(keyList[i], typeof(T)));
            }
            result = true;
        }
        return result;
    }
}
