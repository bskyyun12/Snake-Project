using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    Vector3 spawnPosition;

    Grid grid;

    public void SpawnApple()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        grid = GameObject.Find("A*").GetComponent<Grid>();


        // spawn an apple on a random tile
        int ranIndex = Random.Range(0, tiles.Length);
        spawnPosition = tiles[ranIndex].transform.position;

        // check if obstacles' node is equal to spawnPosition's node
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacle in obstacles)
        {
            // if does recall this function again
            if (grid.NodeFromWorldPoint(obstacle.transform.position) == grid.NodeFromWorldPoint(spawnPosition))
            {
                SpawnApple();
                return;
            }
        }

        // make an apple clone on the spawnPosition
        Instantiate(gameObject, spawnPosition, Quaternion.identity);
        return;
    }
}
