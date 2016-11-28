using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

	public float damage;
	public float speed = 5f;
	public float destroyAfter = 5f;



	private  Rigidbody myRigidbody;
	GameManager manager;

	PlayerBase currentShooter;

	void OnEnable () {
	
		Invoke("AutoDestroy",destroyAfter);
		myRigidbody = GetComponent<Rigidbody>();
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
		myRigidbody.velocity = transform.TransformDirection( Vector3.forward * speed);
	}

	void OnTriggerEnter(Collider other) {

		if(other.tag == "Asteroid")
		{
			AsteroidProp ap = other.GetComponent<AsteroidProp>();
			ap.TakeHealthDamage(10000000f,currentShooter.gameObject);
			currentShooter.Score += ap.scorePoint;

		}
		else if(other.tag == "Player")
		{
			PlayerBase pb = other.GetComponent<PlayerBase>();
			pb.TakeHealthDamage(damage,currentShooter.gameObject);
			currentShooter.Score += Mathf.RoundToInt( pb.Score * 0.2f);
		}

		//CancelInvoke("AutoDestroy");
		AutoDestroy();
	}




	private void AutoDestroy()
	{
		Destroy(this.gameObject);
	}


	public void SetShooter(PlayerBase _player)
	{
		currentShooter = _player;
	}
}
