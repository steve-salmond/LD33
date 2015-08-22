using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{

    public float MaxSpeed = 5;
    public Vector2 InputForceScale = Vector2.one;
    public Vector2 AirborneForceScale = Vector2.one;
    public float JumpForce = 1000;
    public Vector2 OpposingForceScale = Vector2.one;

    public LayerMask GroundMask;
    public float GroundDistance = 1.1f;

    public float IdleDrag = 1;
    public float MovingDrag = 0;

    public float AimOffsetScale = 5;

    public float AlignmentChangeDuration = 1;

    public GameObject Sword;

    public Weapon GoodWeapon;
    public Weapon EvilWeapon;

    /** Jump effect. */
    public GameObject JumpEffect;

    /** Footstep effect. */
    public GameObject FootstepEffect;

    public Destructible Destructible
    { get; private set; }

    public Alignment Alignment
    { get; private set; }

    public float Health
    { get { return Destructible.Health; } }

    public bool IsDead
    { get { return Destructible.IsDestroyed; } }

    public bool Transforming 
    { get; private set; }

    public Transform Mouse;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Unit _unit;

    public bool Grounded { get; private set; }

    private bool _jump;

    private Plane _attackPlane = new Plane(Vector3.up, 1);


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _unit = GetComponentInChildren<Unit>();
        _unit.EquipWeapon(GoodWeapon);

        Destructible = GetComponent<Destructible>();
    }

    void Update()
    {
        // Detect when player wishes to jump.
        // Do this in Update, since it can be missed in FixedUpdate().
        _jump = Input.GetButtonDown("Jump");
        UpdateJump();
    }

    void FixedUpdate()
    {
        UpdateAlignment();
        UpdateGrounded();
        UpdateMovement();
        UpdateAttacks();
        UpdateJump();
    }

    void UpdateAlignment()
    {
        var old = Alignment;
        Alignment = TimeController.Daytime ? Alignment.Good : Alignment.Evil;

        // Perform alignment transition.
        if (old != Alignment)
            StartCoroutine(TransformationRoutine());
    }

    private IEnumerator TransformationRoutine()
    {
        Transforming = true;

        // Update player's layer.
        var game = GameManager.Instance;
        var layerName = game.LayerForAlignment(Alignment);
        var good = Alignment == Alignment.Good;
        gameObject.layer = LayerMask.NameToLayer(layerName);

        // Update sword visibility.
        Sword.SetActive(good);

        // Equip the appropriate weapon.
        var weapon = good ? GoodWeapon : EvilWeapon;
        _unit.EquipWeapon(weapon);

        // Fire off the transformation trigger.
        _animator.SetTrigger("Transformation");

        // Cross-fade animation layers from good to evil (or vice versa).
        var fromLayer = good ? 1 : 2;
        var toLayer = good ? 2 : 1;
        var duration = AlignmentChangeDuration;
        var start = Time.time;
        var end = start + duration;
        while (Time.time < end)
        {
            var f = Mathf.Clamp01((Time.time - start) / duration);
            _animator.SetLayerWeight(fromLayer, f);
            _animator.SetLayerWeight(toLayer, 1 - f);
            yield return 0;
        }

        Transforming = false;
    }

    void UpdateGrounded()
    {
        // Check if player is grounded.
        Grounded = Physics.Raycast(transform.position, Vector3.down, GroundDistance, GroundMask);
    }

    void UpdateMovement()
    {
        // Check if player is dead.
        if (Health <= 0)
            return;

        // Get input force scaling factor.
        var scale = Grounded ? InputForceScale : AirborneForceScale;

        // Get player's inputs.
        var transforming = (Transforming ? 0 : 1);
        var dx = Input.GetAxisRaw("Horizontal") * scale.x * transforming;
        var dz = Input.GetAxisRaw("Vertical") * scale.y * transforming;
        var idle = Mathf.Approximately(dx, 0) && Mathf.Approximately(dz, 0) && Grounded;

        // Ajust player's drag depending on whether they wish to move or not.
        _rigidbody.drag = idle ? IdleDrag : MovingDrag;

        // Update player's movement force from inputs.
        var velocity = _rigidbody.velocity;
        var f = Vector3.zero;
        if (Mathf.Sign(dx) != Mathf.Sign(velocity.x))
            f.x += dx;
        else
            f.x += dx * (1 - Mathf.Clamp01(Mathf.Abs(velocity.x) / MaxSpeed));
        if (Mathf.Sign(dz) != Mathf.Sign(velocity.z))
            f.z += dz;
        else
            f.z += dz * (1 - Mathf.Clamp01(Mathf.Abs(velocity.z) / MaxSpeed));

        // Oppose movement if inputs are idle.
        f.x += -velocity.x * OpposingForceScale.x;
        f.z += -velocity.z * OpposingForceScale.y;

        _rigidbody.AddForce(f);
        Debug.DrawRay(transform.position, f, Color.green);

        // Update animator state.
        _animator.SetBool("IsRunning", !idle);

    }

    void UpdateAttacks()
    {
        // Place mouse transform under the mouse.
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float d;
        _attackPlane.Raycast(ray, out d);
        Mouse.position = ray.GetPoint(d);

        // Check if player has a weapon.
        if (!_unit.CurrentWeapon)
            return;
        if (_unit.CurrentWeapon.Attacking)
            return;

        // Check if player wishes to attack.
        var attacking = Input.GetButton("Fire1") && !Transforming;
        if (attacking)
            StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        var weapon = _unit.CurrentWeapon;
        _animator.SetBool("IsAttacking", true);

        while (Input.GetButton("Fire1") && !Transforming)
        {
            weapon.SetTarget(Mouse);
            weapon.Attack();
            yield return new WaitForSeconds(weapon.AttackPeriod.x);
        }

        _animator.SetBool("IsAttacking", false);
        weapon.SetTarget(null);
    }

    void UpdateJump()
    {
        // Check if player is dead.
        if (Health <= 0)
            return;

        // Check if player wishes to jump and is grounded.
        if (!_jump || !Grounded)
            return;

        // Check if transforming.
        if (Transforming)
            return;

        // Apply jump force.
        _rigidbody.AddForce(Vector2.up * JumpForce);

        // Inform animator that jump has occurred.
        if (_animator)
            _animator.SetTrigger("Jump");

        // Play jump effect.
        if (JumpEffect)
        {
            var effect = Instantiate(JumpEffect, transform.position, transform.rotation) as GameObject;
            effect.transform.parent = transform.transform;
        }

        // Don't jump until next frame.
        _jump = false;
    }

    void Footstep()
    {
        if (!Grounded)
            return;

        if (FootstepEffect)
        {
            var effect = Instantiate(FootstepEffect, transform.position, transform.rotation) as GameObject;
            effect.transform.parent = transform.transform;
        }
    }
}
