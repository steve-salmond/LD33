using UnityEngine;
using System.Collections;

public class DetonateOnTime : DetonateBehavior 
{
	
	// Properties
	// ----------------------------------------------------------------------------

	/** Lifetime of the object. */
	public float Lifetime = 1;
	

	// Use this for initialization
    protected override void OnEnable()
    {
        base.OnEnable();
        Invoke("SelfDestruct", Lifetime);
    }
	
	void OnDisable()
		{ CancelInvoke(); }
	
	private void SelfDestruct()
		{ Detonate(transform.position, transform.rotation, transform.forward); }
	
	
}
