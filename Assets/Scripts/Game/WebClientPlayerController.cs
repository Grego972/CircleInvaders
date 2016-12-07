using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerBase))]
public class WebClientPlayerController : AWebClientPlayerBehaviour{


	PlayerBase player;


	void Start()
	{
		player = GetComponent<PlayerBase>();
	}


	public void Update()
	{
		
		Vector2 axis = Vector2.zero;

		if(BtnUp)
			axis.y = 1f;
		else if(BtnDown)
			axis.y = -1f;

		if(BtnLeft)
			axis.x = -1f;
		else if(BtnRight)
			axis.x = 1f;

		player.Move(axis);

		player.Fire = BtnFire1;


	}






}
