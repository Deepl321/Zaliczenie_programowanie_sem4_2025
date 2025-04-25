using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerCon : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    private Vector3 _input;

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }

    private void Move()
    {
        Vector3 moveDirection = _input.ToIso() * _speed;
        _rb.linearVelocity = new Vector3(moveDirection.x, _rb.linearVelocity.y, moveDirection.z);
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        _rb.rotation = Quaternion.RotateTowards(_rb.rotation, rot, _turnSpeed * Time.fixedDeltaTime);
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
