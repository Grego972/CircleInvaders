using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class WeaponSettings : ScriptableObject {

	[System.Serializable]
	public class WeaponSettingsItem
	{
		public Vector3 offset;
		public float angle;
		public GameObject bulletPrefabs;
	}


	public string name;
	public float damage = 10f;
	public float firePeriod = .2f;
	public bool allowContinuisPress = true;
	public float jitter = 0.5f;
	public float angleJitter = 0f;
	public int fireCount = 100;
	public AudioClip soundFx;

	public WeaponSettingsItem[] prefabs;

}
