using UnityEngine;
using System.Collections.Generic;

public class AttractToTransform : Attract 
{
	public Transform Target;

	private List<Transform> _transforms = new List<Transform>();

	protected void Awake()
	{ _transforms.Add(Target); }

	protected override List<Transform> Attractions
	{ get { return _transforms; } }

}
