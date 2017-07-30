using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager gm;               //static game manager. The only instance of GM 

    public enum GameState                       //enum of 3 Game states
    { Prapare, Playing, GameOver, Winning };
    public GameState gameState;         //游戏状态
    public GameObject player;           //游戏主角
    public int score = 0;                   // The player's score.
    public Text scoreValueLable;                 // The player's score lable;

    private PlayerHealth playerHealth;	//玩家生命值组件
    public GameObject gameResultObj;

    public int maxEnemyExist = 8;               // max living exist enemy 
    public int initEnemyCount = 0;              // initial living enemy
    public int maxGenerateEnemyCount = 0;              // max generated enemy

    //public int maxEnemyCount = 8;               // max living exist enemy limit
    //public int currEnemyCount = 0;              // current living enemy
    public float prepareTime = 30.0f;                // preparation before bunny start
    public float winScore = 100.0f;                // score to win

    void Awake()
    {
        gm = this;
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (prepareTime >= 0 && gm.gameState == GameState.Prapare)
        {
            prepareTime -= Time.deltaTime;
        }
        else if (playerHealth.currentHealth > 0 && gm.gameState != GameState.Playing)
        {
            gm.gameState = GameState.Playing;
        }

        //Debug.Log("---------game status-------" + playerHealth.currentHealth + GameManager.gm.gameState);
        Debug.Log(gameResultObj);

        if (GameManager.gm.gameState == GameState.GameOver)
        {
            //Debug.Log("---------game over-------" + playerHealth.currentHealth);
            GameOver();
        }
        if (score >= winScore)
        {
            gm.gameState = GameState.Winning;
            GameWin();
        }
    }

    // Add score when enemy die.
    public void AddScore(int value)
    {
        score += value;
        Debug.Log("玩家加分======" + score);
        scoreValueLable.GetComponent<Text>().text = score.ToString();
    }


    //玩家扣血
    public void PlayerTakeDamage(int value)
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(value);
    }
    //玩家加血
    public void PlayerAddHealth(int value)
    {
        if (playerHealth != null)
            playerHealth.AddHealth(value);
    }

    void GameOver()
    {
        gameResultObj.SetActive(true);
    }

    void GameWin()
    {
        gameResultObj.GetComponentInChildren<Text>().text = "Congratulation! You win!";
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject go in enemies) {
            go.GetComponent<EnemyHealth>().TakeDamage(20, go.transform.position);
        }
        gameResultObj.SetActive(true);

    }

}
