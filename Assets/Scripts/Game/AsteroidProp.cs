using UnityEngine;
using System.Collections;

public class AsteroidProp : MonoBehaviour, IDamagable {

	public float health = 10;
	//private GameManager manager;

	public int scorePoint = 10;

	public int split;

	public GameObject[] asteroidPrefabs;

	GameManager manager;

	public GameObject fxExplosion;

	public void Start()
	{
		
		manager = FindObjectOfType<GameManager>();
		manager.OnGameStateChange+= Manager_OnGameStateChange;
	}

	void OnDestroy()
	{
		manager.OnGameStateChange-= Manager_OnGameStateChange;
	}

	void Manager_OnGameStateChange (GameManager.GameState s)
	{


		if(s == GameManager.GameState.Ending || s== GameManager.GameState.Starting)
		{
			Destroy(this.gameObject);
		}
	}

	void Update()
	{
		if(transform.position.magnitude > manager.board.GameLimit)
		{
			Destroy(this.gameObject);
		}
	}

	private void Explode()
	{
		/*
		if(split >=2)
		{
			for(int i = 0; i < split; i++)
			{
				GameObject go =  Instantiate(asteroidPrefabs[Random.Range(0,asteroidPrefabs.Length)],Random.insideUnitCircle, Random.rotation) as GameObject;
				go.name = "SplitAsteroid";
				AsteroidProp ap = go.GetComponent<AsteroidProp>();
				ap.health = 10;
				ap.split= 0;
				ap.transform.localScale = this.transform.localScale*Mathf.Pow((float)split, 1.0f/3.0f);
			}
		}

		*/

		GameObject fx = Instantiate(fxExplosion,this.transform.position, Quaternion.identity) as GameObject;
		fx.GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier = this.transform.localScale.magnitude;

		Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {


		if(collision.gameObject.tag == "Player")
		{
			PlayerBase pb = collision.gameObject.GetComponent<PlayerBase>();
			pb.TakeHealthDamage(10,this.gameObject);
			//AutoDestroy();
			Explode();
		}

	}


	#region IDamagable implementation

	public void TakeHealthDamage (float healthDamage, GameObject fromObject)
	{
		health=-healthDamage;


		if(health <=0)
		{
			Explode();
		}
	}

	public void TakeHealthBonus (float healthBonus)
	{
		throw new System.NotImplementedException ();
	}

	public void TakeWeaponBonus (int i)
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
