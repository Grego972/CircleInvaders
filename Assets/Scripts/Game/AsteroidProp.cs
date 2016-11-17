using UnityEngine;
using System.Collections;

public class AsteroidProp : MonoBehaviour {

	public float health = 10;
	//private GameManager manager;


	public int split;

	public GameObject[] asteroidPrefabs;

	GameManager manager;

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

	public void TakeDamage(float d)
	{
		health=-d;


		if(health <=0)
		{
			Explode();
		}
	}

	private void Explode()
	{
		if(split >1)
		{
			for(int i = 0; i < split; i++)
			{
				AsteroidProp go =  Instantiate(asteroidPrefabs[Random.Range(0,asteroidPrefabs.Length)],Random.insideUnitCircle, Random.rotation) as AsteroidProp;
				go.health = 10;
				go.split= 0;
				go.transform.localScale = this.transform.localScale*Mathf.Pow(go.split, 1.0f/3.0f);
			}
		}
	}
}
