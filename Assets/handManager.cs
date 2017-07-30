using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;


public class handManager : MonoBehaviour {

    //public Text infoTextL;
    //public Text infoTextR;
    //public Text testText;
    //public Text boardText;
    private string[] stages;

    private int stageCount = 0;


    private Controller myController;
    //private LeapClient myClient;
    private GameObject myHandL;
    private GameObject myHandR;
    private GameObject myMenu;
    private GameObject myPoint;

    //Conditions check variables:
    //action check for pinch create object
    private bool doPinch = false;   // when both L&R pinch equal 1, set doPinch to true;
    private int pinchCount = 20;    // frames indicate create a object
    private bool startPinch = false;

    // used to count number of different objects
    private int[] countList;
    private int typeIndex = 0;
    private string createType = "MyFireBall";

    private int preNum = 0;

    // Use this for initialization
    void Start() {
        countList = new int[2];



        stages = new string[]
        {
        "Welcome! pose thumb up to continue.",                                                                // 0
        "Nice, next pose paper to continue.",                                                                 // 1
        "Great, Now pose rock to continue.",                                                                  // 2                                                    
        "Good job, now let us do some other tasks: pinch in by using both hands.",                            // 3
        "Get a cube, right? Let us get a sphere: open your left hand like paper pose but face to your eye, use right index finger to touch the small sphere, then, pinch in by both hands again",    // 4
        "Excellent! The tutorial is done. Don't forget to try to play the objects."                           // 5
        };
        
        Debug.Log("start to intial");
        // initial variables
        //myClient = new LeapClient();
        //myClient.initial();
        myController = new Controller();
        //boardText.text = stages[stageCount];

        Frame myFrame = myController.Frame();

        myMenu = GameObject.FindGameObjectWithTag("handMenu");
        myMenu.gameObject.transform.localScale *= 0;

        string msg = doHandAction(myFrame);

  
        //string result = askServer(msg);
        //updateInfo (result);
        updateTextInfo(msg);
    }

    // Update is called once per frame
    void Update() {
        if (myController.IsConnected) {
            Frame myFrame = myController.Frame();

            string msg = doHandAction(myFrame);
            //Debug.Log ("##### msg is:" + msg);

            //string result = askServer(msg);
            //updateInfo (result);
            updateTextInfo(msg);

            /*
			if (msg != "") {
				myClient.maintainConnection();
				myClient.writeSocket (msg);
				string tempMsg = myClient.readSocket ();
				Debug.Log (tempMsg);
				updateInfo (tempMsg);
				//myClient.closeSocket ();
			}
			*/
        }
    }

   /*   string askServer(string msg) {
        string result = "";
        if (msg != "") {
            myClient.maintainConnection();
            Debug.Log ("askserver 1");
            myClient.writeSocket(msg);
            Debug.Log ("askserver 2");
            result = myClient.readSocket();
            Debug.Log ("askserver 3");
            myClient.closeSocket();
            Debug.Log ("askserver 4");
            //
        }

        return result;      
    }
    */

    /*
    void updateAction(string msg) {
        string[] msgList = new string[] { };
        string[] msgL = new string[100];
        string[] msgR = new string[100];
        int indexL = 0;
        int indexR = 0;
        char[] pattern = new char[] { '#' };
        int side = 0;

        // specific name list for hand:
        string grabAngle = "GrabAngle:";
        string pinchStrength = "PinchStrength:";
        string pinchDistance = "PinchDistance:";
        string strPattern = ":";

        if (msg != "") {
            msgList = msg.Split(pattern, System.StringSplitOptions.RemoveEmptyEntries);
        }
        for (int x = 0; x < msgList.Length; x++) {
            if (msgList[x].Equals("left")) {
                msgL[indexL] = msgList[x];
                indexL++;
                side = 0;
            }
            else if (msgList[x].Equals("right")) {
                msgR[indexR] = msgList[x];
                indexR++;
                side = 1;
            }
            else {
                if (side == 0) {
                    if (msgList[x].Contains(grabAngle)) {
                        int tempI = msgList[x].IndexOf(strPattern) + 1;
                    }
                    indexL++;
                }
                else if (side == 1) {
                    msgR.AppendLine(msgList[x]);
                    indexR++;
                }
            }
        }


    }
    */
    void updateTextInfo(string msg) {
        string[] msgList = new string[] { };
        StringBuilder msgL = new StringBuilder("");
        StringBuilder msgR = new StringBuilder("");
        char[] pattern = new char[] { '#' };
        int side = 0; // 0 indicate left and 1 indicate right

        if( msg != "") {
            msgList = msg.Split( pattern, System.StringSplitOptions.RemoveEmptyEntries);
        }
        //Debug.Log("length of msglist:" + msgList.Length.ToString());        
        for(int x = 0; x < msgList.Length; x++) {
            if (msgList[x].Equals("left")) {
                msgL.AppendLine(msgList[x]);
                side = 0;
            }
            else if (msgList[x].Equals("right")) {
                msgR.AppendLine(msgList[x]);
                side = 1;
            } else {
                if (side == 0) {
                    msgL.AppendLine(msgList[x]);
                }
                else if (side == 1) {
                    msgR.AppendLine(msgList[x]);
                }
            }
        }
        //infoTextL.text = msgL.ToString();
        //infoTextR.text = msgR.ToString();
    }


