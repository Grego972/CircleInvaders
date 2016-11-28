using UnityEngine;
using System.Collections;

public class ServerManagerIO : MonoBehaviour {

	public string serverAdress;

	SocketIOClient.Client client;

	// Use this for initialization
	void OnEnable () {
		client = new SocketIOClient.Client(serverAdress+"/?transport=polling");



		client.Opened += SocketOpened;
		client.Message += SocketMessage;
		client.Error += SocketError;
		client.SocketConnectionClosed += SocketConnectionClosed;

		client.Connect();
	}

	void OnDisable()
	{
		client.Close();

	}


	private void SocketOpened(object sender, System.EventArgs e) {
		//invoke when socket opened

		Debug.Log("Connected");
		client.Send("Hello from Unity");
	}

	private void SocketMessage (object sender, SocketIOClient.MessageEventArgs e) {
		if ( e!= null)
		{
			Debug.Log("Socket Receive Message : " + e.Message);

		}
	}

	private void SocketConnectionClosed(object sender, System.EventArgs e) {
		//invoke when socket closed
		Debug.Log("SocketConnectionClosed : " + e.ToString());
	}

	private void SocketError(object sender, SocketIOClient.ErrorEventArgs e) {
		//invoke when a socket Error occur
		if ( e!= null) {
			Debug.LogError("Socket Error : " + e.Message + " / " +e.Exception.ToString());
		}


	}
}
