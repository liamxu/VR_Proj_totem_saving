using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// ControllerGenerateWeapon.
/// ---20170216 Created by Xu Lingxiao---
/// Used for generate Weapon(GO) on the controller
/// </summary>


[RequireComponent(typeof(VRTK_ControllerEvents))]
public class ControllerGenerateWeapon : MonoBehaviour {

    public GameObject generatedObj { get; set; }

    private VRTK_Control vrtkController;

    private bool weapon_generated = false;        // true when touchpad clicked.           
    private bool weapon_menu_shown = false;           // turn to true when touchpad clicked, and the menu shows.         
    /*  
    //  touchpad 第一次按下，显示物品menu。｛weapon_MENU == TRUE｝
    //  第二次按下，根据方位选择物品，并生成相应物品，跟随controller；menu关闭。 {cube_generated = TRUE .  weapon_MENU == FALSE }
    //  物品跟随controller。按下trigger施力发射。
    */
    private GameObject weaponMenu;
    private Transform[] weaponList;
    private int[] countResList;                  //资源编号列表

    private GameObject currHandWeapon;
    private GameObject gameController;
    int typeIndex = 0;
    float scaleValue = 0.5f;


    // Use this for initialization
    void Start() {
        /*
        if (GetComponent<VRTK_ControllerEvents>() == null) {
            Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }
        */

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);
        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        //GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);


        //Set Weapon menu position ___ .
        gameController = VRTK.VRTK_DeviceFinder.GetControllerRightHand(true);
        Debug.Log(gameController);

        weaponMenu = GameObject.FindGameObjectWithTag("WeaponMenu");
        Debug.Log(weaponMenu);
        //GameObject trackpadObj = gameController.transform.Find("Model/trackpad").gameObject;
        setMenu(gameController);
        weaponList = weaponMenu.GetComponentsInChildren<Transform>(true);
        currHandWeapon = null;
        countResList = new int[weaponList.Length];


    }


    // Update is called once per frame
    void Update() {

        if (currHandWeapon) {
            //Debug.Log("------------" + currHandWeapon.name.ToString());
            currHandWeapon.transform.position = gameController.transform.position;
        }

    }


    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e) {
        //Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
        //        + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e) {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "touch start", e);

        //throw new NotImplementedException();
    }




    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e) {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e);
        Vector2 touchAxis = e.touchpadAxis;
        float touchAngle = e.touchpadAngle;
/*
        if (touchAngle <= 45 || touchAngle > 315) {
            typeIndex = 1;
        } else if (touchAngle > 45 && touchAngle <= 135) {
            typeIndex = 2;
        } else if (touchAngle > 135 && touchAngle <= 225) {
            typeIndex = 3;
        } else if (touchAngle > 225 && touchAngle <= 315) {
            typeIndex = 4;
        } else {
            typeIndex = 0;
        }
*/
        if (touchAngle > 0 && touchAngle <= 180) {
            typeIndex = 1;
        } else if (touchAngle > 180 && touchAngle <= 360) {
            typeIndex = 2;
        } else {
            typeIndex = 0;
        }

    }







    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e) {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e);
        
    }




    // 1st pad press {weapon_menu_shown == false; weapon_generated == false} -> show menu {weapon_menu_shown == true; weapon_generated == false}
    // 2st pad press {weapon_menu_shown == true; weapon_generated == false} -> generate object {weapon_menu_shown == false; weapon_generated == true}
    // 3rd trigger click {weapon_menu_shown == false; weapon_generated == true} -> emit object {weapon_menu_shown == false; weapon_generated == false}
   
    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e) {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "released", e);

        //switchMenu()
        if (!weapon_menu_shown && !weapon_generated)
        {
            weaponMenu.SetActive(true);
            weapon_menu_shown = true;
        }else if (weapon_menu_shown && !weapon_generated) {
            CreateObject(typeIndex, weaponList, gameController.transform.position, scaleValue);
            weapon_generated = true;
            weaponMenu.SetActive(false);
            weapon_menu_shown = false;
        }

        Debug.Log("-----------------" + weapon_menu_shown + "--------menu state" + weaponMenu.active) ;
    }


    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e) {
        DebugLogger(e.controllerIndex, "TRIGGER", "clicked", e);

		Vector3 force = gameController.transform.forward * 3f;
        Vector3 angle = Vector3.zero;
        if (currHandWeapon) { 
            EmitWeapon(currHandWeapon, force, angle);
            currHandWeapon = null;
            weapon_generated = false;
        }
    }


    void EmitWeapon(GameObject currWeapon, Vector3 emitForce, Vector3 emitAngle) {

        Rigidbody rig = currWeapon.GetComponent<Rigidbody>();
        //rig.AddForce(emitForce, ForceMode.Impulse);
		rig.velocity = emitForce;
        //rig.AddTorque(emitForce, ForceMode.Acceleration);
        rig.isKinematic = false;

    }







    void setMenu(GameObject attachPoint) {
        weaponMenu.gameObject.transform.parent = attachPoint.transform;
        weaponMenu.gameObject.transform.localRotation = Quaternion.identity;
        weaponMenu.gameObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
        weaponMenu.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        weaponMenu.SetActive(false);
    }



    void CreateObject(int typeIndex, Transform[] objList, Vector3 position, float scaleVar) {

        string objName = objList[typeIndex].name;

        Debug.Log("Create " + objName + " " + objList[typeIndex].ToString());

        GameObject orgGameObject = objList[typeIndex].gameObject;
        GameObject newGameObject = Instantiate(
            orgGameObject,
            position,
            Quaternion.identity
        ) as GameObject;

        newGameObject.transform.localScale *= scaleVar;
        countResList[typeIndex]++;
        newGameObject.name = objName + countResList[typeIndex].ToString();
        currHandWeapon = newGameObject;

    }

}