   private GameObject tempObj;

    string doHandAction(Frame frame) {

        StringBuilder msg = new StringBuilder("");
        //string msg = "";

        List<Hand> hands = frame.Hands;
        // check hands and return infomation
        /*Debug.Log("hand number is:" + hands.Count.ToString());
		if (hands.Count > 0) {
			msg = hands[0].IsLeft? "left" : "right";
		}*/

        //needed variables:
        Vector3 hPosL = new Vector3();
        Vector3 hPosR = new Vector3();

        //conditions:
        bool doPinchL = false;
        bool doPinchR = false;
        bool openMenu = false;

       
        /***************************************************************************************
		the info order of hands: the function of hand could be called
		0. 	hand id									-- int id
		1. 	side: left, right 						-- bool isLeft
		2. 	pitch of hand 							-- float Direction.Pitch
		3. 	yaw of hand 							-- float Direction.Yaw
		4. 	roll of hand							-- float Direction.Roll
		5. 	position of hand palm 					-- Vector PalmPosition
		6. 	velocity of hand						-- Vector PalmVelocity
		7. 	width of hand							-- float PalmWidth
		8. 	distance between thumb/index fingers	-- float PinchDistance 
		9. 	pinch strength between thumb/others		-- float PinchStength    													-- 0 for the cloths hand
		10. rotation of hand as a quaternion		-- LeapQuaternion Rotation
		11. stabilized palm position of hand		-- Vector StabilizedPalmPosition
		12. duration of time the hand been visible	-- float TimeVisible
		13. position of wrist						-- Vector WristPosition
		note: Basis: the transform of the hand which return type is matrix object in version prior to 3.1 or LeapTransform
		14. confidence with a given hand pose		-- float Confidence
		15. the angle between fingers and the hand	-- float GrabAngle                                                          -- 3(3.14) for rock     -- 0.1 for cloths
		16. the stength of a grab hand pose			-- float GrabStrength
		
		(x, y, z)
		coordinate : left/right 		x
					 forword/back		y
					 up/down			z
					
		
		the info order of fingers:
		0.	finger id: handid + 0-4					-- int fingerId
		1.	the name of the finger					-- Finger.FingerType Type
		2. 	the position of the finger tip			-- Vector TipPosition
		3. 	the velocity of the finger tip			-- Vector TipVelocity
		4.  the estimated width of the finger		-- float Width
		5. 	the direction of fingers 				-- vector Direction
		
		the info order of bone:
		
		*********************************************************************************/

        for (int index = 0; index < hands.Count; index++) {
            
            

            Debug.Log("try to get hands infomation");
            Hand tempHand = hands[index];
            string handSide = "";
            
            // check which side of hand
            if (tempHand.IsLeft) {
                handSide = "left";
                
                myHandL = GameObject.FindGameObjectWithTag("HandLPalm");

                //HandAttachments tempHL = myHandL.GetComponent<HandAttachments>().;
                hPosL = myHandL.transform.position;
                //hPosL = myHandL..Palm.transform.position;//myHandL.transform.position;//tempHand.PalmPosition;
                //Debug.Log("left get transform position:" + myHandL.transform.position.ToString());
            }
            else {
                handSide = "right";
                myHandR = GameObject.FindGameObjectWithTag("HandRPalm");
                //HandAttachments tempHR = myHandR.GetComponent<HandAttachments>();
                hPosR = myHandR.transform.position;
                //hPosR = tempHR.Palm.transform.position;//myHandR.transform.position;//tempHand.PalmPosition;
                //Debug.Log("right get transform position:" + myHandR.transform.position.ToString());
            }

            //msg.Append ("#" + handSide);
            msg.Append("#" + handSide);

            Vector handCenter = tempHand.PalmPosition;
            float pitch = tempHand.Direction.Pitch;
            float yaw = tempHand.Direction.Yaw;
            float roll = tempHand.Direction.Roll;
            float nPitch = tempHand.PalmNormal.Pitch;
            float nYaw = tempHand.PalmNormal.Yaw;
            float nRoll = tempHand.PalmNormal.Roll;
            //float grabAngle = tempHand.GrabAngle;
            //float x = tempHand.PalmNormal.x;
            //float y = tempHand.PalmNormal.y;
            //float z = tempHand.PalmNormal.z;

            //float roll = tempHand.PalmNormal.Roll;

            //msg.Append( "#" + "hand position: " + handCenter.ToString ());
            //msg.AppendLine("hand position: " + handCenter.ToString());
            //msg.AppendLine ("hand normal position: " + x.ToString () + ", " + y.ToString () + ", " + z.ToString ());
            //msg.AppendLine("hand pitch: " + pitch.ToString());
            //msg.AppendLine("hand yaw: " + yaw.ToString());
            //msg.AppendLine("hand roll: " + roll.ToString());
            //msg.AppendLine("hand npitch: " + nPitch.ToString());
            //msg.AppendLine("hand nyaw: " + nYaw.ToString());
            //msg.AppendLine("hand nroll: " + nRoll.ToString());
            //msg.AppendLine("hand pinch strength: " + tempHand.PinchStrength.ToString());
            //msg.AppendLine("hand graph stength: " + tempHand.GrabStrength.ToString());
            //msg.AppendLine("hand confidence: " + tempHand.Confidence.ToString());
            //msg.AppendLine(grabAngle.ToString());

            //********** pose recognition ************************
            if (tempHand.PinchStrength == 0 && tempHand.GrabAngle > 3.0) {
                if(stageCount == 0) {
                    updateStage();
                }
                msg.Append("#" + "Pose:Thunb Up");
            }
            else if (tempHand.GrabAngle > 3.0 && tempHand.PinchStrength > 0.5) {
                if(stageCount == 2) {
                    preNum = countList[0];
                    updateStage();
                }

                msg.Append("#" + "Pose:Rock");
            }
            else if(tempHand.PinchStrength < 0.1 && tempHand.GrabAngle < 0.1) {
                if (tempHand.PalmNormal.z > 0) {
                    if(stageCount == 1) {
                        updateStage();
                    }
                    msg.Append("#" + "Pose:Paper down");
                }
                else if(tempHand.PalmNormal.z < 0) {
                    if (tempHand.IsLeft) {
                        openMenu = true;
                    }
                    msg.Append("#" + "Pose:Paper up");
                }
            }
            

            if(tempHand.IsLeft && tempHand.PinchStrength > 0.90) {
                doPinchL = true;
            }
            else if(tempHand.IsRight && tempHand.PinchStrength > 0.90) {
                doPinchR = true;
            }

            
            msg.Append("#" + "direction:" + tempHand.PalmNormal.ToString());
            msg.Append("#" + "GrabAngle: " + tempHand.GrabAngle.ToString());
            msg.Append("#" + "PinchStrength: " + tempHand.PinchStrength.ToString());
            msg.Append("#" + "PinchDistance: " + tempHand.PinchDistance.ToString());

            // information for fingers
            List<Finger> fingers = tempHand.Fingers;
            for (int n = 0; n < fingers.Count; n++) {
                Finger tempf = fingers[n];

                Vector positionf = tempf.TipPosition - handCenter;
                Vector directF = tempf.Direction;
                if (tempf.Type.Equals(Finger.FingerType.TYPE_THUMB)) {
                    //Debug.Log("position of hand: " + handCenter.ToString());
                    //Debug.Log("position of hand in vector3: " + handCenter.ToVector3());
                    //Debug.Log("position of thumb: " + tempf.TipPosition.ToString());
                    //Debug.Log("std position of thumb: " + positionf.ToString());
                }

                //msg.AppendLine ("finger name: " + tempf.Type.ToString () + " position: " + positionf.ToString());
                //msg.AppendLine("finger name: " + tempf.Type.ToString() + " direction: " + directF.ToString());
            }


            //Debug.Log("return hands info");
        }
        
        // do transform of the coordinate
   

        //************ action for pinch ***************
        if (doPinchL && doPinchR && !doPinch) {
            doPinch = true;
            //testText.text = "first get ture";

        }
        else if(doPinchL && doPinchR && !startPinch) {
            pinchCount--;
            //testText.text = "cout:" + pinchCount.ToString();

            float palmDist = calculateDistance(hPosL, hPosR);
            //testText.text = "distance:" + palmDist.ToString();

        }
        
        else if(!doPinchL || !doPinchR){
            doPinch = false;
            pinchCount = 20;
            startPinch = false;
            //testText.text = "get failed!!!!!!!";
        }



        if (doPinch && pinchCount == 0 && !startPinch) {
            //testText.text = "#####successful!!!!";

            float palmDist = calculateDistance(hPosL, hPosR);
            tempObj = CreateObject((hPosL + hPosR) / 2.0F, createType, palmDist);

            Debug.Log("###pre Num:" + preNum.ToString());
            Debug.Log("###number of cube:" + countList[0].ToString());

            if (stageCount == 3 && countList[0] > preNum) {
                preNum = countList[1];
                updateStage();
            }

            if (stageCount == 4 && countList[1] > preNum) {
                updateStage();
            }

            //Debug.Log("start to create cube");
            //Debug.Log("L:" + hPosL.ToString());
            //Debug.Log("R:" + hPosR.ToString());
            //CreateObject(hPosL);
            //CreateObject(myHandL.transform.position);
            //CreateObject(new Vector3(0f, 1.0f, 0f));
            //Debug.Log("done to create cube");
            startPinch = true;
            //doPinch = true;
            //pinchCount = 20;
        }
        else if (startPinch) {
            //testText.text = "set obj";
            tempObj.transform.position = (hPosL + hPosR) / 2.0F;
            tempObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * 4 * calculateDistance(hPosL, hPosR);
        }

 
        //************** action for open menu **************
        if (openMenu) {
            // myMenu.gameObject.transform
            myPoint = GameObject.FindGameObjectWithTag("HandRIndex");
            myMenu.gameObject.transform.parent = myHandL.transform;
            myMenu.gameObject.transform.localRotation = Quaternion.identity;
            myMenu.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);        
            myMenu.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            GameObject[] objList = GameObject.FindGameObjectsWithTag("createObj");
            

            foreach(GameObject tempObj in objList) {
                Collider temp = tempObj.GetComponent<Collider>();

                if (temp != null  && temp.bounds.Contains(myPoint.transform.position)) {
                    Debug.Log("find point:" + tempObj.name);
                    touchCheck touch = tempObj.GetComponent<touchCheck>();
                    touch.status = true;
                    setCreateType(tempObj.name);

                    
                }
            }
            foreach (GameObject tempObj in objList) {
                if(tempObj.name.Equals("FireSphere") && typeIndex == 1) {
                    touchCheck touch = tempObj.GetComponent<touchCheck>();
                    touch.status = false;
                }
                else if(tempObj.name.Equals("IceSphere") && typeIndex == 0) {
                    touchCheck touch = tempObj.GetComponent<touchCheck>();
                    touch.status = false;
                }
            }
  

            
        }else {

            myMenu.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            myMenu.gameObject.transform.parent = null;
        }


