using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;



public class LeapClient : MonoBehaviour {

	bool socketReady = false;

	TcpClient myClient;
	NetworkStream myStream;
	StreamWriter myWriter;
	StreamReader myReader;
	String host = "localhost";
	int port = 2543;


	public void initial() {
		try {
			myClient = new TcpClient(host, port);
			myStream = myClient.GetStream();
			myWriter = new StreamWriter(myStream);
			myReader = new StreamReader(myStream);
			socketReady = true;

		}
		catch(Exception e) {
			Debug.Log("socket initial error: " + e);
		}
	}

	public void writeSocket(String content) {
		if (!socketReady) {
			Debug.Log("scoket is not ready");
			return;
		}
		String myMsg = content + "\r\n";
		myWriter.Write(myMsg);
		myWriter.Flush();
	}

	public String readSocket() {
		if (!socketReady) {
			Debug.Log("socket is not ready");
			return "";
		}
		if (myStream.DataAvailable) {
			Debug.Log ("get response");
			return myReader.ReadLine();
		}
		Debug.Log("No reponse");
		return "";
	}

	public void closeSocket() {
		if (!socketReady) {
			Debug.Log("socket is not ready");
			return;
		}
        myWriter.Write("CLOSE");
		myWriter.Close();
		myReader.Close();
		myClient.Close();
		socketReady = false;
	}


	public void maintainConnection() {
		if (!myStream.CanRead) {
			initial();
		}
	}

}
