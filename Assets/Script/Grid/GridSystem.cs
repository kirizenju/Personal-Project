using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;
    public GridSystem(int width, int height,float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridObjectArray = new GridObject[width,height];
    }
    public void DrawGridLines()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition girdPosition = new GridPosition(x,z);
                gridObjectArray[x, z] = new GridObject(this, girdPosition);
               
            }
        }
    }
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, 0, gridPosition.z * cellSize);
        //return new Vector3(x,0,z)*cellSize;
    }
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x/cellSize),
            Mathf.RoundToInt(worldPosition.z/cellSize)
            );
    }
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Create a GridPosition for the current position
                GridPosition gridPosition =new GridPosition(x,z);
                // Instantiate a debug object at the current grid position
                Transform debugTransform = GameObject.Instantiate(debugPrefab,GetWorldPosition(gridPosition),Quaternion.identity);
                // Get the GridDebugObject component from the instantiated debug object
                GridDebugObject gridDebugObject =debugTransform.GetComponent<GridDebugObject>();
                // Set the GridObject for the debug object based on the current grid position
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x,gridPosition.z];
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
            gridPosition.z >= 0 && 
            gridPosition.x < width && 
            gridPosition.z < height;
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}
