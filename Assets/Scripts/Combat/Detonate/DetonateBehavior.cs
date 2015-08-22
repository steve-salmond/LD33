using UnityEngine;
using System.Collections;

public class DetonateBehavior : MonoBehaviour 
{
	
	// Properties
	// ----------------------------------------------------------------------------

	/** Detonation mask. */
	public LayerMask DetonateMask;

	/** Destruction effect. */
	public GameObject DetonateEffect;

    /** Probability of playing destruction effect. */
    public float DetonateEffectProbability = 1;
	
	/** Location to place detonation. */
	public Transform DetonateLocation;
	
	/** Blast radius. */
	public float DetonateRadius = 2;

    /** Explosive force to apply. */
    public float DetonateForce = 0;

	/** Minimum damage amount. */
	public float MinDamage = 10;
	
	/** Maximum damage amount. */
	public float MaxDamage = 10;


    public Attack Attack { get; private set; }

	void Start()
	{
	    Attack = GetComponent<Attack>();

		if (DetonateLocation == null)
			DetonateLocation = transform;
	}
	
	/** Detonate the object. */
	public void Detonate(Vector3 position, Quaternion rotation, Vector3 direction)
	{
		// Apply blast radius effect.
		ApplyBlast(transform.position, DetonateRadius);
		
		// Create the detonation effect.
        if (DetonateEffect != null && DetonateLocation != null && Random.value <= DetonateEffectProbability)
		{
			var go = ObjectPool.Instance.GetObject(DetonateEffect);
			go.transform.position = DetonateLocation.position;
		}

		// Destroy this object.
		ObjectPool.Instance.ReturnObject(gameObject);
	}

	/** Damage everything in the blast radius. */
	private void ApplyBlast(Vector3 location, float radius) 
	{
		var hits = Physics.OverlapSphere(transform.position, DetonateRadius, DetonateMask);
		foreach (var hit in hits)
		{
			if (!hit.transform.gameObject.activeSelf)
				continue;

			var d = hit.transform.GetComponent<Destructible>();
			if (d == null)
				continue;

		    var delta = hit.transform.position - location;
			var t = 1 - delta.magnitude / radius;
			var damage = Mathf.Lerp(MinDamage, MaxDamage, t);

            d.Damage(damage, hit.transform.position, Attack);

		    if (DetonateForce > 0)
		    {
		        var body = hit.GetComponent<Rigidbody>();
                if (body)
		            body.AddForce(delta.normalized * DetonateForce);
		    }

		}
	}

}
