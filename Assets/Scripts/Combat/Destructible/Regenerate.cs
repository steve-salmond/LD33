using UnityEngine;
using System.Collections;

public class Regenerate : MonoBehaviour
{

    public int Amount = 1;
    public float Period = 1;

    private Destructible _destructible;

	// Use this for initialization
	void Start ()
	{
	    _destructible = GetComponent<Destructible>();
	    StartCoroutine(UpdateHealthRoutine());
	}
	
	// Update is called once per frame
    IEnumerator UpdateHealthRoutine()
    {
        var wait = new WaitForSeconds(Period);
        while (!_destructible.IsDestroyed)
        {
            yield return wait;
            _destructible.Heal(Amount);
        }
	}
}
