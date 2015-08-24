using UnityEngine;
using System.Collections;

public class ProjectileAttack : Attack 
{
	public GameObject MuzzleFlare;

	public Vector2 SpeedRange;

    public bool MatchEmitterRotation;

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

	public override void Target(Transform target)
	{
		if (MuzzleFlare != null)
			SpawnMuzzleFlare(Weapon);

		transform.position = Weapon.Emitter.position;

        var speed = Random.Range(SpeedRange.x, SpeedRange.y);
        var direction = (target.position - transform.position).normalized;

	    if (MatchEmitterRotation)
	        transform.rotation = Weapon.Emitter.rotation;
        else
		    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        if (_rigidbody)
            _rigidbody.velocity = (direction * speed);
	}

	private void SpawnMuzzleFlare(Weapon weapon)
	{
		var flare = ObjectPool.Instance.GetObject(MuzzleFlare);
		flare.transform.position = weapon.Emitter.position;
		flare.transform.rotation = weapon.Emitter.rotation;
		flare.transform.parent = weapon.Emitter;
	}

}
