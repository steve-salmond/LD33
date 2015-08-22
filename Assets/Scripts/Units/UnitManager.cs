using UnityEngine;
using System.Collections;

public class UnitManager : Singleton<UnitManager> 
{
	public int Good { get; private set; }
	public int Evil { get; private set; }

    public int GoodKilled { get; private set; }
    public int EvilKilled { get; private set; }

    public int GoodScore { get; private set; }
    public int EvilScore { get; private set; }

	public void Add(Unit unit)
	{
		if (unit.Alignment == Alignment.Good)
			Good++;
		else
			Evil++;
	}

	public void Remove(Unit unit)
	{
	    if (unit.Alignment == Alignment.Good)
	    {
	        Good--;
	        GoodKilled++;
	        EvilScore += unit.KillScore;
	    }
	    else
	    {
	        Evil--; 
            EvilKilled++;
            GoodScore += unit.KillScore;
	    }
	}

}
