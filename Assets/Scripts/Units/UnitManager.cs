using UnityEngine;
using System.Collections;

public class UnitManager : Singleton<UnitManager> 
{
	public int Good { get; private set; }
	public int Evil { get; private set; }
    public int Total { get { return Good + Evil; } }

    public int GoodKilled { get; private set; }
    public int EvilKilled { get; private set; }

    public int GoodScore { get; private set; }
    public int EvilScore { get; private set; }

    public float GoodFraction
    { get { return Mathf.Clamp01((Good / (float) Total) * 2 - 1); } }

    public float EvilFraction
    { get { return Mathf.Clamp01((Evil / (float) Total) * 2 - 1); } }

	public void Add(Unit unit)
	{
        if (unit.KillScore <= 0)
            return;

		if (unit.Alignment == Alignment.Good)
			Good++;
		else
			Evil++;
	}

	public void Remove(Unit unit)
	{
        if (unit.KillScore <= 0)
            return;
        
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
