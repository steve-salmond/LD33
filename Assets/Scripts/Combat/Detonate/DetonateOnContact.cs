using UnityEngine;
using System.Collections;

public class DetonateOnContact : DetonateBehavior 
{
	
	// Properties
	// ----------------------------------------------------------------------------

	/** Detonation offset from surface. */
	public float Offset = 1;
	
	/** Layer mask for valid contactors. */
	public LayerMask ContactMask;

	/** Detonates object when it collides with something. */
	public void OnCollisionEnter(Collision collision)
	{
		// Check if we should detonate on this object.
		int layer = collision.gameObject.layer;
		if ((ContactMask.value & 1 << layer) == 0)
			return;
			
		Vector3 normal = collision.contacts[0].normal;
		Detonate(transform.position + normal * Offset, transform.rotation, normal);
	}
	
}
