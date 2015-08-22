using UnityEngine;
using System.Collections;

public class UnitManager : Singleton<UnitManager> 
{
	public int Friendlies { get; private set; }
	public int Enemies { get; private set; }

    public int FriendliesKilled { get; private set; }
    public int EnemiesKilled { get; private set; }

	public void Add(Unit unit)
	{
		if (unit.Alignment == Alignment.Good)
			Friendlies++;
		else
			Enemies++;
	}

	public void Remove(Unit unit)
	{
        if (unit.Alignment == Alignment.Good)
            { Friendlies--; FriendliesKilled++; }
        else
            { Enemies--; EnemiesKilled++; }
	}

}
