using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction;
    private Rigidbody2D myRigidbody;

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
        curDirection = Direction.right;
        myRigidbody = GetComponent<Rigidbody2D>();

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
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            grid.ChangeWalkableValue(obstacle, false);
        }

        // save the previous position before moving
        List<Vector3> snakeParts = new List<Vector3>();
        for (int i = 0; i < gameManager.list.Count; i++)
        {
            snakeParts.Add(gameManager.list[i].transform.position);
            //grid.ChangeWalkableValue(gameManager.list[i].transform.position, true);
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
            Vector2 prePosition = transform.position;

            transform.Translate(new Vector2(x, y));

            //LinkedList.Node currentNode = gameManager.list.headNode;

            //while (currentNode.next != null)
            //{
            //    currentNode = currentNode.next;
            //    Vector2 oldPosition = currentNode.data.transform.position;
            //    currentNode.data.transform.position = prePosition;
            //    prePosition = oldPosition;
            //}
        }
        else
        {
            int dstX = Mathf.Abs(grid.NodeFromWorldPoint(transform.position).gridX - grid.NodeFromWorldPoint(GameObject.Find("Apple(Clone)").transform.position).gridX);
            int dstY = Mathf.Abs(grid.NodeFromWorldPoint(transform.position).gridY - grid.NodeFromWorldPoint(GameObject.Find("Apple(Clone)").transform.position).gridY);
            int dst = dstX + dstY;

            print("path.Count : " + path.Count + " | dst : " + dst);

            if ((dst > 1 && path.Count == 1) || path.Count == 0)
            {
                print("no path");
                neighbours = grid.GetNeighbours(grid.NodeFromWorldPoint(transform.position));

                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (neighbours[i].walkable)
                    {
                        print("55555555555555");
                        transform.position = neighbours[i].worldPosition;
                        break;
                    }
                }
            }
            else if (path.Count != 0 && path[0] != grid.NodeFromWorldPoint(transform.position))
            {
                transform.position = path[0].worldPosition;
            }
        }


        //move the tails to the previous position saved above
        for (int i = 0; i < gameManager.list.Count; i++)
        {
            if (i != 0)
            {
                gameManager.list[i].transform.position = snakeParts[i - 1];
                //grid.ChangeWalkableValue(gameManager.list[i].transform.position, false);
                gameManager.list[i].tag = "Obstacle";
            }
        }

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

    //wanted to use this func but failed..
    //public void MoveTailTo(Vector2 position, LinkedList.Node obj)
    //{       

    //    if (obj.next != null)
    //    {
    //        Vector2 oldPosition = obj.next.data.transform.position;
    //        obj.next.data.transform.position = position;
    //        MoveTailTo(oldPosition, obj.next);
    //    }
    //}


}
