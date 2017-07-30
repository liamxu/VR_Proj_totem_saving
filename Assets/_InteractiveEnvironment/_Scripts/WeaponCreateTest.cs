
using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;



[RequireComponent(typeof(VRTK_ControllerEvents))]
public class WeaponCreateTest : MonoBehaviour {

	public GameObject generatedObj;
	private float touchpadPressingTimer;
	private bool touchpadPressingState = false;
    private VRTK_InteractGrab controller;


    // Use this for initialization
    void Start () {
        if(GetComponent<VRTK_ControllerEvents>() == null) {
            Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);
        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);

    }



    // Update is called once per frame
    void Update () {
		if (touchpadPressingState == true) {
			touchpadPressingTimer = touchpadPressingTimer + Time.deltaTime;
		} 
	}


    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e) {
        Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }
    

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e) {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e);
		touchpadPressingTimer = 0.0f;
		touchpadPressingState = true;
	}

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e) {
		DebugLogger (e.controllerIndex, "TOUCHPAD", "released", e);
		Debug.Log ("touch pad has been pressed for " + touchpadPressingTimer + " seconds.");
		touchpadPressingState = false;
		//if (touchpadPressingTimer < 0.2f) {
            // generate an obj
        //} else {
			// show a menu
		//}    
        GameObject geneCube = Resources.Load("StoneCube") as GameObject;
        geneCube.name = "StoneCube";
        controller = this.GetComponent<VRTK_InteractGrab>();
        CreateObject();
        //geneCube.transform.position = e.touchpadAxis;
        //controller.GetComponent<VRTK_InteractTouch>().ForceTouch(geneCube);
        //controller.AttemptGrab();

        
	}

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e) {
        throw new NotImplementedException();
    }



    //生成游戏对象函数
    void CreateObject() {
        var modelController = VRTK_DeviceFinder.GetModelAliasController(gameObject);        //find the location of controller (with an alias name in VRTK_DeviceFinder)
        Debug.Log(modelController.transform.position);
        Vector3 deltaVector = new Vector3(0.0f, 0.0f, 0.0f);    //生成位置偏差向量
        GameObject geneCube = Resources.Load("StoneCube") as GameObject;                    // 动态找GameObject。 Dinamicly Find a prefab from the "Resources" folder by using the name string
        geneCube.name = "StoneCube";
        geneCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        GameObject newGameObject = Instantiate(             //生成游戏对象，instatiate至场景中
            geneCube,                   //生成游戏对象的预制件
            modelController.transform.position - deltaVector,   //生成游戏对象的位置，为该脚本所在游戏对象的位置减去生成位置偏差向量
            modelController.transform.rotation                  //生成游戏对象的朝向
            
        ) as GameObject;
    }


    // flash effect of firing
    public GameObject muzzleFlash;
    // position of muzzle
    [SerializeField]
    private Transform firePos;
    // Ray test result
    RaycastHit hit;
    void Shoot(){
        GameObject go = GameObject.Instantiate(muzzleFlash);        // 静态实例化，在Inspector面板中赋予相关的GO
        go.transform.position = firePos.position;
        go.transform.rotation = firePos.rotation;

        //如果射线检测有结果
        //if(Physics.Raycast(firePos.position, firePos.forward, out hit, Mathf.Infinity)){
        //    print(hit.collider.name);
        //}            
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, Mathf.Abs(2))) {
            print(hit.collider.name);
        }
    }




}


