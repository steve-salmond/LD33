using UnityEngine;
using System.Collections;

public class DeactivateWhenAttracted : MonoBehaviour {

	public Attract Attract;

	public GameObject Target;

	void Update() 
	{ Target.SetActive(!Attract.Attracted);	}
}
