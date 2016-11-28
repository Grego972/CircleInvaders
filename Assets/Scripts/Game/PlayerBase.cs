using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour, IDamagable {


	private GameManager manager;

	public string playerName;

	[SerializeField]
	private float playerHealth;

	//[SerializeField]
	private int playerScore;

	public Vector2 speed;

	public Vector2 position;

	public Transform bulletSpawnTransform;
	//public GameObject bulletPrefab;
	public int currentWeaponIndex = 0;

	public AudioSource audioSource;

	public void Init(GameManager m)
	{
		manager = m;

		this.transform.LookAt(Vector3.zero,Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {

		if(manager.State == GameManager.GameState.Running)
		{
			//UpdateControl();
			UpdateWeapon();
		}
	}

	public void Move(Vector2 normalizedDirection)
	{
		
		Vector3 p = this.transform.TransformPoint(new Vector3( normalizedDirection.x*speed.x,0,normalizedDirection.y*speed.y) * Time.deltaTime);
		this.transform.position = manager.board.Clamp(Vector3.zero,p);

		this.transform.LookAt(Vector3.zero,Vector3.up);
	}


	private bool _fire = false;
	private bool _fireJustPress = false;
	private float _lastFireTime = 0;

	//
	public bool Fire
	{
		get
		{
			return _fire;
		}

		set
		{
			if(value != _fire)
			{
				_fire = value;
				if(_fire)
				{
					_fireJustPress = true;
				}
			}
		}
	}

	private void UpdateWeapon()
	{
		WeaponSettings w = manager.GetWeapon(currentWeaponIndex);

		if(_fire && (w.allowContinuisPress || _fireJustPress))
		{
			if( (Time.time - _lastFireTime) >= w.firePeriod )
			{
				FireWeapon(w);
				_lastFireTime = Time.time;
			}
		}

		_fireJustPress = false;

	}
	public bool doPitch = true;
	public float pitchVal = 0.2f;

	private void FireWeapon(WeaponSettings w)
	{
		//Vector3 p = bulletSpawnTransform.position ;
		Vector3 p = bulletSpawnTransform.TransformPoint(Vector3.right * Random.Range(-w.jitter,w.jitter) );
		//Quaternion rot = bulletSpawnTransform.rotation;
		Quaternion rot = bulletSpawnTransform.rotation * Quaternion.AngleAxis(Random.Range(-w.angleJitter,w.angleJitter),Vector3.up);

		for(int i = 0; i< w.prefabs.Length ; i++)
		{
			WeaponSettings.WeaponSettingsItem prefab = w.prefabs[i];
			GameObject go = Instantiate(prefab.bulletPrefabs,p + bulletSpawnTransform.TransformVector(prefab.offset),rot*Quaternion.AngleAxis(prefab.angle,Vector3.up) ) as GameObject;	
			go.GetComponent<Bullet>().SetShooter(this);
		}

		if(doPitch)
			audioSource.pitch =1f +  Random.Range(-pitchVal, pitchVal);
		
		audioSource.PlayOneShot(w.soundFx);
	}


	public void ChangeWeapon(int idx)
	{
		currentWeaponIndex = Mathf.Clamp(idx,0,manager.GetWeaponCount());
	}


	public float Health
	{
		get
		{
			return playerHealth;
		}

		private set
		{
			if(value != playerHealth)
			{
				playerHealth = value;
				if(playerHealth <= 0f)
				{
					playerHealth = 0;
					PlayerDie();
				}
			}
		}
	}

	public int Score
	{
		get
		{
			return playerScore;
		}

		set
		{
			if(value != playerScore)
			{
				playerScore = value;
			}
		}
	}

	private void PlayerDie()
	{
		//TODO
	}

	#region IDamagable implementation

	public void TakeHealthDamage (float healthDamage, GameObject fromObject)
	{
		Health -= healthDamage;

	}

	public void TakeHealthBonus (float healthBonus)
	{
		Health += healthBonus;
	}

	public void TakeWeaponBonus (int i)
	{
		ChangeWeapon(i);
	}

	#endregion

}