        //return msg;
        return msg.ToString();
    }

    float calculateDistance(Vector3 palm1, Vector3 palm2) {
        float distance = 0f;
        Vector3 diff = palm1 - palm2;
        distance = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);
        return distance;
    }



    GameObject CreateObject(Vector3 position, string objName, float palmDistance) {

        Vector3 deltaVector = new Vector3(0.0f, 0.0f, 0.0f);
        GameObject geneCube = Resources.Load(objName) as GameObject;         
        geneCube.name = objName + countList[typeIndex].ToString();

        Debug.Log("Create " + objName + " " + countList[typeIndex].ToString());

        geneCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * 4 * palmDistance;

        GameObject newGameObject = Instantiate(
            geneCube,
            position - deltaVector,
            Quaternion.identity
        ) as GameObject;

        countList[typeIndex]++;
        return newGameObject;

    }

    public void setCreateType(string typeName) {
        if (typeName.Equals("FireSphere"))
        {
            //createType = "InteractionCube";
            createType = "MyFireBall";
            Debug.Log("set create MyFireBall");
            typeIndex = 0;
        }
        else if (typeName.Equals("IceSphere")) {
            //createType = "InteractionSphere
            createType = "MyIceBall";
            Debug.Log("set create MyIceBall");
            typeIndex = 1;
        }
        

    }

    private void updateStage() {
        stageCount++;
        //boardText.text = stages[stageCount];
    }

}
