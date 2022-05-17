//Copyright 2022©, Yerio Janssen, All rights reserved.\
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
    
    /// <summary>
    /// sets all the needed data for the node
    /// </summary>
    /// <param name="grid">grid reference</param>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="isWalkable">represents if you can walk on this node or not</param>
    public PathNode(GridSystem<PathNode> grid, int x, int y, bool isWalkable)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
    }

    /// <summary>
    /// calculates the total cost to move (fCost)
    /// </summary>
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
