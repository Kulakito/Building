using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/buildings.dat";

    public static void SaveBuilding(List<BuildingData> buildings)
    {
        string json = JsonUtility.ToJson(new BuildingList { buildings = buildings }, true);
        File.WriteAllText(path, json);
    }

    public static List<BuildingData> LoadBuilding()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<BuildingList>(json).buildings;
        }
        return new List<BuildingData>();
    }
}
