using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class IntroController : MonoBehaviour
{

    

    public Graphic One;
    public Graphic Two;
    public Graphic Three;

	void Start () 
    {
        // Initialization.
        One.color = new Color(1, 1, 1, 0);
        Two.color = new Color(1, 1, 1, 0);
        Three.color = new Color(1, 1, 1, 0);

        // Start intro routine.
	    StartCoroutine(IntroRoutine());
    }
	
	IEnumerator IntroRoutine()
	{
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(WaitForFade(One, 0.5f, 3, 0.5f));
        yield return StartCoroutine(WaitForFade(Two, 0.5f, 3, 0.5f));
        yield return StartCoroutine(WaitForFade(Three, 0.5f, 10, 0.5f));

	    Application.LoadLevel("Game");
    }

    private IEnumerator WaitForFade(Graphic g, float fadeInTime, float waitTime, float fadeOutTime, float waitAfter = 0)
    {
        yield return g.DOFade(1, fadeInTime).WaitForCompletion();
        yield return StartCoroutine(WaitOrSkip(waitTime));
        yield return g.DOFade(0, fadeOutTime).WaitForCompletion();
        yield return StartCoroutine(WaitOrSkip(waitAfter));
    }

    private IEnumerator WaitOrSkip(float t)
    {
        var end = Time.time + t;
        while (Time.time < end && !Input.anyKey)
            yield return 0;
    }
}
