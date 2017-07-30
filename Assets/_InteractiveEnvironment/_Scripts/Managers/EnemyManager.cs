using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    static public EnemyManager em;               //static enemy manager. The only instance of enemymanager 

    public float minSecond = 2.0f;          //随机生成游戏对象的最小时间
    public float maxSecond = 5.0f;          //随机生成游戏对象的最大时间
    public float scaleSize = 1.0f;          //随机生成游戏对象的缩放比例
    private GameObject[] enemyGenerators = null;


    private float timer;        //生成时间间隔，记录从上次生成游戏对象到现在经过的时间
    private float createTime;   //生成时间，下次以生成游戏对象的时间，该值在[minSeconds,maxSeconds]随机生成

    private int[] countResList;                  //资源编号列表
    private GameObject[] enemyList;             // 敌人资源GameObject List
    public int enemyResCount = 4;                               // enemy resource count

    public int enemyCurrCount = 0;                               // enemy in game count
    public int enemyTotalGaneCount = 0;                          // all generated enemy count

    void Awake() {
        em = this;
    }

    //初始化，参数初始化
    void Start()
    {
        if (enemyGenerators == null)
        {
            enemyGenerators = GameObject.FindGameObjectsWithTag("startPoint");
        }


        timer = 0.0f;           //将生成时间间隔清零
        createTime = Random.Range(minSecond, maxSecond);    //在[minSeconds,maxSeconds]区间随机设置生成时间
        enemyCurrCount = 0;
        InitObjects();

    }

    //每帧执行，用于在随机时间内自动生成游戏对象
    void Update()
    {
        //若游戏状态不是游戏进行（Playing），则不生成游戏对象
        if (GameManager.gm != null && GameManager.gm.gameState != GameManager.GameState.Playing)
            return;

        timer += Time.deltaTime;    //更新生成时间间隔，增加上一帧所花费的时间
        if (timer >= createTime)
        {  //当生成时间间隔大于等于生成时间时

            /*调用CreateObject生成随机游戏对象*/
            //enemyGenerator = enemyGenerators[Random.Range(0, enemyGenerators.Length)];
            //int toGeneObjIndex = Random.Range(0, enemyList.Length);      // Random.Range(min, max) between min [inclusive] and max [exclusive] 
            //CreateObject(toGeneObjIndex, enemyGenerator.transform.position, scaleSize);      

            /*Generate 33 rabbits for each route {0、1、2}. the {3} rabbit is the BOSS*/
            //if (enemyCount < 99 && GameManager.gm.currEnemyCount < GameManager.gm.maxEnemyCount)

            if (GameManager.gm != null)
            {
                if (enemyTotalGaneCount < GameManager.gm.maxGenerateEnemyCount
                      && enemyCurrCount < GameManager.gm.maxEnemyExist) {
                    int i = enemyTotalGaneCount % enemyResCount;
                    CreateObject(i);
                }
                else if (enemyTotalGaneCount == GameManager.gm.maxGenerateEnemyCount)
                {
                    CreateObject(3);
                }
            }
            timer = 0.0f;           //将生成时间间隔清零
            createTime = Random.Range(minSecond, maxSecond);    //在[minSeconds,maxSeconds]区间随机设置生成时间
        }
    }


    /// <summary>
    /// Initial Enemy data from the "Enemy" folder by using Resouces.LoadAll()
    /// </summary>
    void InitObjects()
    {

        enemyList = Resources.LoadAll<GameObject>("Enemy");

        countResList = new int[enemyList.Length];

    }


    //生成游戏对象函数
    void CreateObject(int typeIndex, Vector3 position, float scaleVar)
    {

        string objName = enemyList[typeIndex].name;


        Debug.Log("Create " + objName + " " + countResList[typeIndex].ToString());

        GameObject newGameObject = Instantiate(
            enemyList[typeIndex],
            position,
            Quaternion.identity
        ) as GameObject;

        newGameObject.transform.localScale *= scaleVar;
        countResList[typeIndex]++;
        newGameObject.name = objName + countResList[typeIndex].ToString();
    }

    void CreateObject(int typeIndex)
    {

        string objName = enemyList[typeIndex].name;

        Debug.Log("Create " + objName + " " + countResList[typeIndex].ToString());

        GameObject newGameObject = Instantiate(
            enemyList[typeIndex]
        ) as GameObject;

        newGameObject.transform.localScale *= scaleSize;
        countResList[typeIndex]++;
        newGameObject.name = objName + countResList[typeIndex].ToString();
        string route = "Route" + typeIndex.ToString();
        newGameObject.GetComponent<Follow>().circuit = GameObject.Find(route).GetComponent<WaypointCircuit>();

        enemyCurrCount += 1;
        enemyTotalGaneCount += 1;
    }

    // Adjust enemy Count(e.g. +1/-1) when enemy create / die.
    public void AdjustEnemyCount(int value) {
        if (enemyCurrCount >= 0) {
            enemyCurrCount += value;
            Debug.Log("count change ---- " + enemyCurrCount);
        }
    }


}
