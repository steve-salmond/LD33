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
	private static float _nextVocalEffect = 0;

	public Facing Facing;
    public Unit Unit;

	public Transform Target { get; private set; }

	public bool Attacking { get { return Target != null; } }

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

		StartCoroutine(UpdateTarget());
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

            // Check if target is inactive.
			if (Target == null)
				{ SetTarget(null); continue; }
			if (!Target.gameObject.activeSelf)
				{ SetTarget(null); continue; }
            if (!IsValidTarget(Target))
				{ SetTarget(null); continue; }

            // Check if target is outside detection range.
		    var sqrDistance = UseBoundsForRange
		        ? _targetCollider.bounds.SqrDistance(transform.position)
		        : Vector3.SqrMagnitude(Target.position - transform.position);

            if (sqrDistance > _maxSqrDetectionDistance)
                { SetTarget(null); continue; }

            // Check if target is outside weapon range.
            if (sqrDistance > _maxSqrDistance)
                continue;

            // Check for line of sight.
		    RaycastHit hit;
		    if (!Physics.Raycast(transform.position, Target.position - transform.position, out hit, Range + Detection, LineOfSightMask))
                continue;

            // Can we range the target down line of sight?
            if (hit.collider == null)
                continue;
            if (((1 << hit.collider.gameObject.layer) & Mask.value) == 0)
		        continue;
			if (hit.distance > Range)
				continue;

            // Spawn an attack.
			var attack = ObjectPool.Instance.GetObjectWithComponent(AttackPrefab);
		    attack.Weapon = this;
			attack.Target(Target);
		}
	}

    protected virtual bool IsValidTarget(Transform target)
    {
        return target.gameObject.activeSelf;
    }

    private void SetTarget(Transform value)
	{
	    if (Target == null && value != null)
            SpawnVocalEffect();

		Target = value;

		if (Facing != null)
			Facing.Target = Target;

        if (Target != null && UseBoundsForRange)
            _targetCollider = Target.GetComponent<Collider>();
	}

    private void SpawnVocalEffect()
	{
		if (!VocalEffect)
			return;
		
		if (Time.time < _nextVocalEffect)
			return;
		
		var go = ObjectPool.Instance.GetObject(VocalEffect);
		go.transform.position = transform.position;
		go.transform.rotation = transform.rotation;
		go.transform.parent = transform;
		
		_nextVocalEffect = Time.time + 5;
	}

}
