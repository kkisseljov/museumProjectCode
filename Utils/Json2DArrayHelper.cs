using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Json2DArrayHelper {

    public static T[][] FromJson<T>(string json)
    {
        string[] rows = JsonArrayHelper.FromJson<string>(json);
        T[][] result = new T[rows.GetLength(0)][];

        for(int i = 0; i < rows.GetLength(0); i++)
        {
            Debug.Log("JSON row(" + i + "): " + rows[i]);

            result[i] = JsonArrayHelper.FromJson<T>(rows[i]);

            Debug.Log("Deserialized row(" + i + "): " + result[i]);
        }

        return result;
    }

    public static string ToJson<T>(T[][] array)
    {
        string[] rows = new string[array.GetLength(0)];
        for(int i = 0; i < array.GetLength(0); i++)
        {
            rows[i] = JsonArrayHelper.ToJson(array[i]);
        }

        return JsonArrayHelper.ToJson(rows);
    }

    public static string ToJson<T>(T[][] array, bool prettyPrint)
    {
        string[] rows = new string[array.GetLength(0)];
        for (int i = 0; i < array.GetLength(0); i++)
        {
            rows[i] = JsonArrayHelper.ToJson(array[i], prettyPrint);
        }

        return JsonArrayHelper.ToJson(rows, prettyPrint);
    }
}
