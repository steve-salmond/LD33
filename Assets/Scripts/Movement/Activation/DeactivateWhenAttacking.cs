using UnityEngine;
using System.Collections;

public class DeactivateWhenAttacking : MonoBehaviour {

    public Unit Unit;
	public GameObject Target;

	void Update()
    { Target.SetActive(!Unit.Attacking); }
}
