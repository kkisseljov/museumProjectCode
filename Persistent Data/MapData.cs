using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//TODO file methods can be refactored as an abstract class
[Serializable]
public class MapData
{
    public string name;
    public BlockGrid blockGrid;

    public MapData(string name, BlockGrid blockGrid)
    {
        this.name = name;
        this.blockGrid = blockGrid;
    }

    public static string GetFilePath(string mapName, bool lookInResources = false)
    {
        return GetDirectoryPath() + "/" + mapName + ".dat";
    }

    public static string GetDirectoryPath()
    {
        return Application.dataPath + "/Data/maps";
    }

    public static string[] GetMapsInDirectory()
    {
        CheckDirectory(GetDirectoryPath());
        string[] allFiles = Directory.GetFiles(GetDirectoryPath());
        List<string> results = new List<string>();
        for(int i = 0; i < allFiles.Length; i++)
        {
            if (allFiles[i].Contains(".dat") && !allFiles[i].Contains(".meta"))
            {
                string[] splitted = allFiles[i].Split('\\');
                string fileName = splitted[splitted.Length - 1];
                string fileNameWithoutExtension = fileName.Split('.')[0];
                results.Add(fileNameWithoutExtension);
            }
        }

        return results.ToArray();
    }

    public bool Save()
    {
        Debug.Log("SAVING");

        try
        {
            CheckDirectory(GetDirectoryPath());
            BinaryFormatter bf = new BinaryFormatter();

            if (File.Exists(GetFilePath(name)))
            {
                Debug.Log("Map exist -> overwrite");
                File.Delete(GetFilePath(name));
            }

            FileStream file = File.Create(GetFilePath(name));

            bf.Serialize(file, this);
            file.Close();
        }
        catch (SystemException e)
        {
            Debug.LogError(e);
            return false;
        }


        return true;
    }

    private static void CheckDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Debug.Log("Directory doesn't exist: " + directory + "\n\tTry to create");
            Directory.CreateDirectory(directory);
        }
        else
        {
            Debug.Log("Directory exist: " + directory);
        };
    }

    public static MapData Load(string name)
    {
        Debug.Log("LOADING");

        if (File.Exists(GetFilePath(name)))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(GetFilePath(name), FileMode.Open);
                MapData loadedMap = (MapData)bf.Deserialize(file);

                file.Close();

                return loadedMap;
            }
            catch (SystemException e)
            {
                Debug.LogError(e);
                return null;
            }
        }
        else
        {
            Debug.LogError("File doens't exit");
            return null;
        }
    }

    public static bool Delete(string name)
    {
        Debug.Log("DELETING");

        try
        {
            CheckDirectory(GetDirectoryPath());

            if (File.Exists(GetFilePath(name)))
            {
                File.Delete(GetFilePath(name));
            } else
            {
                Debug.LogError("File " + name + " wasn't found!");
            }
        }
        catch (SystemException e)
        {
            Debug.LogError(e);
            return false;
        }

        return true;
    }

    //TODO might be useful in future
    //https://answers.unity.com/questions/591545/not-able-to-load-binary-file-through-resourcesload.html
}
