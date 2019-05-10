using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class LevelScenarioData {

    [Serializable]
    public class Goal
    {
        public int numberOfShifts;
        public int targetShaleAmount;

        public Goal(int numberOfShifts, int targetShaleAmount)
        {
            this.numberOfShifts = numberOfShifts;
            this.targetShaleAmount = targetShaleAmount;
        }
    }

    public string mapFileName;

    public string name;
    public string description;
    public int index;

    public Goal goal;

    public LevelScenarioData(string mapFileName, string name, string description, int index, Goal goal)
    {
        this.mapFileName = mapFileName;
        this.name = name;
        this.description = description;
        this.index = index;
        this.goal = goal;
    }

    public static string GetFilePath(string scenarioName)
    {
        return GetDirectoryPath() + "/" + scenarioName + ".dat";
    }

    public static string GetDirectoryPath()
    {
        return Application.dataPath + "/Data/scenarios";
    }

    public static string[] GetScenariosInDirectory()
    {
        CheckDirectory(GetDirectoryPath());
        string[] allFiles = Directory.GetFiles(GetDirectoryPath());
        List<string> results = new List<string>();
        for (int i = 0; i < allFiles.Length; i++)
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
        try
        {
            CheckDirectory(GetDirectoryPath());
            BinaryFormatter bf = new BinaryFormatter();

            if (File.Exists(GetFilePath(name)))
            {
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
            Directory.CreateDirectory(GetDirectoryPath());
        }
        else
        {
            Debug.Log("Directory exist: " + directory);
        };
    }

    public static LevelScenarioData Load(string name)
    {
        if (File.Exists(GetFilePath(name)))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(GetFilePath(name), FileMode.Open);
                LevelScenarioData loadedMap = (LevelScenarioData)bf.Deserialize(file);

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
        try
        {
            CheckDirectory(GetDirectoryPath());

            if (File.Exists(GetFilePath(name)))
            {
                File.Delete(GetFilePath(name));
            }
            else
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

    public override string ToString()
    {
        return "Name: " + name + "\n\n"
            + "Map: " + mapFileName + "\n\n"
            + "Index: " + index + "\n\n"
            + "Description: " + "\n\n\t" + description + "\n\n"
            + "Goal: \n\n" + "\tTarget shale amount: " + goal.targetShaleAmount + "\n"
            + "\tNumber of shifts: " + goal.numberOfShifts + "\n";
    }
}
