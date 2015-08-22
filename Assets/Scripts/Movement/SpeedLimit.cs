using UnityEngine;
using System.Collections;

public class SpeedLimit : MonoBehaviour 
{
    
    public float MaxSpeed;

	
	void FixedUpdate() 
    {
        var speed = rigidbody.velocity.magnitude;
        if (speed > MaxSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
	}
}
