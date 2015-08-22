using System;
using System.Collections;
using UnityEngine;

public class UnitController : Singleton<UnitController>
{
    public float IdleThreshold = 0.1f;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Unit _unit;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _unit = GetComponentInChildren<Unit>();
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateAttacks();
    }

    void UpdateMovement()
    {
        var running = _rigidbody.velocity.magnitude > IdleThreshold;
        _animator.SetBool("IsRunning", running);
    }

    void UpdateAttacks()
    {
        var weapon = _unit.CurrentWeapon;
        _animator.SetBool("IsAttacking", weapon != null && weapon.Attacking);
    }

}
