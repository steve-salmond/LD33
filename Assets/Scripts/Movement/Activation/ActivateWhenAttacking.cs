using UnityEngine;
using System.Collections;

public class ActivateWhenAttacking : MonoBehaviour {

	public Unit Unit;
	public GameObject Target;

	void Update() 
	{ Target.SetActive(Unit.Attacking);	}
}
