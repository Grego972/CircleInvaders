using UnityEngine;
using System.Collections;

public class AsteroidManager : MonoBehaviour {




	public GameManager manager;
	[Space]
	public GameObject[] asteroidPrefabs;
	[Space]
	public float minInstanciateTime = .5f;
	public float maxInstanciateTime = 4f;
	[Space]
	public float minScaleSize = 4f;
	public float maxScaleSize = 10f;
	[Space]
	public float minVelocity = 6f;
	public float maxVelocity = 10f;
	public float initialAngularVelocity = 5f;

	[Space]
	public float zoneRadius = 5f;

	[Space]
	public LayerMask checkLayerMask;



	private float nextPropTime = 0;

	// Use this for initialization
	void OnEnable () {
		manager.OnGameStateChange += Manager_OnGameStateChange;

	}

	void OnDisable()
	{
		manager.OnGameStateChange -= Manager_OnGameStateChange;
	}


	void Manager_OnGameStateChange (GameManager.GameState s)
	{
		if(s == GameManager.GameState.Starting)
		{
			InitNextInstanciate();
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		GuizmoCircle(zoneRadius);


	}

	private void GuizmoCircle(float radius, int slice = 100)
	{
		float inc = 360f/(float)slice;
		Vector3 p0 = new Vector3(1f,0f,0f);
		for(int i = 1 ; i<=slice; i++)
		{
			float a =Mathf.Deg2Rad*inc * (float) i;
			Vector3 p1 = new Vector3(Mathf.Cos(a),0f,Mathf.Sin(a));
			Gizmos.DrawLine(p0*radius, p1*radius);
			p0 = p1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(manager.State == GameManager.GameState.Running)
		{
			nextPropTime -= Time.deltaTime;

			if(nextPropTime <= 0f)
			{
				InstanciateAsteroid();
				InitNextInstanciate();
			}
		}

	}


	void InitNextInstanciate()
	{
		nextPropTime = Random.Range(minInstanciateTime, maxInstanciateTime);
	}
	private static Vector3 ToVector3(Vector2 v)
	{
		return new Vector3(v.x,0,v.y);
	}

	void InstanciateAsteroid()
	{
		Vector3 asteroidScale = (Vector3.one +Random.insideUnitSphere*0.1f) *Random.Range(minScaleSize, maxScaleSize);

		Vector3 asteroidPosition =ToVector3( Random.insideUnitCircle*2f*zoneRadius);
		float asteroidRadius = asteroidScale.magnitude;
		int tryCount = 10;
		while(Physics.CheckSphere(asteroidPosition,asteroidRadius,checkLayerMask) && tryCount >0)
		{
			asteroidPosition =ToVector3( Random.insideUnitCircle*2f*zoneRadius);
			tryCount--;
		}

		if(tryCount == 0)
		{
			//Fail to find a good position for the new asteroid
			Debug.LogWarning("Fail to find a good position for the new asteroid");
			return;
		}

		GameObject go =  Instantiate(asteroidPrefabs[Random.Range(0,asteroidPrefabs.Length)],asteroidPosition, Random.rotation) as GameObject;
		go.transform.localScale = asteroidScale;
		AsteroidProp a = go.GetComponent<AsteroidProp>();
		a.health = 10;
		a.split= 2;

		Rigidbody r = go.GetComponent<Rigidbody>();
		r.velocity = ToVector3(Random.insideUnitCircle.normalized*Random.Range(minVelocity,maxVelocity));
		r.angularVelocity = Random.insideUnitSphere* initialAngularVelocity;
	}
}
