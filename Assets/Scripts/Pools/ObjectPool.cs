using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** The object pool maintains lists of available
  * objects separated by type, and provides methods
  * for getting and recycling them. */ 
public class ObjectPool : Singleton<ObjectPool>
{
	// Properties
	// ----------------------------------------------------------------------------

	/** When unparenting put under this transform for a tidy hierarchy (may effect performance). */
	public bool TidyUnparent;

    /** Whether pool is currently being preinstantiated. */
    public bool Preinstantiating { get; private set; }


	// Members
	// ----------------------------------------------------------------------------

	/** The object pool instance. */
	private LocalObjectPool poolInstance;

	/** Get the object pool. */
	private LocalObjectPool pool
	{
		get
		{
			if (poolInstance == null)
				poolInstance = new LocalObjectPool(transform, TidyUnparent);

			return poolInstance;
		}
	}

	
	// Public Interface
	// ----------------------------------------------------------------------------
	
	/** Preinstantiate a number of objects into the pool. */
	public void Preinstantiate(GameObject template, int n = 1, bool active = true, bool staticBatch = false)
	{
	    Preinstantiating = true;
		pool.Preinstantiate(template, n, active, staticBatch);
	    Preinstantiating = false;
	}
	
	/** Get an object of the specified type. */
	public GameObject GetObject(GameObject template, bool active = true, bool staticBatch = false, string name = null)
	{
		return pool.GetObject(template, active, staticBatch, name);
	}

	/** Get an object with the associated component on it. */
	public T GetObjectWithComponent<T>(T component, bool active = true, bool staticBatch = false, string name = null) where T : Component
	{
		return pool.GetObjectWithComponent<T>(component, active, staticBatch, name);
	}
	
	/** Return an object to the pool. */
	public void ReturnObject(GameObject o, bool unparent = false)
	{
		pool.ReturnObject(o, unparent);
	}
	
	/** Log a report of pooled objects. */
	public void Report()
	{
		pool.Report();
	}

	/** Generate a preinstantiator object. */
	public void GeneratePreinstantiator()
	{
		pool.GeneratePreinstantiator();
	}
}
