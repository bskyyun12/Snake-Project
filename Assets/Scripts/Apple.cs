using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    Vector3 spawnPosition;

    //[SerializeField]
    //Snake snake;

    Grid grid;


    private void Start()
    {
        //snake = GameObject.Find("Snake").GetComponent<Snake>();
    }

    public void SpawnApple()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        grid = GameObject.Find("A*").GetComponent<Grid>();

        for (int i = 0; i < tiles.Length; i++)
        {
            // spawn an apple on a random tile
            int ranIndex = Random.Range(0, tiles.Length);
            spawnPosition = tiles[ranIndex].transform.position;

            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (var obstacle in obstacles)
            {
                if (grid.NodeFromWorldPoint(obstacle.transform.position) == grid.NodeFromWorldPoint(spawnPosition))
                {
                    SpawnApple();
                    return;
                }
            }

            Instantiate(gameObject, spawnPosition, Quaternion.identity);
            return;
        }
    }
}
