using UnityEngine;
using System.Collections;

public class TransformRandomizer : MonoBehaviour 
{

	public Vector2 RadiusRange = new Vector2(1, 10);
	public Vector2 IntervalRange = new Vector2(1, 10);

	void OnEnable() 
	{
		StopCoroutine("UpdateTransform");
		StartCoroutine(UpdateTransform());
	}
	
	IEnumerator UpdateTransform() 
	{
		while (true)
		{
			var length = Random.Range(RadiusRange.x, RadiusRange.y);
		    var p = Random.insideUnitCircle * length;
			transform.localPosition = new Vector3(p.x, 0, p.y);

			var interval = Random.Range(IntervalRange.x, IntervalRange.y);
			yield return new WaitForSeconds(interval);
		}
	}
}
