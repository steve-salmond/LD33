using System;
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

    /** Jump effect. */
    public GameObject JumpEffect;

    /** Footstep effect. */
    public GameObject FootstepEffect;

    public Damageable Damageable
    { get; private set; }

    public float Health
    { get { return Damageable.Health; } }

    private Rigidbody _rigidbody;
    private Animator _animator;

    public bool Grounded { get; private set; }

    private bool _jump;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        Damageable = GetComponent<Damageable>();
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
        UpdateGrounded();
        UpdateMovement();
        UpdateJump();
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
        var dx = Input.GetAxisRaw("Horizontal") * scale.x;
        var dz = Input.GetAxisRaw("Vertical") * scale.y;
        var idle = Mathf.Approximately(dx, 0) && Mathf.Approximately(dz, 0) && Grounded;

        // Ajust player's drag depending on whether they wish to move or not.
        _rigidbody.drag = idle ? IdleDrag : MovingDrag;

        // Update player's movement force from inputs.
        var velocity = _rigidbody.velocity;
        var speed = velocity.magnitude;

        var f = Vector3.zero;
        if (Mathf.Sign(dx) != Mathf.Sign(velocity.x))
            f.x += dx;
        else
            f.x += dx * (1 - Mathf.Clamp01(Mathf.Abs(velocity.x) / MaxSpeed));

        if (Mathf.Sign(dz) != Mathf.Sign(velocity.z))
            f.z += dz;
        else
            f.z += dz * (1 - Mathf.Clamp01(Mathf.Abs(velocity.z) / MaxSpeed));

        f.x += -velocity.x * OpposingForceScale.x;
        f.z += -velocity.z * OpposingForceScale.y;

        _rigidbody.AddForce(f);
        Debug.DrawRay(transform.position, f, Color.green);

        // Update animator state.
        // _animator.SetFloat("RunSpeed", Input.GetAxis("Horizontal"));
    }

    void UpdateJump()
    {
        // Check if player is dead.
        if (Health <= 0)
            return;

        // Check if player wishes to jump and is grounded.
        if (!_jump || !Grounded)
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
