using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** The object pool maintains lists of available
  * objects separated by type, and provides methods
  * for getting and recycling them. */ 
public class LocalObjectPool
{
	// Members
	// ----------------------------------------------------------------------------
	
	/** Lists of objects mapped to object types. */
	private Dictionary<string, Stack<GameObject>> objectLists;
	
	/** A parent transform used as a container for instantiated objects. */
	private Transform transform;

	/** When unparenting put under this transform for a tidy hierarchy (may effect performance). */
	private bool tidyUnparent;


	// Life Cycle
	// ----------------------------------------------------------------------------
	
	/** Constructor. */
	public LocalObjectPool(Transform transform, bool tidyUnparent = true)
	{
		// Create the object list and instance count maps.
		objectLists = new Dictionary<string, Stack<GameObject>>();
		
		this.transform = transform;
		this.tidyUnparent = tidyUnparent;
	}
	
	
	// Public Interface
	// ----------------------------------------------------------------------------
	
	/** Preinstantiate a number of objects into the pool. */
	public void Preinstantiate(GameObject template, int n = 1, bool active = true, bool staticBatch = false)
	{
		// Check that the template exists.
		if (template == null)
			return;
		
		// Get the list of objects for the object type.
		Stack<GameObject> objects = GetObjectList(template.name);
		
		// Create n objects and add them to the pool.
		for (int i = 0; i < n; i++)
		{
			// Create a new object instance.
			GameObject o = CreateObject(template, active, staticBatch);
			
			// Parent the preinstantiated objects to the
			// pool if in editor mode to keep the scene tidy.
			#if UNITY_EDITOR
			o.transform.parent = transform;
			#endif
			
			// Add object in to the list.
			objects.Push(o);
		}
	}
	
	/** Get an instantiated object of the specified template. */
	public GameObject GetObject(GameObject template, bool active = true, bool staticBatch = false, string name = null)
	{
		// Use the type name if not specified.
		if (name == null)
			name = template.name;

		// Get the list of available objects.
		Stack<GameObject> objects = GetObjectList(name);
		
		// Instantiate a new object if there are none in the list.
		if (objects.Count <= 0)
		{
			// Create a new object instance.
			GameObject o = CreateObject(template, active, staticBatch, name);
			
			return o;
		}
		
		// Otherwise, remove and return the last object.
		else
		{
			GameObject o = objects.Pop();
			
			if (active)
				o.SetActive(true);
			
			return o;
		}
	}

	/** Get an object with the associated component on it. */
	public T GetObjectWithComponent<T>(T component, bool active = true, bool staticBatch = false, string name = null) where T : Component
	{
		return GetObject(component.gameObject, active, staticBatch, name).GetComponent<T>();
	}

	/** Return a object to the pool. */
	public void ReturnObject(GameObject o, bool unparent = false)
	{
		// Check that the object exists.
		if (o == null)
			return;
		
		// Deactivate the object.
		o.SetActive(false);
		
		// Unparent.
		if (unparent)
		{
			if (tidyUnparent)
				o.transform.parent = transform;	
			else
				o.transform.parent = null;
		}
		
		// Get the list of objects for the object type.
		Stack<GameObject> objects = GetObjectList(o.name);
		
		// Add the object back in to the list.
		objects.Push(o);
	}
	
	// Private Methods
	// ----------------------------------------------------------------------------
	
	/** Create a game object. */
	private GameObject CreateObject(GameObject template, bool active = true, bool staticBatch = false, string name = null)
	{
		// Create a new object instance.
		GameObject o = (GameObject)GameObject.Instantiate(template);
		o.name = name != null ? name : template.name;
		
		// Combine for static batching if required.
		if (staticBatch)
			StaticBatchingUtility.Combine(o);
		
		// Deactivate if required.
		if (!active)
			o.SetActive(false);
		
		return o;
	}
	
	/** Get a list of avaialable objects for the specified object name. */
	private Stack<GameObject> GetObjectList(string name)
	{
		// The list of objects.
		Stack<GameObject> objects;
		
		// Create a new object list if it doesn't exist yet.
		if (!objectLists.ContainsKey(name))
		{
			objects = new Stack<GameObject>();
			objectLists.Add(name, objects);
		}
		
		// Otherwise, find the mapped object list.
		else
			objects = objectLists[name];
		
		return objects;
	}
}
