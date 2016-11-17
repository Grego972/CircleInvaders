using UnityEngine;
using System.Collections;

public class RotateBehaviour : MonoBehaviour {

	public Vector3 axis;
	public float speed;
	public Space space = Space.World;

	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(axis, speed*Time.deltaTime,space); 
	}
}
