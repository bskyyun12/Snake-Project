using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Apple apple;

    bool up, left, right, down;

    float moveRate = 0.4f;
    float timer;

    Direction curDirection;

    LinkedList.Node node;

    public enum Direction
    {
        up, down, left, right
    }

    [SerializeField]
    Grid grid;

    GameObject[] tiles;

    bool isAIPlaying;
    public List<Node> path;
    float speed = 20f;
    int targetIndex;
    int i = 0;

    List<Node> neighbours;

    private void Start()
    {
        // To make Snake to move right in the beginning
        curDirection = Direction.right;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        SetPlayerDirection();
        SetMoveRate(isAIPlaying);

        if (Input.GetButton("Jump"))
        {
            isAIPlaying = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            isAIPlaying = false;
            gameManager.PauseGame(true);
        }

        timer += Time.deltaTime;
        if (timer > moveRate)
        {
            timer = 0;
            Move();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Apple")
        {
            gameManager.AddTail();
            Destroy(collision.gameObject);
            apple.SpawnApple();
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            gameManager.GameOver();
        }
    }

    void GetInput()
    {
        up = Input.GetButtonDown("Up");
        down = Input.GetButtonDown("Down");
        left = Input.GetButtonDown("Left");
        right = Input.GetButtonDown("Right");

        // Break the pause
        if (up || down || left || right)
        {
            gameManager.PauseGame(false);
        }
    }

    void SetPlayerDirection()
    {
        if (up)
        {
            curDirection = Direction.up;
        }
        else if (down)
        {
            curDirection = Direction.down;
        }
        else if (left)
        {
            curDirection = Direction.left;
        }
        else if (right)
        {
            curDirection = Direction.right;
        }
    }

    /// <summary>
    /// Change MoveRate depends on the tile's type and depends on if AI is playing
    /// </summary>
    void SetMoveRate(bool isAIPlaying)
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var tile in tiles)
        {
            if (tile.name == "Tile_sand_01(Clone)" || tile.name == "Tile_sand_02(Clone)")
            {
                // if snake is on a sand tile
                if (grid.NodeFromWorldPoint(tile.transform.position) == grid.NodeFromWorldPoint(transform.position))
                {
                    if (isAIPlaying)
                    {
                        moveRate = 0.2f;
                    }
                    else
                    {
                        moveRate = 0.6f;
                    }
                    break;
                }
                // if snake is on a normal tile
                else
                {
                    if (isAIPlaying)
                    {
                        moveRate = 0.1f;
                    }
                    else
                    {
                        moveRate = 0.3f;
                    }
                }
            }
        }
    }

    public void Move()
    {
        // find all the tails and set their nodes' walkable value to false
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            grid.ChangeWalkableValue(obstacle, false);
        }

        // save positions before moving
        List<Vector3> snakeParts = new List<Vector3>();
        for (int i = 0; i < gameManager.list.Count; i++)
        {
            snakeParts.Add(gameManager.list[i].transform.position);
        }


        if (!isAIPlaying)
        {
            // move the snake
            int x = 0;
            int y = 0;
            switch (curDirection)
            {
                case Direction.up:
                    y += 1;
                    break;
                case Direction.down:
                    y -= 1;
                    break;
                case Direction.left:
                    x -= 1;
                    break;
                case Direction.right:
                    x += 1;
                    break;
            }

            transform.Translate(new Vector2(x, y));

        }
        else
        {
            // get distance(how many nodes) between the snake and the apple
            int dstX = Mathf.Abs(grid.NodeFromWorldPoint(transform.position).gridX - grid.NodeFromWorldPoint(GameObject.Find("Apple(Clone)").transform.position).gridX);
            int dstY = Mathf.Abs(grid.NodeFromWorldPoint(transform.position).gridY - grid.NodeFromWorldPoint(GameObject.Find("Apple(Clone)").transform.position).gridY);
            int dst = dstX + dstY;

            // if distance is greater than 1 but path is 1 or path is 0
            if ((dst > 1 && path.Count == 1) || path.Count == 0)
            {
                // There is no path. 
                // getting neighbour nodes from snake's current node and move the snake to its neighbour if the neighbour is walkable.
                neighbours = grid.GetNeighbours(grid.NodeFromWorldPoint(transform.position));

                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (neighbours[i].walkable)
                    {
                        transform.position = neighbours[i].worldPosition;
                        break;
                    }
                }
            }
            // if there is path, move the snake to the first index node value in path
            else if (path.Count != 0 && path[0] != grid.NodeFromWorldPoint(transform.position))
            {
                transform.position = path[0].worldPosition;
            }
        }


        // move the tails to the previous position saved above
        for (int i = 0; i < gameManager.list.Count; i++)
        {
            if (i != 0)
            {
                gameManager.list[i].transform.position = snakeParts[i - 1];
                gameManager.list[i].tag = "Obstacle";
            }
        }

        // make sure all the tiles are walkable but all the obstacles are not walkable.
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles)
        {
            grid.ChangeWalkableValue(tile, true);
            foreach (GameObject obstacle in obstacles)
            {
                grid.ChangeWalkableValue(obstacle, false);
            }
        }
    }
}
