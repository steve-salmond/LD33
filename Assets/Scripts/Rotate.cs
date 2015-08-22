using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{

    public Vector3 Axis;

    public float Speed;

	void Update() 
    { transform.Rotate(Axis, Speed * Time.deltaTime); }
}
