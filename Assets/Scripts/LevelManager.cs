using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    Grid grid;

    public void CreateLevel()
    {
        string[] mapData = ReadLevelText();

        // char length of first index
        int mapX = mapData[0].ToCharArray().Length;
        // array length
        int mapY = mapData.Length;

        // set worldStart topleft
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        // y axis
        for (int y = 0; y < mapY; y++)
        {
            // separate numbers one by one
            char[] newTiles = mapData[y].ToCharArray();

            // x axis
            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
    }

    void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        GameObject newTile = Instantiate(tilePrefabs[tileIndex], gameObject.transform);
        
        // 0.5 offset so snake moves on the center of tiles
        newTile.transform.position = new Vector3(worldStart.x + x + 0.5f, worldStart.y - y - 0.5f, 0f);

        // wall tile
        if (tileIndex == 6) 
        {
            grid.ChangeWalkableValue(newTile, false);
        }
    }

    private string[] ReadLevelText()
    {
        // get a txt file from Resources folder
        TextAsset bindData = Resources.Load(SceneManager.GetActiveScene().name) as TextAsset;

        // replace newline to empty
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        // split data using - and return the split data
        return data.Split('-');
    }
}
