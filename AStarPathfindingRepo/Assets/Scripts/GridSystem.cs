//Copyright 2022©, Yerio Janssen, All rights reserved.
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
    public Vector3 GetWorldPositionWithY(int x, int y, int z)
    {
        return new Vector3(x + offset.x, y, z+ offset.z) * cellSize;
    }
    
    public void GetGridPositionXY(Vector3 worldPosition, out int x, out int y)
    {
        //sets world position to grid position
        x = Mathf.FloorToInt((worldPosition.x - offset.x) / cellSize);
        y = Mathf.FloorToInt((worldPosition.z  - offset.z) / cellSize);
    }

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
