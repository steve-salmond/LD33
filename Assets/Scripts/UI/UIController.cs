using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{

    public Graphic Fade;
    public Text GoodScore;
    public Text EvilScore;
    public Text Health;

    public Graphic GoodVictory;
    public Graphic EvilVictory;
    public Graphic GoodDefeat;
    public Graphic EvilDefeat;

    public float FadeInTime = 5;
    public float FadeOutTime = 5;

    private bool _gameOver;

	void Start ()
	{
	    Fade.color = Color.black;
	    Fade.DOFade(0, FadeInTime);
	}
	
	void Update ()
	{
	    GoodScore.text = string.Format("{0}", GameManager.Instance.GoodScore);
        EvilScore.text = string.Format("{0}", GameManager.Instance.EvilScore);
        Health.text = string.Format("{0}", PlayerController.Instance.Health);

	    if (GameManager.Instance.State != GameManager.GameState.Playing)
	        GameOver();
	}

    void GameOver()
    {
        if (_gameOver)
            return;

        _gameOver = true;

        switch (GameManager.Instance.State)
        {
            case GameManager.GameState.GoodVictory:
                StartCoroutine(GoodVictoryRoutine());
                break;
            case GameManager.GameState.EvilVictory:
                StartCoroutine(EvilVictoryRoutine());
                break;
            case GameManager.GameState.GoodDefeat:
                StartCoroutine(GoodDefeatRoutine());
                break;
            case GameManager.GameState.EvilDefeat:
                StartCoroutine(EvilDefeatRoutine());
                break;
        }
        
    }

    IEnumerator GoodVictoryRoutine()
    {
        Fade.DOFade(1, FadeOutTime);
        yield return StartCoroutine(WaitForFade(GoodVictory, FadeOutTime, 10, 0.5f));
        Application.LoadLevel("Intro");
    }

    IEnumerator EvilVictoryRoutine()
    {
        Fade.DOFade(1, FadeOutTime);
        yield return StartCoroutine(WaitForFade(EvilVictory, FadeOutTime, 10, 0.5f));
        Application.LoadLevel("Intro");
    }

    IEnumerator GoodDefeatRoutine()
    {
        Fade.DOFade(1, FadeOutTime);
        yield return StartCoroutine(WaitForFade(GoodDefeat, FadeOutTime, 10, 0.5f));
        Application.LoadLevel("Intro");
    }

    IEnumerator EvilDefeatRoutine()
    {
        Fade.DOFade(1, FadeOutTime);
        yield return StartCoroutine(WaitForFade(EvilDefeat, FadeOutTime, 10, 0.5f));
        Application.LoadLevel("Intro");
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
