using UnityEngine;
using System.Collections;

public class DeactivateWhenHasTarget : MonoBehaviour {

    public Unit Unit;
	public GameObject Target;

	void Update()
    { Target.SetActive(!Unit.HasTarget); }
}
