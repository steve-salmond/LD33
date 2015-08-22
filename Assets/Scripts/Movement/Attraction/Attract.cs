using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public abstract class Attract : MonoBehaviour 
{
	public AnimationCurve Attraction;
	public float Strength = 1;
    public float Range = 1;

    public int Interval = 5;
    private const float MinDistance = 0.5f;

	public bool Attracted 
	{ get { return Attractions != null && Attractions.Count > 0; } }

	protected abstract List<Transform> Attractions { get; }

	protected Rigidbody Rigidbody;

    private int _interval;
    private int _frame;

	protected virtual void OnEnable()
	{
	    _frame = Random.Range(0, Interval);
	    _interval = Interval;

        Rigidbody = GetComponentInParent<Rigidbody>();
        Rigidbody.WakeUp();
	}

    protected virtual void OnDisable()
    {
    }

    protected void OnBecameVisible()
    { _interval = Interval; }

    protected void OnBecameInvisible()
    { _interval = Interval * 5; }


    protected virtual void FixedUpdate()
    {
        // Wait until sufficient frames have passed.
        _frame++;
        if (_frame < _interval)
            return;
        _frame = 0;

        // Perform attraction.
		var attractions = Attractions;
		if (attractions == null)
			return;

		var n = attractions.Count;
		for (var i = 0; i < n; i++)
			AttractTo(i, attractions[i]);
	}

	protected virtual void AttractTo(int i, Transform t)
	{
		if (t == transform || t == null)
			return;

		if (!t.gameObject.activeSelf)
			return;

	    var f = AttractionForce(i, t);
		Rigidbody.AddForce(f);
	}

    protected virtual Vector3 AttractionForce(int i, Transform t)
    {
        var delta = t.position - transform.position;
        delta.y = 0;

        var distance = delta.magnitude;
        if (distance < MinDistance)
            return Vector3.zero;

        var direction = delta * (1.0f / distance);
        var f = AttractionStrength(i, t, distance);
        return direction * f * Interval;
    }

    protected virtual float AttractionStrength(int i, Transform t, float distance)
    { return Attraction.Evaluate(distance / Range) * Strength; }
}
