using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameBoard
{

	public float minLimit = 50;
	public float maxLimit = 75;

	public float GameLimit = 100f;

	public Vector3 Clamp(Vector3 center, Vector3 position)
	{
		Vector3 direction = position - center;
		direction.y = 0;

		float distance = direction.magnitude;

		if(distance < minLimit)
		{
			return minLimit*direction.normalized + center;
		}

		if(distance > maxLimit)
		{
			return maxLimit*direction.normalized + center;
		}

		return position;
	}

}
