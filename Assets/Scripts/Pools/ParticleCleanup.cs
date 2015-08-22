using UnityEngine;
using System.Collections;

public class ParticleCleanup : MonoBehaviour 
{
    void OnEnable()
    { GetComponent<ParticleSystem>().Clear(); }

	void OnDisable()
    { GetComponent<ParticleSystem>().Clear(); }
}
