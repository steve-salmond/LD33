using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractToObjects : Attract 
{
	public LayerMask Mask;
	public float Radius = 20;
	public float UpdatePeriod = 0.5f;

	private readonly List<Transform> _transforms = new List<Transform>();

    private WaitForSeconds _updateWait;

    private void Awake()
    {
        _updateWait = new WaitForSeconds(UpdatePeriod);
    }

	protected override void OnEnable()
	{
		base.OnEnable();
        StopCoroutine(UpdateNearbyObjects());
		StartCoroutine(UpdateNearbyObjects());
	}

	protected IEnumerator UpdateNearbyObjects()
	{
		while (true)
		{
			_transforms.Clear();

		    var hits = Physics.OverlapSphere(transform.position, Radius, Mask);
		    var n = hits.Length;

            for (var i = 0; i < n; i++)
		    {
                var t = hits[i].transform;
                if (t.gameObject.activeSelf)
                    _transforms.Add(t);
		    }

            yield return _updateWait;
		}
	}

	protected override List<Transform> Attractions
	{ get { return _transforms; } }
}
