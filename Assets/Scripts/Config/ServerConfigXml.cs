using UnityEngine;
using System.Collections;

using System.Xml.Serialization;

[System.Serializable]
[XmlType("ServerConfig")]
public class ServerConfigXml {

	[XmlAttribute("adress")]
	public string adress;


	[XmlAttribute("name")]
	public string name;

	[XmlAttribute("roomSize")]
	public string roomSize;

}
