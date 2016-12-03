using UnityEngine;
using System.Collections;

public class UiPlayerInfo : MonoBehaviour {


	[SerializeField]
	private UnityEngine.UI.Text playerName;

	[SerializeField]
	private UnityEngine.UI.Text playerScore;


	[SerializeField]
	private UnityEngine.UI.Extensions.UICircle lifeCircle;

	[SerializeField]
	private UnityEngine.UI.Extensions.UICircle weaponCircle;


	public string PlayerName
	{
		set
		{
			if(playerName != null)
			{
				playerName.text = value;
			}
		}
	}

	public int PlayerScore
	{
		set
		{
			if(playerScore != null)
			{
				playerScore.text = value.ToString();
			}
		}
	}


	public float NormalizedLife
	{
		set
		{
			if(lifeCircle != null)
			{
				lifeCircle.fillPercent =Mathf.RoundToInt( value*100);
				lifeCircle.SetVerticesDirty();
			}
		}
	}

	public float NormalizedWeapon
	{
		set
		{
			if(weaponCircle != null)
			{
				weaponCircle.fillPercent =Mathf.RoundToInt( value*100);
				weaponCircle.SetVerticesDirty();
			}
		}
	}

	public bool WeaponCircleVisibility
	{
		get
		{
			if(weaponCircle != null)
			{
				return weaponCircle.gameObject.activeInHierarchy;
			}
			return false;

		}

		set
		{
			if(weaponCircle != null)
			{
				weaponCircle.gameObject.SetActive(value);
			}

		}
	}


	void Update()
	{
		this.transform.rotation = Quaternion.LookRotation(-1*Vector3.up,Vector3.forward);
	}
}
