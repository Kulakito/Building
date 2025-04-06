using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;

    private void Start()
    {
        buildingManager.LoadSave();
    }
}
