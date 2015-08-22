using UnityEngine;
using System.Collections.Generic;

public class Group : MonoBehaviour
{
    public string Id;

	[HideInInspector]
	[System.NonSerialized]
	public List<Unit> Units = new List<Unit>();

	public delegate void UnitEventHandler(Unit unit);

	public event UnitEventHandler OnUnitAdded;
	public event UnitEventHandler OnUnitRemoved;


    public void Clear()
    {
        foreach (var unit in Units)
        {
            unit.RemovedFromGroup(this);
            if (OnUnitRemoved != null)
                OnUnitRemoved(unit);
        }

        Units.Clear();
    }

	public void AddUnit(Unit unit)
	{
		if (Units.Contains(unit))
			return;

		Units.Add(unit);
        unit.AddedToGroup(this);

		if (OnUnitAdded != null)
			OnUnitAdded(unit);
	}
	
	public void RemoveUnit(Unit unit)
	{
		if (!Units.Contains(unit))
			return;

		Units.Remove(unit);
        unit.RemovedFromGroup(this);

		if (OnUnitRemoved != null)
			OnUnitRemoved(unit);
	}
}
