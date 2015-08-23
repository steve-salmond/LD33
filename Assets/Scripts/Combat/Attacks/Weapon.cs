using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour, IAttacker
{
    public int Level = 1;

	public LayerMask Mask;
	public float Detection = 0;
    public float Range = 20;
    public bool UseBoundsForRange;
    public int MaxDetectionHits = 20;
    public float UpdatePeriod = 0.5f;

    public LayerMask LineOfSightMask;

	public Attack AttackPrefab;
	public Vector2 AttackPeriod = Vector2.one;
	public Transform Emitter;

	public GameObject VocalEffect;
	// private static float _nextVocalEffect = 0;

	public Facing Facing;
    public Unit Unit;

    public bool AutoTarget = true;
    public bool AutoAttack = true;

	public Transform Target { get; private set; }

	public bool Attacking { get; private set; }

    public bool HasTarget { get { return Target != null; } }

    private float _maxSqrDetectionDistance;
    private float _maxSqrDistance;

    private Collider _targetCollider;

    private WaitForSeconds _updateWait;
    private WaitForSeconds[] _attackWaits;
    private const int AttackRangeCount = 3;

    private void Awake()
    {
        if (Emitter == null)
            Emitter = transform;

        _maxSqrDetectionDistance = (Range + Detection) * (Range + Detection);
        _maxSqrDistance = Range * Range;
        _updateWait = new WaitForSeconds(UpdatePeriod);

        _attackWaits = new WaitForSeconds[AttackRangeCount];
        for (var i = 0; i < AttackRangeCount; i++)
            _attackWaits[i] = new WaitForSeconds(Random.Range(AttackPeriod.x, AttackPeriod.y));
    }

	private void OnEnable()
	{
        if (Unit == null)
            Unit = GetComponentInParent<Unit>();

        if (AutoTarget)
		    StartCoroutine(UpdateTarget());

        if (AutoAttack)
            StartCoroutine(AttackTarget());
	}

    void OnDisable()
    { StopAllCoroutines(); }

	private IEnumerator UpdateTarget()
	{
		while (true)
		{
            // Wait for a while.
            yield return _updateWait;

			// Locate closest target.
			var closest = float.MaxValue;
		    var range = Target != null ? Range : Range + Detection;
		    var hits = Physics.OverlapSphere(transform.position, range, Mask);
		    var n = hits.Length;
			for (var i = 0; i < n; i++)
			{
                if (!IsValidTarget(hits[i].transform))
                    continue; 

			    var distance = Vector3.SqrMagnitude(hits[i].transform.position - transform.position);
			    if (distance >= closest) 
                    continue;

			    SetTarget(hits[i].transform);
			    closest = distance;
			}	

		}
	}

	private IEnumerator AttackTarget()
	{
		while (true)
		{
            // Wait a while.
            yield return _attackWaits[Random.Range(0, AttackRangeCount)];

            // Attempt an attack.
		    Attack();
		}
	}

    public void Attack()
    {
        Attacking = false;

        // Check if target is inactive.
        if (Target == null)
            { SetTarget(null); return; }
        if (!Target.gameObject.activeSelf)
            { SetTarget(null); return; }
        if (!IsValidTarget(Target))
            { SetTarget(null); return; }

        // Check if target is outside detection range.
        var sqrDistance = UseBoundsForRange
            ? _targetCollider.bounds.SqrDistance(transform.position)
            : Vector3.SqrMagnitude(Target.position - transform.position);

        if (sqrDistance > _maxSqrDetectionDistance)
            { SetTarget(null); return; }

        // Check if target is outside weapon range.
        if (sqrDistance > _maxSqrDistance)
            return;

        if (LineOfSightMask != 0)
        {
            // Check for line of sight.
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, Target.position - transform.position, out hit, Range + Detection,
                    LineOfSightMask))
                return;

            // Can we range the target down line of sight?
            if (hit.collider == null)
                return;
            if (((1 << hit.collider.gameObject.layer) & Mask.value) == 0)
                return;
            if (hit.distance > Range)
                return;
        }

        // Spawn an attack.
        var attack = ObjectPool.Instance.GetObjectWithComponent(AttackPrefab);
        attack.Weapon = this;
        attack.Target(Target);
        Attacking = true;
    }

    protected virtual bool IsValidTarget(Transform target)
    {
        return target.gameObject.activeSelf;
    }

    public void SetTarget(Transform value)
	{
	    if (Target == null && value != null)
            SpawnVocalEffect();

		Target = value;

		if (Facing != null)
			Facing.Target = Target;

        if (Target != null && UseBoundsForRange)
            _targetCollider = Target.GetComponent<Collider>();

        Attacking = false;
	}

    private void SpawnVocalEffect()
	{
		if (!VocalEffect)
			return;
		
		// if (Time.time < _nextVocalEffect)
		//	return;
		
		var go = ObjectPool.Instance.GetObject(VocalEffect);
		go.transform.position = transform.position;
		go.transform.rotation = transform.rotation;
		go.transform.parent = transform;
		
		// _nextVocalEffect = Time.time + 5;
	}

}
