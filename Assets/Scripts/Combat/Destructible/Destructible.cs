using UnityEngine;

public class Destructible : MonoBehaviour 
{
	
	// Properties
	// ----------------------------------------------------------------------------

	/** Amount of damage the object can sustain before being destroyed. */
	public float Health = 100;

	/** Damage effect. */
	public GameObject DamageEffect;

    /** Probability of seeing a damage effect. */
    public float DamageEffectProbability = 1;

    /** Various damage states for the model. */
    public DestructibleDamageState[] DamageStates;

	/** Destruction effect. */
	public GameObject DestructionEffect;
    
    /** Destructible's associated unit. */
    public Unit Unit { get; private set; }

    /** Whether object is at full health. */
    public bool IsFullHealth
        { get { return _health >= Health; } }

    /** Whether object is damaged. */
    public bool IsDamaged
        { get { return _health < Health; } }

    /** Whether object is destroyed. */
    public bool IsDestroyed
        { get { return _health <= 0; } }


    // Members
    // ----------------------------------------------------------------------------

    /** Current health. */
	private float _health;

	
	// Unity Implementation
	// ----------------------------------------------------------------------------

    void Awake()
        { Unit = GetComponentInParent<Unit>(); }

	/** Starts this component. */
	public void OnEnable()
	{
		// Reset health to initial value.
		_health = Health;

        // Update damage state.
        foreach (var s in DamageStates)
            s.UpdateState(_health);
	}

	/** Apply damage to this entity. */
	public bool Damage(float damage, Vector3 point, Attack attack)
	{
		// Check if any damage was done.
        if (_health <= 0 || Mathf.Approximately(damage, 0))
			return false;
	    if (_health >= Health && damage < 0)
	        return false;
		
		// Apply damage to the object's health.
		_health -= damage;
		
		// Clamp health to allowed range.
		_health = Mathf.Clamp(_health, 0, Health);

        // Update damage state.
        foreach (var s in DamageStates)
            s.UpdateState(_health);

		// Kick off damage effect, if any.
        if (_health > 0 && damage > 0 && DamageEffect != null && Random.value <= DamageEffectProbability)
		{
			var go = ObjectPool.Instance.GetObject(DamageEffect);
			go.transform.position = point != Vector3.zero ? point : transform.position;
		}
		
		// Check if the object should die.
		if (_health <= 0)
            Die(attack);

        // Damage occurred.
	    return true;
	}
	
	// Private Methods
	// ----------------------------------------------------------------------------
	
	/** Initiates object destruction. */
	private void Die(Attack attack)
	{
	    if (Unit != null)
	        Unit.Die(attack);

		// Kick off death effect, if any.
		if (DestructionEffect)
		{
			var go = ObjectPool.Instance.GetObject(DestructionEffect);
			go.transform.position = transform.position;
		}
		
		// Destroy(gameObject);

        ObjectPool.Instance.ReturnObject(gameObject);
	}

	
}
