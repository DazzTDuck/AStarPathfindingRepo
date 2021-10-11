using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("---PathfindingHandler---")] 
    [SerializeField] private PathfindingHandler pathfindingHandler;

    [Header("---Agent Settings---")] 
    [SerializeField] private float moveSpeed;
    [Tooltip("Minimal distance to check to the next node, if in that distance switch to next node in list and move to that")]
    [SerializeField] private float minimalDistance= 0.25f;
    [SerializeField] private float rotationSpeed;

    [Header("---Debug Path---")] 
    [SerializeField] private bool debugPath = false;
    [SerializeField] private Color pathColor;

    //for debugging
    [Space]
    [SerializeField] private Transform moveTo;
    
    private List<Vector3> path = new List<Vector3>();
    private bool hasDestination = false;
    private bool atDestination = false;
    private int currentPathNodeIndex = 0;
    

    private void Update()
    {
        //for debugging
         if(Input.GetMouseButtonDown(0))
         {
             SetDestination(moveTo.position);
         }
    }

    public void SetDestination(Vector3 targetPos)
    {
        atDestination = false;
        path = pathfindingHandler.GetFinalPathFromGrid(transform.position, targetPos);
        hasDestination = path.Count > 0;
        currentPathNodeIndex = 0;
    }

    private void FixedUpdate()
    {
        if (!hasDestination) 
            return;

        Vector3 moveToPos = path[currentPathNodeIndex];
        moveToPos.y = transform.position.y;

        if (Vector3.Distance(transform.position, moveToPos) < minimalDistance)
        {
            if (currentPathNodeIndex + 1 > path.Count - 1)
            {
                //reached destination
                path.Clear();
                hasDestination = false;
                atDestination = true;
            }
            else
                currentPathNodeIndex++;
        }
        else
        {
            //move player to position
            var position = transform.position;
            position = Vector3.MoveTowards(position, moveToPos, moveSpeed);
            transform.position = position;

            //rotation
            Quaternion newRotation = Quaternion.LookRotation(moveToPos - position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public bool GetIfAtDestination()
    {
        return atDestination;
    }

    //debug path
    private void OnDrawGizmos()
    {
        if (!debugPath)
            return;
        
        var position = transform.position;

        if (pathfindingHandler == null || path == null) 
            return;

        foreach (var pos in path)
        {
            Gizmos.color = pathColor; // show path that was found

            var cellSize = pathfindingHandler.GetCellSize();
            
            Gizmos.DrawWireCube(new Vector3(pos.x, pos.y, pos.z), 
                new Vector3(cellSize,cellSize,cellSize));
        }
    }
}
