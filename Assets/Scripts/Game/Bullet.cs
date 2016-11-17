using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

	public float damage;
	public float speed = 5f;
	public float destroyAfter = 5f;


	private  Rigidbody myRigidbody;
	GameManager manager;

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

		//CancelInvoke("AutoDestroy");
		AutoDestroy();
	}


	private void AutoDestroy()
	{
		Destroy(this.gameObject);
	}
}
