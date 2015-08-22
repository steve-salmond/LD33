using UnityEngine;
using System.Collections.Generic;

public class AttractToGroup : Attract 
{
	private Unit _unit;
	private readonly List<Transform> _transforms = new List<Transform>();

	protected override void OnEnable()
	{
        base.OnEnable();

		_unit = GetComponent<Unit>();
		if (_unit == null)
			return;

		foreach (var group in _unit.Groups)
			HandleOnAddedToGroup(_unit, group);

		_unit.OnAddedToGroup += HandleOnAddedToGroup;
		_unit.OnRemovedFromGroup += HandleOnRemovedFromGroup;
	}

	void HandleOnAddedToGroup(Unit unit, Group group)
	{
		foreach (var member in group.Units)
			HandleOnUnitAdded(member);

		group.OnUnitAdded += HandleOnUnitAdded;
		group.OnUnitRemoved += HandleOnUnitRemoved;
	}

	void HandleOnRemovedFromGroup(Unit unit, Group group)
	{
		foreach (var member in group.Units)
			HandleOnUnitRemoved(member);

		group.OnUnitAdded -= HandleOnUnitAdded;
		group.OnUnitRemoved -= HandleOnUnitRemoved;
	}

	void HandleOnUnitAdded(Unit unit)
	{ 
		if (_transforms.Contains(unit.transform))
			return;

		if (unit != _unit)
			_transforms.Add(unit.transform); 
	}

	void HandleOnUnitRemoved (Unit unit)
	{ 
		if (!_transforms.Contains(unit.transform))
			return;

		_transforms.Remove(unit.transform); 
	}

	protected override List<Transform> Attractions
	{ get { return _transforms; } }

}
