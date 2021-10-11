using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private GridSystem<PathNode> grid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    private float cellSize;
    Vector3 offset;
    
    private LayerMask notWalkable = LayerMask.GetMask("NotWalkable");

    public Pathfinding(int width, int height, float cellSize, Vector3 offset)
    {
        this.cellSize = cellSize;
        this.offset = offset;
        
        grid = new GridSystem<PathNode>(width, height, cellSize, offset, (g, x, y, b) => new PathNode(g, x, y, b));
    }
    
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObjectValue(startX, startY);
        PathNode endNode = grid.GetGridObjectValue(endX, endY);

        openList = new List<PathNode>{startNode};
        closedList = new List<PathNode>();
        
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObjectValue(x, y);

                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                //reached final Node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighborNode in GetNeighborList(currentNode))
            {
                if(closedList.Contains(neighborNode)) continue;

                if(!neighborNode.isWalkable)
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistance(neighborNode, endNode);
                    neighborNode.CalculateFCost();
                }

                if (!openList.Contains(neighborNode))
                {
                    openList.Add(neighborNode);
                }
            }
        }
        //out of nodes in open list
        return null;
    }

    private List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighborList = new List<PathNode>();
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = currentNode.x + x;
                int checkY = currentNode.y + y;

                if (checkX >= 0 && checkX < grid.GetWidth() && checkY >= 0 && checkY < grid.GetHeight())
                {
                    neighborList.Add(grid.GetGridObjectValue(checkX, checkY));
                }
            }
        }
        return neighborList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        
        path.Reverse();
        return path;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = pathNodeList[i];
        }
        return lowestFCostNode;
    }
    public GridSystem<PathNode> GetGrid()
    {
        return grid;
    }
    
    public void UpdateGridCollisions()
    {
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObjectValue(x, y);
                
                float cellSizeHalf = cellSize / 2;
                Vector3 cellSizeHalfExtends = new Vector3(cellSizeHalf, cellSizeHalf, cellSizeHalf);
                
                pathNode.isWalkable = !Physics.CheckBox(GetGrid().GetWorldPositionWithY(x, (int)cellSize / 2, y), cellSizeHalfExtends, Quaternion.identity, notWalkable);
            }
        }
    }
}