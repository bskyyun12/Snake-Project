using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject snake;

    [SerializeField]
    private GameObject tail;

    [SerializeField]
    private Apple apple;

    private LevelManager levelManager;

    public LinkedList list;

    private GameObject[] tiles;

    // buttons and texts
    GameObject GameOverButtons;
    GameObject ClearButtons;
    Text centerText;
    Text rightTopText;

    // count before game starts
    bool count = true;
    float time = 4f;

    // It won't pause game when gameover or clear
    bool gameover = false;
    bool clear = false;

    // score to clear level
    int targetScore;

    // property for list
    public LinkedList List
    {
        get
        {
            return list;
        }
    }

    void Start()
    {
        // create level
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.CreateLevel();

        apple.SpawnApple();

        // Set snake position on top of random tile's position
        snake = GameObject.Find("Snake");
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        int ranNum = Random.Range(0, tiles.Length);
        snake.transform.position = tiles[ranNum].transform.position;

        // add snake to linkedList
        list = new LinkedList();
        list.AddToEnd(snake);

        GameOverButtons = GameObject.Find("GameOverButtons");
        ClearButtons = GameObject.Find("ClearButtons");
        GameOverButtons.SetActive(false);
        ClearButtons.SetActive(false);

        centerText = GameObject.Find("CenterText").GetComponent<Text>();
        rightTopText = GameObject.Find("RightTopText").GetComponent<Text>();

        targetScore = tiles.Length / 5;
    }

    public void PauseGame(bool paused)
    {
        if (!gameover && !clear && !count)
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
            Time.timeScale = 0;
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
        // spawn a tail where the snake is
        if (list.Count == 1)
        {
            tail = Instantiate(tail, snake.transform.position, Quaternion.identity, snake.transform);
        }
        // spawn a tail at the end of tail
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

    public void Clear()
    {
        centerText.text = "Clear!";
        ClearButtons.SetActive(true);
        Time.timeScale = 0;
        clear = true;
    }
}
