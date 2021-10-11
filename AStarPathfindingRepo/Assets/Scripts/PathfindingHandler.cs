using System.Collections.Generic;
using UnityEngine;

public class PathfindingHandler : MonoBehaviour
{
    [Header("Grid Size")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [Header("Node Size")]
    [SerializeField] private float cellSize;

    [Header("Offset")]
    [SerializeField] private Vector3 gridOffset;

    [Header("Debug")]
    [Space, SerializeField] private bool debugGrid = false;
    [SerializeField] private Color gridColor;
    [Space,SerializeField] private bool debugWall = false;
    [SerializeField] private Color wallColor;

    private Pathfinding pathfinding;
    private void Start()
    {
        pathfinding = new Pathfinding(gridWidth, gridHeight, cellSize, gridOffset);
    }

    public List<Vector3> GetFinalPathFromGrid(Vector3 currentPos, Vector3 targetPos)
    {
        pathfinding.GetGrid().GetGridPositionXY(currentPos, out int xStart, out int yStart);
        pathfinding.GetGrid().GetGridPositionXY(targetPos, out int xEnd, out int yEnd);

        //make sure end position is never outside the grid
        if (xEnd < 0) 
            xEnd = 0;

        if (yEnd < 0) 
            yEnd = 0;
            
        var pathNodeList = pathfinding.FindPath(xStart, yStart, xEnd, yEnd);
        var finalPath = new List<Vector3>();

        foreach (var pathNode in pathNodeList)
        {
            int x = pathNode.x;
            int y = (int)cellSize;
            int z = pathNode.y;
            
            //check again for collisions
            //pathNode.isWalkable = !Physics.CheckSphere(pathfinding.GetGrid().GetWorldPositionWithY(x, (int)cellSize / 2, z), cellSize, notWalkableLayerMask);

            if (!finalPath.Contains(pathfinding.GetGrid()
                .GetWorldPositionWithY(x, y, z)))
            {     
                var pos = pathfinding.GetGrid().GetWorldPositionWithY(x, y, z);
                finalPath.Add(pos);
            }
        }
        return finalPath;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    private void OnDrawGizmos()
    {
        if (!debugGrid || pathfinding == null)
            return;

        for (int x = 0; x < pathfinding.GetGrid().GetWidth(); x++) {
            for (int y = 0; y < pathfinding.GetGrid().GetHeight(); y++) {

                PathNode currentNode = pathfinding.GetGrid().GetGridObjectValue(x, y);

                if (debugWall)
                    Gizmos.color = currentNode.isWalkable ? gridColor : wallColor;
                else
                    Gizmos.color = gridColor;
                
                var pos = pathfinding.GetGrid().GetWorldPositionWithY(x, 0, y);
                pos.y = cellSize / 2;
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, cellSize, cellSize));
            }
        }
    }
    public void UpdateGrid()
    {
        pathfinding.UpdateGridCollisions();
    }
}