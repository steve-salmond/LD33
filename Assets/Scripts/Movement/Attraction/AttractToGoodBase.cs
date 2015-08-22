using UnityEngine;
using System.Collections.Generic;

public class AttractToGoodBase: Attract 
{
	private List<Transform> _transforms = new List<Transform>();
	
	private void Awake()
	{ _transforms.Add(LocationManager.Instance.GoodBase); }
	
	protected override List<Transform> Attractions
	{ get { return _transforms; } }
	
}
