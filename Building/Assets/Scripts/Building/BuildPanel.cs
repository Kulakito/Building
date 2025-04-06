using UnityEngine;
using UnityEngine.UI;

public class BuildPanel : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject[] buildings;
    [SerializeField] private Transform buttonParent;

    public bool IsDeleting { get; private set; }

    public GameObject SelectedBuilding { get; private set; }

    private void Awake()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);
            Image icon = newButton.transform.GetChild(0).GetComponent<Image>();
            icon.sprite = buildings[i].GetComponent<SpriteRenderer>().sprite;
            int index = i;
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectBuilding(index));
        }
    }

    private void SelectBuilding(int index)
    {
        SelectedBuilding = buildings[index];
    }

    public void CreateBuilding()
    {
        if (SelectedBuilding == null) return;

        Instantiate(SelectedBuilding);
    }

    public void SetDeleteState(bool delete)
    {
        IsDeleting = delete;
    }
}
