using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour 
{

    public GameObject MusicPrefab;

    private static GameObject _music;

	void Start () 
    {
        // Start music.
        if (_music == null)
            _music = Instantiate(MusicPrefab) as GameObject;
	}

}
