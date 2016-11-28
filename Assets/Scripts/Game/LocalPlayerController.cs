using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerBase))]
public class LocalPlayerController : MonoBehaviour{


	PlayerBase player;


	void Start()
	{
		player = GetComponent<PlayerBase>();
	}


	public void Update()
	{
		
		Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		player.Move(axis);

		player.Fire = Input.GetButton("Jump");
		/*
		foreach(KeyCode kcode in SystemEnum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kcode))
				Debug.Log("KeyCode down: " + kcode);
		}
		*/

		if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.W) )
		{
			//Debug.Log("Key press");
			player.ChangeWeapon(0);
		}

		if(Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.X))
		{
			player.ChangeWeapon(1);
		}

		if(Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.C))
		{
			player.ChangeWeapon(2);
		}

		if(Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.V))
		{
			player.ChangeWeapon(3);
		}

		if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.B))
		{
			player.ChangeWeapon(4);
		}


	}






}
