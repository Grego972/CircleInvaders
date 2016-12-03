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

	public UiPlayerInfo info;

	public GameObject fxExplosion;

	private float startPlayerHealth;

	private float currentFireCount;

	public void Init(GameManager m)
	{
		manager = m;

		this.transform.LookAt(Vector3.zero,Vector3.forward);

		WeaponSettings w = manager.GetWeapon(currentWeaponIndex);
		currentFireCount = w.fireCount;

		startPlayerHealth = playerHealth;
		if(info != null)
		{
			info.PlayerName = playerName;
			info.NormalizedLife = 1f;
			info.NormalizedWeapon = 1f;
			info.WeaponCircleVisibility = currentFireCount != -1;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(manager.State == GameManager.GameState.Running)
		{
			//UpdateControl();
			if(IsAlive)
				UpdateWeapon();
		}
	}

	public void Move(Vector2 normalizedDirection)
	{
		if(!IsAlive)
			return;
		
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
		if(w.fireCount != -1)
		{
			if(currentFireCount <=0)
			{
				ChangeWeapon(0);
				return;
			}
			currentFireCount --;

			if(info)
			{
				info.NormalizedWeapon = (float) currentFireCount / (float) w.fireCount;
			}
		}

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

		currentFireCount = manager.GetWeapon(currentWeaponIndex).fireCount;

		if(info)
		{
			info.WeaponCircleVisibility = currentFireCount != -1;
			info.NormalizedWeapon = 1f;
		}

	}

	public bool IsAlive
	{
		get
		{
			return playerHealth>0f;
		}
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

				if(info != null)
				{
					info.NormalizedLife = playerHealth / startPlayerHealth;
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

				if(info != null)
				{
					info.PlayerScore = playerScore;
				}
			}
		}
	}

	private void PlayerDie()
	{
		//TODO

		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		Collider[] colliders = GetComponentsInChildren<Collider>();

		for(int i  = 0 ; i < renderers.Length ; i++)
		{
			renderers[i].enabled = false;
		}

		for(int i  = 0 ; i < colliders.Length ; i++)
		{
			colliders[i].enabled = false;
		}

		GameObject fx = Instantiate(fxExplosion,this.transform.position, Quaternion.identity) as GameObject;
		//fx.GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier = this.transform.localScale.magnitude;

		if(info)
		{
			info.gameObject.SetActive(false);
		}

		//Destroy(this.gameObject);
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
