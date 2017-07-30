using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchCheck : MonoBehaviour {

    public bool status = false;
    Vector3 position;
    Vector3 scale;
    Material mat1;
    Material mat2;
    public Renderer rend;


    void Start() {
        //gameObject.GetComponent<Rigidbody>().transform.localPosition = gameObject.transform.localPosition;
        //gameObject.GetComponent<Rigidbody>().transform.localRotation = gameObject.transform.localRotation;
        position = gameObject.transform.localPosition;
        scale = gameObject.transform.localScale;
        rend = GetComponent<Renderer>();
        mat1 = rend.material;
        mat2 = Resources.Load("_Shader/Silhouette") as Material;
        //Debug.Log("!!!!!!!!!!!!!!!-----shader name is :" + mat2.shader.name);
        updateSilhouette();
    }

    void Update() {
        gameObject.transform.localPosition = position;
        gameObject.transform.localScale = scale;
        //Debug.Log("update rigi pos:" + gameObject.GetComponent<Rigidbody>().transform.localPosition.ToString());
        //Debug.Log("update rigi rot:" + gameObject.GetComponent<Rigidbody>().transform.localRotation.ToString());
        updateSilhouette();


    }

    void OnTriggerEnter(Collider collision) {
        GameObject myParent = GameObject.FindGameObjectWithTag("mainController");
        handManager myManager = myParent.GetComponent<handManager>();
        myManager.setCreateType(gameObject.name);
        Debug.Log("!!!get collision with:" + collision.gameObject.name);
        Debug.Log("!!!get collision:" + gameObject.name);
    }

    void updateSilhouette() {
        if (status) {
            rend.material = mat2;
        } 
        else {
            rend.material = mat1;
        }
    }

    /*void OnCollisionEnter(Collision collision) {
        //if(collision.gameObject.name == "Index") {

        //GameObject myParent = GameObject.FindGameObjectWithTag("mainController");
        //handManager myManager= myParent.GetComponent<handManager>();
        //myManager.setCreateType(gameObject.name);
        Debug.Log("!!!get collision with:" + collision.gameObject.name);
        Debug.Log("!!!get collision:" + gameObject.name);
            

        //}
    }
    */
    /*
    public bool getStatus() {
        return active;
    }

    public void setStatus(bool status) {
        active = status;
    }
    */
}
