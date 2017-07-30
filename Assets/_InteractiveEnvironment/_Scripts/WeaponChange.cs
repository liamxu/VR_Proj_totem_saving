using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour {

    // Controller
    SteamVR_TrackedObject gameController;

    // flash effect of firing
    public GameObject muzzleFlash;
    // position of muzzle
    [SerializeField]
    private Transform firePos;

    // Ray test result
    RaycastHit hit;

    void Awake() {
        gameController = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate() {
        var device = SteamVR_Controller.Input((int)gameController.index);       //SteamVR 自带，从Input中通过 index 获取device
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            GameObject go = GameObject.Instantiate(muzzleFlash);
            go.transform.position = firePos.position;
            go.transform.rotation = firePos.rotation;

            //如果射线检测有结果
            if (Physics.Raycast(firePos.position, firePos.forward, out hit, Mathf.Infinity)) {
                print(hit.collider.name);
            }
        }
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }



}
