using UnityEngine;
using System.Collections;

public class LocalPlayer : AbstractPlayer {
	#region implemented abstract members of AbstractPlayer


	public override void UpdateControl ()
	{
		
		Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		move(axis);

		Fire = Input.GetButton("Jump");
	}

	#endregion



}
