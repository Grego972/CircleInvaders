using UnityEngine;
using System.Collections;

public class ServerManager : MonoBehaviour {

	public string serverAdress;

	WebSocketSharp.WebSocket ws;

	// Use this for initialization
	void OnEnable () {
		ws = new WebSocketSharp.WebSocket (serverAdress);

		ws.EmitOnPing = true;

		ws.OnOpen += SocketOpened;
		ws.OnMessage += SocketMessage;
		ws.OnError += SocketError;
		ws.OnClose += SocketConnectionClosed;

		ws.Connect();
	}

	void OnDisable()
	{
		ws.Close();

	}


	private void SocketOpened(object sender, System.EventArgs e) {
		//invoke when socket opened

		Debug.Log("Connected");
		ws.Send("Hello from Unity");
	}

	private void SocketMessage (object sender, WebSocketSharp.MessageEventArgs e) {
		if ( e!= null)
		{
			if(e.IsPing)
			{
				Debug.Log("Socket Receive PING ");
			}
			else if(e.IsText)
			{
				Debug.Log("Socket Receive Message : " + e.Data);
			}
			else if(e.IsBinary)
			{
				Debug.Log("Socket Receive Binary ");
			}
			else
			{
				Debug.Log("Socket Receive Unknow ");
			}
		}
	}

	private void SocketConnectionClosed(object sender, WebSocketSharp.CloseEventArgs e) {
		//invoke when socket closed
		Debug.Log("SocketConnectionClosed : " + e.Reason);
	}

	private void SocketError(object sender, WebSocketSharp.ErrorEventArgs e) {
		//invoke when a socket Error occur
		if ( e!= null) {
			Debug.LogError("Socket Error : " + e.Message + " / " +e.Exception.ToString());
		}


	}
}
