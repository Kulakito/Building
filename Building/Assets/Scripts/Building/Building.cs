using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int Size { get; private set; }
    private bool isPlacing = false;

    private SpriteRenderer spriteRenderer;
    private BuildingManager buildingManager;
    private BuildPanel buildPanel;

    private void Start()
    {
        if (buildingManager != null) return;

        Initialize();
        isPlacing = true;
    }

    public void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingManager = FindFirstObjectByType<BuildingManager>();
        buildPanel = FindFirstObjectByType<BuildPanel>();

        Collider2D collider = GetComponent<Collider2D>();
        Vector2 size = collider.bounds.size;
        Size = new Vector2Int(
            Mathf.CeilToInt(size.x / buildingManager.CellSize),
            Mathf.CeilToInt(size.y / buildingManager.CellSize)
        );
    }

    private void Update()
    {
        if (isPlacing)
        {
            Vector2Int gridPos = buildingManager.SetGridPos(this);
            bool canPlace = buildingManager.CanPlaceBuilding(this, gridPos);

            spriteRenderer.color = canPlace ? buildingManager.CanPostBuildingColor : buildingManager.CantPostBuildingColor;

            if (canPlace && Input.GetMouseButtonDown(0))
            {
                buildingManager.PlaceBuilding(this, gridPos);
                spriteRenderer.color = new Color(1, 1, 1, 1);
                isPlacing = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isPlacing)
        {
            if (buildPanel.IsDeleting)
            {
                buildPanel.SetDeleteState(false);
                buildingManager.TryDeleteOccupiedPositions(this);
                Destroy(gameObject);
            }
            else
            {
                buildingManager.PrintBuildingCells(this);
            }
        }
    }
}
