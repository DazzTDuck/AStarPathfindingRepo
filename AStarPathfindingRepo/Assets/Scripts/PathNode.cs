using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridSystem<PathNode> grid;
    
    public int x;
    public int y;
    
    public int gCost;
    public int hCost;
    public int fCost;
    
    public bool isWalkable;

    public PathNode cameFromNode; 
    
    public PathNode(GridSystem<PathNode> grid, int x, int y, bool isWalkable)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
