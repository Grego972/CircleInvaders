using UnityEngine;
using System.Collections;

public interface IDamagable {

	void TakeHealthDamage(float healthDamage, GameObject fromObject);


	void TakeHealthBonus(float healthBonus);

	void TakeWeaponBonus(int i);

}
