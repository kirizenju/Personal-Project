using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    private GridSystemVisualSingle[,] gridSystemVisualArray;
    private void Awake()
    {
        //singleton
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 GridSystemVisual~" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        int width = LevelGrid.Instance.GetWidth();
        int height = LevelGrid.Instance.GetHeight();
        gridSystemVisualArray = new GridSystemVisualSingle[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform=
                    Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualArray[x,z]=gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }
    private void Update()
    {
        UpdateGridVisual();
    }
    private void UpdateGridVisual()
    {
        HideAllGridPosition();
        BaseAction selectedAction=UnitActionSystem.Instance.GetSelectedAction();
        ShowAllGridPosition(selectedAction.GetValidActionGridPositionList());
    }
    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualArray[x, z].Hide();
            }
        }
    }

    public void ShowAllGridPosition(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualArray[gridPosition.x, gridPosition.z].Show();
        }
    }

}
