using UnityEngine;
using System.Collections;

/** Cleans up completed effects by returning them to the object pool. */
public class PooledEffectCleanup : MonoBehaviour 
{
	
	// Properties
	// ----------------------------------------------------------------------------
	
	/** Effect's minimum duration. */
	public float Duration = 0;
	
	/** Whether to unparent the effect on cleanup. */
	public bool Unparent = true;
	
	
	// Members
	// ----------------------------------------------------------------------------

	/** Cached particle system component. */
	private ParticleSystem _particles;

    /** Cached audio component. */
    private AudioSource _audio;
	
	/** Timestamp for effect to become eligible for cleanup. */
	private float _cleanupTime;

	
	// Unity Implementation
	// ----------------------------------------------------------------------------
	
	/** Initializes the cleanup script. */
	public void Awake() 
	{
        _audio = GetComponent<AudioSource>();
        _particles = GetComponent<ParticleSystem>();
	}
	
	/** Handle the game object becoming active. */
	public void OnEnable()
	{
        // Schedule cleanup.
		_cleanupTime = Time.time + Duration;

        // Register for cleanup (unless preinstantiating).
        if (!ObjectPool.Instance.Preinstantiating)
            PooledEffectManager.Instance.Register(this);
	}
	
	/** Updates the cleanup script. */
	public bool Returnable
	{
	    get
	    {
	        if (Time.time < _cleanupTime)
                return false;
            if (_audio && _audio.isPlaying)
	            return false;
	        if (_particles && _particles.IsAlive())
	            return false;

	        return true;
	    }
	}

    /** Return this object to the pool. */
    public void Return()
    { ObjectPool.Instance.ReturnObject(gameObject, Unparent); }
	
}
