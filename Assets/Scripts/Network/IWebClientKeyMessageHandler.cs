using UnityEngine;
using System.Collections;

public interface IWebClientKeyMessageHandler {

	string ClientKey
	{
		get;
	}

	int ClientKeyHash
	{
		get;
	}

	void ProcessMessage(SimpleJSON.JSONNode jsonNode);
}
