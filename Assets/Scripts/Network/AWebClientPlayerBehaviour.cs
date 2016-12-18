using UnityEngine;
using System.Collections;

public abstract class AWebClientPlayerBehaviour : MonoBehaviour, IWebClientKeyMessageHandler {


	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	public delegate void SwipeEventDelegate(Direction direction);
	public event SwipeEventDelegate handleSwipeEvent;

	private string clientKey;
	private int clientKeyHash;

	public void Init(string key)
	{
		clientKey = key;
		clientKeyHash = key.GetHashCode();
		//Register
		ServerManager.Instance.RegisterClientPlayer(this);

	}

	void OnDestroy()
	{
		//Unregister
		ServerManager.Instance.UnregisterClientPlayer(this);
	}


	public bool BtnUp
	{
		get;
		private set;
	}

	public bool BtnDown
	{
		get;
		private set;
	}

	public bool BtnLeft
	{
		get;
		private set;
	}

	public bool BtnRight
	{
		get;
		private set;
	}

	public bool BtnFire1
	{
		get;
		private set;
	}

	public bool BtnFire2
	{
		get;
		private set;
	}




	#region IWebClientKeyMessageHandler implementation

	void IWebClientKeyMessageHandler.ProcessMessage (SimpleJSON.JSONNode jsonNode)
	{
		//string clientKey = jsonNode["webClientKey"];
	
		string buttonName = jsonNode["button"];
		bool status = jsonNode["status"].AsBool;

		Debug.Log("PressButton Client : " + clientKey +" -> " + buttonName + " ["+status+"]");

		switch(buttonName)
		{
		case "up":BtnUp = status;return;
		case "down":BtnDown = status;return;
		case "left":BtnLeft = status;return;
		case "right":BtnRight = status;return;
		case "fire":BtnFire1 = status;return;
		case "fire1":BtnFire1 = status;return;
		case "fire2":BtnFire2 = status;return;

		case "swipeUp":handleSwipeEvent.Invoke(Direction.Up);return;
		case "swipeDown":handleSwipeEvent.Invoke(Direction.Down);return;
		case "swipeLeft":handleSwipeEvent.Invoke(Direction.Left);return;
		case "swipeRight":handleSwipeEvent.Invoke(Direction.Right);return;
		}

		Debug.Log("[AWebClientPlayerBehaviour] Unknown command : [" + clientKey +"] -> " + buttonName + " ["+jsonNode["status"]+"]");
	}

	string IWebClientKeyMessageHandler.ClientKey {
		get {
			return clientKey;
		}
	}

	int IWebClientKeyMessageHandler.ClientKeyHash {
		get {
			return clientKeyHash;
		}
	}

	#endregion
}
