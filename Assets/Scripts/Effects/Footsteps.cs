using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour 
{

    /** Footstep effect. */
    public GameObject FootstepEffect;

    void Footstep()
    {
        if (FootstepEffect)
        {
            var effect = Instantiate(FootstepEffect, transform.position, transform.rotation) as GameObject;
            effect.transform.parent = transform.transform;
        }
    }
}
