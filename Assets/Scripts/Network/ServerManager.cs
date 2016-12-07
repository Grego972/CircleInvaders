using UnityEngine;
using System.Collections;

public class ServerManager : MonoBehaviour {


	private static ServerManager instance = null;

	public ServerConfigXml config;

	public bool loadXmlConfig = false;
	public string configFilename = "config.xml";

	WebSocketSharp.WebSocket ws;

	private bool isConnected = false;

	//private System.Collections.Generic.Dictionary<string, IWebClientKeyMessageHandler> webClients = new System.Collections.Generic.Dictionary<string, IWebClientKeyMessageHandler> ();
	private System.Collections.Generic.Dictionary<int, IWebClientKeyMessageHandler> webClients = new System.Collections.Generic.Dictionary<int, IWebClientKeyMessageHandler> ();

	// Use this for initialization
	void OnEnable () {

		if(loadXmlConfig)
			LoadConfig();

		ws = new WebSocketSharp.WebSocket (config.adress);

		ws.EmitOnPing = true;

		ws.OnOpen += SocketOpened;
		ws.OnMessage += SocketMessage;
		ws.OnError += SocketError;
		ws.OnClose += SocketConnectionClosed;

		isConnected = false;

		ws.Connect();
	}

	void OnDisable()
	{
		
		ws.Close();
		isConnected = false;
	}

	public static ServerManager Instance
	{
		get
		{
			if (instance == null)
			{
				
				instance = FindObjectOfType<ServerManager>();
				if (instance == null)
				{
					Debug.LogError("[ServerManager] Manager not found");
				}
			}
			return instance;
		}
	}


	public bool IsConnected
	{
		get
		{
			return isConnected;
		}
	}

	private void SocketOpened(object sender, System.EventArgs e) {
		//invoke when socket opened

		Debug.Log("Connected");
		ws.Send("Hello from Unity");

		System.Collections.Generic.Dictionary<string,object> dict = new System.Collections.Generic.Dictionary<string, object>();
		dict.Add("Test","Hello");
		dict.Add("name",config.name);
		dict.Add("roomSize",config.roomSize);

		Debug.Log(SimpleJson.SimpleJson.SerializeObject(dict));
		ws.Send(SimpleJson.SimpleJson.SerializeObject(dict));
		//ws.Send(UnityEngine.JsonUtility.ToJson(dict));
		//ws.Send(UnityEngine.JsonUtility.ToJson(config));

		isConnected = true;
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

				if(!ParseMessage(e.Data))
					Debug.Log("Unparse Message : " + e.Data);
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
		isConnected = false;
	}

	private void SocketError(object sender, WebSocketSharp.ErrorEventArgs e) {
		//invoke when a socket Error occur
		if ( e!= null) {
			Debug.LogError("Socket Error : " + e.Message + " / " +e.Exception.ToString());
		}


	}

	[ContextMenu("Load Configuration file")]
	public void LoadConfig()
	{
		LoadConfig(Application.streamingAssetsPath + "/" + configFilename);
	}

	public void LoadConfig(string filePath)
	{
		System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ServerConfigXml));
		System.IO.StringReader str = new System.IO.StringReader( System.IO.File.ReadAllText(filePath));
		ServerConfigXml cfg = null;
		try
		{
			cfg =(ServerConfigXml) xmlSerializer.Deserialize(str) ;
		}
		catch(System.Exception ex)
		{
			Debug.Log("Error can't deserialized object of type ServerConfigXml");
			Debug.LogError(ex.Message);
		}

		if(cfg != null)
		{
			config = cfg;
		}

	}

	[ContextMenu("Save Configuration file")]
	public void SaveConfigFile()
	{
		SaveConfigFile(Application.streamingAssetsPath + "/" + configFilename);
	}

	public void SaveConfigFile(string filePath)
	{
		System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ServerConfigXml));
		System.IO.TextWriter writer = new System.IO.StreamWriter(filePath);
		xmlSerializer.Serialize(writer,config);
		writer.Close();

	}


	private bool ParseMessage(string msg)
	{
		SimpleJSON.JSONNode json = null;
		try
		{
			json = SimpleJSON.JSON.Parse(msg);
		}
		catch(System.Exception ex)
		{
			Debug.LogError("Json Error : " + ex.Message);
		}

		if(json == null)
			return false;


		bool hasBeenParse = false;

		if(json["webClients"] != null)
		{
			ReadClientList(json["webClients"]);

			hasBeenParse = true;
		}

		if(json["pressButton"] != null)
		{
			//ReadPressButton(json["pressButton"]);
			SimpleJSON.JSONNode pressButton = json["pressButton"];
			DispatchClientPlayerMessage(pressButton["webClients"].GetHashCode(),pressButton);
			hasBeenParse = true;
		}

		return hasBeenParse;
	}


	private System.Collections.Generic.List<string> _clientList = new System.Collections.Generic.List<string>();

	public System.Collections.Generic.List<string> ClientList
	{
		get
		{
			return _clientList;
		}
	}

	private void ReadClientList(SimpleJSON.JSONNode clientsNode)
	{
		_clientList.Clear();
		Debug.Log("Found " + clientsNode.Count +" Clients");
		for(int i = 0 ; i< clientsNode.Count ; i++)
		{
			Debug.Log("Found Client : " + clientsNode[i]["key"]);
			_clientList.Add(clientsNode[i]["key"]);
		}
	}

	private void ReadPressButton(SimpleJSON.JSONNode jsonNode)
	{
		string clientKey = jsonNode["webClientKey"];
		string buttonName = jsonNode["button"];
		string status = jsonNode["status"];

		Debug.Log("PressButton Client : " + clientKey +" -> " + buttonName + " ["+status+"]");

	}

	private void DispatchClientPlayerMessage(int key, SimpleJSON.JSONNode jsonNode)
	{
		if(webClients.ContainsKey(key))
		{
			webClients[key].ProcessMessage(jsonNode);
		}
	}

	public void RegisterClientPlayer(IWebClientKeyMessageHandler client)
	{
		if(!webClients.ContainsKey(client.ClientKeyHash))
		{
			webClients.Add(client.ClientKeyHash,client);
		}
	}

	public void UnregisterClientPlayer(IWebClientKeyMessageHandler client)
	{
		if(webClients.ContainsKey(client.ClientKeyHash))
		{
			webClients.Remove(client.ClientKeyHash);
		}
	}



}
