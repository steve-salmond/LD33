using UnityEngine;

public class Destructible : MonoBehaviour 
{
	
	// Properties
	// ----------------------------------------------------------------------------

	/** Amount of damage the object can sustain before being destroyed. */
	public float InitialHealth = 100;

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

    /** Destructible's current health. */
    public float Health { get; private set; }

    /** Whether object is at full health. */
    public bool IsFullHealth
        { get { return Health >= InitialHealth; } }

    /** Whether object is damaged. */
    public bool IsDamaged
        { get { return Health < InitialHealth; } }

    /** Whether object is destroyed. */
    public bool IsDestroyed
        { get { return Health <= 0; } }


    // Members
    // ----------------------------------------------------------------------------

    /** Current health. */


    // Unity Implementation
	// ----------------------------------------------------------------------------

    void Awake()
        { Unit = GetComponentInParent<Unit>(); }

	/** Starts this component. */
	public void OnEnable()
	{
		// Reset health to initial value.
		Health = InitialHealth;

        // Update damage state.
        foreach (var s in DamageStates)
            s.UpdateState(Health);
	}

	/** Apply damage to this entity. */
	public bool Damage(float damage, Vector3 point, Attack attack)
	{
		// Check if any damage was done.
        if (Health <= 0 || Mathf.Approximately(damage, 0))
			return false;
	    if (Health >= InitialHealth && damage < 0)
	        return false;
		
		// Apply damage to the object's health.
		Health -= damage;
		
		// Clamp health to allowed range.
		Health = Mathf.Clamp(Health, 0, InitialHealth);

        // Update damage state.
        foreach (var s in DamageStates)
            s.UpdateState(Health);

		// Kick off damage effect, if any.
        if (Health > 0 && damage > 0 && DamageEffect != null && Random.value <= DamageEffectProbability)
		{
			var go = ObjectPool.Instance.GetObject(DamageEffect);
			go.transform.position = point != Vector3.zero ? point : transform.position;
		}
		
		// Check if the object should die.
		if (Health <= 0)
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
