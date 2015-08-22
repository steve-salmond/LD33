using UnityEngine;
using System.Collections;

public class DestructibleDamageState : MonoBehaviour
{
	
	// Properties
	// ----------------------------------------------------------------------------
	
	/** Minimum health threshold for this state. */
	public float MinHealth;

	/** Maximum health threshold for this state. */
	public float MaxHealth;
	
	
	// Methods
	// ----------------------------------------------------------------------------
	
	/** Updates damage state according to health. */
	public void UpdateState(float health)
	{
		bool active = (health > MinHealth) && (health <= MaxHealth);
		gameObject.SetActive(active);
	}
	
}

