using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction.CApi;

public class objTouchCheck : MonoBehaviour {
    Material mat1;
    Material mat2;
    public Renderer rend;
    

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        mat1 = rend.material;
        mat2 = Resources.Load("_Shader/Silhouette2") as Material;
        Debug.Log("!!!!!material:" + mat1.name);
    }

    // Update is called once per frame
    void Update() {
        GameObject lHand = GameObject.Find("HandAttachments-L");
        GameObject rHand = GameObject.Find("HandAttachments-R");
        /*
        Vector3[] positions = new Vector3[8];
        if (lHand.transform.Find("Middle")) {
            positions[0] = lHand.transform.Find("Middle").transform.position;
        } 
        if (lHand.transform.Find("Index")) {
            positions[1] = lHand.transform.Find("Index").transform.position;
        }
        if (lHand.transform.Find("Thumb")) {
            positions[2] = lHand.transform.Find("Thumb").transform.position;
        } 
        if (lHand.transform.Find("Palm")) {
            positions[3] = lHand.transform.Find("Palm").transform.position;
        } 
        if (rHand.transform.Find("Middle")) {
            positions[4] = rHand.transform.Find("Middle").transform.position;
        } 
        if (rHand.transform.Find("Index")) {
            positions[5] = rHand.transform.Find("Index").transform.position;
        } 
        if (rHand.transform.Find("Thumb")) {
            positions[6] = rHand.transform.Find("Thumb").transform.position;
        } 
        if (rHand.transform.Find("Palm")) {
            positions[7] = rHand.transform.Find("Palm").transform.position;
        }

        
        for(int x = 0; x < positions.Length; x++) {
            if (gameObject.GetComponent<Collider>().bounds.Contains(positions[x])) {
                touched = true;
            }
        }


        */
        bool touched = false;

        if(lHand != null) {
            if (gameObject.GetComponent<Collider>().bounds.Contains(lHand.transform.Find("Middle").transform.position)  ||
                gameObject.GetComponent<Collider>().bounds.Contains(lHand.transform.Find("Index").transform.position)   ||
                gameObject.GetComponent<Collider>().bounds.Contains(lHand.transform.Find("Thumb").transform.position)   ||
                gameObject.GetComponent<Collider>().bounds.Contains(lHand.transform.Find("Palm").transform.position)) {
                touched = true;
                    
            }
        }
        if(rHand != null) {
            if (gameObject.GetComponent<Collider>().bounds.Contains(rHand.transform.Find("Middle").transform.position)  ||
                gameObject.GetComponent<Collider>().bounds.Contains(rHand.transform.Find("Index").transform.position)   ||
                gameObject.GetComponent<Collider>().bounds.Contains(rHand.transform.Find("Thumb").transform.position)   ||
                gameObject.GetComponent<Collider>().bounds.Contains(rHand.transform.Find("Palm").transform.position)) {
                touched = true;
            }
    }



        if (touched)
        {
            rend.material = mat2;

        } else {
            rend.material = mat1;
        }

}

    /*void OnTriggerStay(Collider collision) {
        GameObject collideObj = collision.gameObject;
        if (collideObj.name.Equals("BrushHand_L") || collideObj.name.Equals("BrushHand_R")) {
            rend.material = mat2;
        }

    }

    void OnTriggerExit(Collider collision) {
        GameObject collideObj = collision.gameObject;
        if (collideObj.name.Equals("BrushHand_L") || collideObj.name.Equals("BrushHand_R")) {
            rend.material = mat1;
        }

    }

    void OnCollisionStay(Collision collision) {
        GameObject collideObj = collision.gameObject;
        if(collideObj.name.Equals("BrushHand_L") || collideObj.name.Equals("BrushHand_R")) {
            rend.material = mat2;
        }
        
    }

    void OnCollisionExit(Collision collision) {
        GameObject collideObj = collision.gameObject;
        if (collideObj.name.Equals("BrushHand_L") || collideObj.name.Equals("BrushHand_R")) {
            rend.material = mat1;
        }

    }
    */

}
