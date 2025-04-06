using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public string prefabName;
    public Vector3 position;

    public BuildingData(Building building)
    {
        prefabName = building.name.Replace("(Clone)", "").Trim();
        position = building.transform.position;
    }
}

[System.Serializable]
public class BuildingList
{
    public List<BuildingData> buildings = new List<BuildingData>();
}
