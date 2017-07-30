using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour {

    public GameObject InitSubPanel;     //开始界面的初始面板
    public GameObject StartSubPanel;    //点击“开始”按钮后的面板
    public GameObject OptionSubPanel;   //点击“选项”按钮后的面板
    public GameObject LevelNumLable;       //lable place to show game level number

    public InputField usernameInputField;   //用户名输入框组件
    public Toggle soundToggle;              //声音开关

    //GameObject lefthand = GameObject.FindGameObjectWithTag("HandL");
    //GameObject righthand = GameObject.FindGameObjectWithTag("HandR");
    //GameObject startBtn;
    int levelNum;
    GameObject[] enemies;

    //“开战”按钮调用的函数
    public void StartGame() {
        //PlayerPrefs.SetString("Username", usernameInputField.text); //将用户输入的用户名保存在本地，标识名为“Username”
        Debug.Log("Gonna start game soon");
        PlayerPrefs.SetString("Username", "test user 1");
        SceneManager.LoadScene("CartoonScene");                             //加载游戏场景
        
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "StartScene_newTest") {
            SceneManager.LoadScene("GameScene" + levelNum.ToString());
        }

        //SceneManager.LoadScene("CartoonScene");                             //加载游戏场景

    }
    //声音开关
    public void SwitchSound() {
        if (soundToggle.isOn) PlayerPrefs.SetInt("SoundOff", 0);    //当声音开关开启时，将声音开关设置保存在本地，标识名为“SoundOff”，值为0
        else PlayerPrefs.SetInt("SoundOff", 1);                 //当声音开关开启时，将声音开关设置保存在本地，标识名为“SoundOff”，值为1
    }

    //“退出”按钮调用的函数
    public void ExitGame() {
        Application.Quit(); //退出游戏
    }

    //初始化函数
    void Start() {
        ActiveInitPanel();  //调用ActiveInitPanel函数，启用初始面板，禁用其他面板
        //startBtn = transform.Find("Menus/WindowPanels/IntroCavas").gameObject;
        //startBtn = GameObject.Find("Menus/WindowPanels/IntroCavas").gameObject;
        //Debug.Log(startBtn.transform.name);
        levelNum = int.Parse(LevelNumLable.GetComponent<Text>().text);      //Set the start game level
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject go in enemies) {
            go.GetComponent<Animator>().SetBool("isMove", true);
        }
    }
    void Update() {

    }

    public void setLevelNum(float sliderposition) {
        levelNum = (int)sliderposition;
        Debug.Log(sliderposition);
        LevelNumLable.GetComponent<Text>().text = levelNum.ToString();
    }

    //启用初始面板，禁用其他面板
    public void ActiveInitPanel() {
        InitSubPanel.SetActive(true);
        //StartSubPanel.SetActive(false);
        //OptionSubPanel.SetActive(false);
    }

    //启用开始面板，禁用其他面板
    public void ActiveStartPanel() {
        InitSubPanel.SetActive(false);
        StartSubPanel.SetActive(true);
        OptionSubPanel.SetActive(false);
    }

    //启用选项面板，禁用其他面板
    public void ActiveOptionPanel() {
        InitSubPanel.SetActive(false);
        StartSubPanel.SetActive(false);
        OptionSubPanel.SetActive(true);
    }
}
