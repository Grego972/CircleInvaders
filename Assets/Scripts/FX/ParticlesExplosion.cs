using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesExplosion : MonoBehaviour {




	public bool doExplosionForce = true;

	public float explodeRadius = 5f;
	public float explodeForce = 50f;



	private ParticleSystem particles;

	// Use this for initialization
	IEnumerator Start () {

		particles = GetComponent<ParticleSystem>();
		Invoke("DestroyDelayed",10f);	

		if(doExplosionForce)
		{
			yield return null;
			Explode();
		}
	}


	void Update()
	{
		if(particles.isStopped && (particles.particleCount == 0))
		{
			DestroyDelayed();
		}

	}
	
	private void DestroyDelayed()
	{
		Destroy(this.gameObject);
	}


	private void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(this.transform.position,explodeRadius);

		for(int i = 0 ; i< colliders.Length ; i++)
		{
			Rigidbody rb = colliders[i].GetComponent<Rigidbody>();

			if (rb != null)
			{
				//Debug.Log("Applying explosion on " + rb.name);
				rb.AddExplosionForce(explodeForce, this.transform.position, explodeRadius,0f,ForceMode.VelocityChange);
			}
		}

		//TODO Particules
	}
}
