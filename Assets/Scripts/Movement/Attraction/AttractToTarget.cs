using UnityEngine;
using System.Collections.Generic;

public class AttractToTarget : Attract 
{
	public Unit Unit;

	private List<Transform> _transforms = new List<Transform>();

	private void Awake()
	{ _transforms.Add(null); }

	void Update()
	{
        if (Unit.CurrentWeapon != null)
            _transforms[0] = Unit.CurrentWeapon.Target;
	}

	protected override List<Transform> Attractions
	{ get { return _transforms; } }

}
