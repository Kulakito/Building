using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [field: SerializeField] public Vector2Int Grid { get; private set; }

    private Dictionary<Building, List<Vector2Int>> placedBuildings = new Dictionary<Building, List<Vector2Int>>();

    [field: SerializeField] public float CellSize { get; private set; }

    [field: SerializeField] public Color CanPostBuildingColor { get; private set; }
    [field: SerializeField] public Color CantPostBuildingColor { get; private set; }

    public Vector2Int SetGridPos(Building building)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int gridPosX = Mathf.RoundToInt(mousePos.x / CellSize);
        int gridPosY = Mathf.RoundToInt(mousePos.y / CellSize);

        if (building.Size.x % 2 == 0) building.transform.position = new Vector2(gridPosX * CellSize, gridPosY * CellSize);
        else building.transform.position = new Vector2((gridPosX - CellSize) * CellSize, gridPosY * CellSize);

        return new Vector2Int(gridPosX, gridPosY);
    }

    public bool CanPlaceBuilding(Building building, Vector2Int startGridPos)
    {
        int startPointX = 0 - Mathf.CeilToInt((float)building.Size.x / 2);
        for (int x = startPointX; x <= building.Size.x + startPointX; x++)
        {
            for (int y = 0; y <= building.Size.y; y++)
            {
                Vector2Int checkPos = new Vector2Int(startGridPos.x + x, startGridPos.y + y);
                if (IsCellOccupied(checkPos))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PlaceBuilding(Building building, Vector2Int startGridPos)
    {
        List<Vector2Int> occupiedCells = new List<Vector2Int>();

        int startPointX = 0 - Mathf.CeilToInt((float)building.Size.x / 2);
        for (int x = startPointX; x <= building.Size.x + startPointX; x++)
        {
            for (int y = 0; y <= building.Size.y; y++)
            {
                Vector2Int occupiedPos = new Vector2Int(startGridPos.x + x, startGridPos.y + y);
                occupiedCells.Add(occupiedPos);
            }
        }
        placedBuildings.Add(building, occupiedCells);
    }

    private bool IsCellOccupied(Vector2Int position)
    {
        foreach (List<Vector2Int> positions in placedBuildings.Values)
        {
            if (positions.Contains(position))
            {
                return true;
            }
        }
        return false;
    }

    public void TryDeleteOccupiedPositions(Building building)
    {
        if (placedBuildings.ContainsKey(building))
        {
            placedBuildings.Remove(building);
        }
    }

    public void PrintBuildingCells(Building building)
    {
        print(building.name);
        foreach (Vector2Int cell in placedBuildings[building])
        {
            print(cell);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int x = 0; x <= Grid.x; x++)
        {
            float worldX = transform.position.x + x * CellSize;
            Gizmos.DrawLine(new Vector3(worldX, transform.position.y, 0), new Vector3(worldX, transform.position.y + Grid.y * CellSize, 0));
        }

        for (int y = 0; y <= Grid.y; y++)
        {
            float worldY = transform.position.y + y * CellSize;
            Gizmos.DrawLine(new Vector3(transform.position.x, worldY, 0), new Vector3(transform.position.x + Grid.x * CellSize, worldY, 0));
        }
    }

    private void OnApplicationQuit()
    {
        List<BuildingData> buildings = new List<BuildingData>();
        foreach (Building building in placedBuildings.Keys)
        {
            buildings.Add(new BuildingData(building));
        }
        SaveSystem.SaveBuilding(buildings);
    }

    public void LoadSave()
    {
        List<BuildingData> buildings = SaveSystem.LoadBuilding();
        if (buildings.Count == 0) return;

        foreach (BuildingData data in buildings)
        {
            GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Buildings/" + data.prefabName);
            GameObject buildingObject = Instantiate(buildingPrefab, data.position, Quaternion.identity);

            Building building = buildingObject.GetComponent<Building>();
            building.Initialize();

            PlaceBuilding(building, new Vector2Int(Mathf.CeilToInt(building.transform.position.x / CellSize), Mathf.CeilToInt(building.transform.position.y / CellSize)));
        }
    }
}
