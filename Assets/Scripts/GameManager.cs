using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject snake;

    [SerializeField]
    private GameObject tail;

    [SerializeField]
    private Apple apple;

    [SerializeField]
    private LevelManager levelManager;

    public LinkedList list;

    private GameObject[] tiles;

    GameObject GameOverButtons;
    GameObject ClearButtons;

    Text centerText;
    Text rightTopText;

    bool count = true;
    float time = 4f;

    bool gameover = false;
    bool clear = false;

    int targetScore;

    public LinkedList List
    {
        get
        {
            return list;
        }
    }

    void Start()
    {
        levelManager.CreateLevel();

        apple.SpawnApple();

        // Set snake position on top of random tile's position
        snake = GameObject.Find("Snake");
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        snake.transform.position = tiles[100].transform.position;

        list = new LinkedList();
        list.AddToEnd(snake);

        GameOverButtons = GameObject.Find("GameOverButtons");
        GameOverButtons.SetActive(false);

        ClearButtons = GameObject.Find("ClearButtons");
        ClearButtons.SetActive(false);

        centerText = GameObject.Find("CenterText").GetComponent<Text>();
        rightTopText = GameObject.Find("RightTopText").GetComponent<Text>();
        Time.timeScale = 0;

        targetScore = tiles.Length / 5;
    }

    public void PauseGame(bool paused)
    {
        if (!gameover && !clear)
        {
            if (paused)
            {
                Time.timeScale = 0;
                centerText.text = "Paused!\nMove the snake to continue..";
                centerText.fontSize = 32;
            }
            else
            {
                Time.timeScale = 1;
                centerText.text = "";
            }
        }
    }

    private void Update()
    {
        if (count)
        {
            time -= Time.unscaledDeltaTime;
            centerText.text = ((int)time).ToString();
            if (time <= 0)
            {
                centerText.text = "";
                Time.timeScale = 1;
                count = false;
            }
        }
        rightTopText.text = list.Count + " / " + targetScore;

    }

    public void Clear()
    {
        centerText.text = "Clear!";
        ClearButtons.SetActive(true);
        Time.timeScale = 0;
        clear = true;
    }

    public void GameOver()
    {
        Debug.Log("YOU TOUCHED AN OBSTACLE!! GAME OVER!!!!!!!");
        gameover = true;
        GameOverButtons.SetActive(true);
        centerText.text = "Game Over!";
        Time.timeScale = 0;
    }


    public void AddTail()
    {
        if (list.Count == 1)
        {
            tail = Instantiate(tail, snake.transform.position, Quaternion.identity, snake.transform);
        }
        else
        {
            tail = Instantiate(tail, list[list.Count - 1].transform.position, Quaternion.identity, snake.transform);
        }
        tail.tag = "Untagged";
        list.AddToEnd(tail);


        // clear level
        if (list.Count == targetScore)
        {
            Clear();
        }

    }
}
