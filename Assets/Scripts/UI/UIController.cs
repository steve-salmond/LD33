using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : Singleton<UIController> 
{

    public Graphic Fade;

    public float FadeInTime = 3;

	void Start ()
	{
	    Fade.color = Color.black;
	    Fade.DOFade(0, FadeInTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
