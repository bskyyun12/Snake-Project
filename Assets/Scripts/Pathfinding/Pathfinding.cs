using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    GameObject snake;
    Transform targetTransform;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        snake = GameObject.Find("Snake");
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            targetTransform = GameObject.Find("Apple(Clone)").GetComponent<Transform>();
            FindPath(snake.transform.position, targetTransform.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // start from current snake's node and add its neighbours to openSet
        List<Node> openSet = new List<Node>();
        // add the node where the snake was
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                // cheapest cost node will be current node
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            // if get the target node
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                // skip all non walkable nodes and nodes in closeSet
                if (!neighbour.walkable || closeSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;   // cost between currentNode and neighbour + currentNode's gcost
                    neighbour.hCost = GetDistance(neighbour, targetNode);   // cost between neighbour and targetNode
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    /// <summary>
    /// save the list of path nodes and use it in Grid and Snake script
    /// </summary>
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
        snake.GetComponent<Snake>().path = path;
    }

    // get x and y's node differences between two nodes and get distance value with them
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

}
