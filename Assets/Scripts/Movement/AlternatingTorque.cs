using UnityEngine;
using System.Collections;

public class AlternatingTorque : MonoBehaviour {

	public float MinStrength = 1;
	public float MaxStrength = 1;

	public float MinInterval = 1;
	public float MaxInterval = 1;

	private float torque;

    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_rigidbody2D)
            StartCoroutine(AlternateTorque());
    }

	IEnumerator AlternateTorque()
	{
        while (_rigidbody2D != null)
		{
			var f = Random.Range(MinStrength, MaxStrength);
			torque = (torque < 0) ? f : -f;
			var delay = Random.Range(MinInterval, MaxInterval);
			yield return new WaitForSeconds(delay);
		}
	}

    void FixedUpdate()
    {
        if (_rigidbody2D)
            _rigidbody2D.AddTorque(torque);
    }
}
