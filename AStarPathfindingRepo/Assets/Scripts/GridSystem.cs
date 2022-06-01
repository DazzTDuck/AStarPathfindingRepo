//Copyright 202©, Yerio Janssen, All rights reserved.
using System;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 offset;

    private TGridObject[,] gridArray;

    private LayerMask notWalkable = LayerMask.GetMask("NotWalkable");

    /// <summary>
    /// Generates the grid with given data & nodes
    /// </summary>
    /// <param name="width">width of grid</param>
    /// <param name="height">height of grid</param>
    /// <param name="cellSize">cell size of grid</param>
    /// <param name="offset">position offset for grid</param>
    /// <param name="createGridObject">creates node with given overloads</param>
    public GridSystem(int width, int height, float cellSize, Vector3 offset, Func<GridSystem<TGridObject>, int, int, bool, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset = offset;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                float cellSizeHalf = cellSize / 2;
                Vector3 cellSizeHalfExtends = new Vector3(cellSizeHalf, cellSizeHalf, cellSizeHalf);

                bool isWalkable = !Physics.CheckBox(GetWorldPositionWithY(x, (int)cellSize / 2, y), cellSizeHalfExtends, Quaternion.identity, notWalkable);
                gridArray[x, y] = createGridObject(this, x, y, isWalkable);
            }
        }
    }
    /// <summary>
    /// Returns world position
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="height">height of the cell to translate into Vector3</param>
    /// <param name="y">y coordinate</param>
    /// <returns></returns>
    public Vector3 GetWorldPositionWithY(int x, int height, int y)
    {
        return new Vector3(x + offset.x, height, y + offset.z) * cellSize;
    }
    
    /// <summary>
    /// returns the grid position of a cell at given world position
    /// </summary>
    /// <param name="worldPosition">World position you want to get coordinates for</param>
    /// <param name="x">returns the x coordinate of cell at world position</param>
    /// <param name="y">returns the y coordinate of cell at world position</param>
    public void GetGridPositionXY(Vector3 worldPosition, out int x, out int y)
    {
        //sets world position to grid position
        x = Mathf.FloorToInt((worldPosition.x - offset.x) / cellSize);
        y = Mathf.FloorToInt((worldPosition.z  - offset.z) / cellSize);
    }

    /// <summary>
    /// returns value that is at given grid position
    /// </summary>
    /// <param name="x">x coordinate of cell</param>
    /// <param name="y">x=y coordinate of cell</param>
    /// <returns></returns>
    public TGridObject GetGridObjectValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];
        else
            return default(TGridObject);
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
