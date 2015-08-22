using UnityEngine;
using System.Collections;

public class DeactivateWhenFiring : MonoBehaviour {

    public Weapon Weapon;
	public GameObject Target;

	void Update()
    { Target.SetActive(!Weapon.Attacking); }
}
