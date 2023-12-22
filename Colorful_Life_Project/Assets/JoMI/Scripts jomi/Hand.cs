using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hand : MonoBehaviour
{
    Vector3 _targetPos;
    Quaternion _targetRotation;

    Vector3 _currentVelocity;

    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void updateTarget(Vector3 tPos, Quaternion tRot) {

        _targetPos = tPos; _targetRotation = tRot;
    }

    private void Update() {

        Move();
        Rotate();
    }


    private void Move()
    {
        float currentDistance = Vector3.Distance(transform.position, _targetPos);
        if (currentDistance < 0.01f) return;

        Vector3 direction = (_targetPos - transform.position).normalized;
        Vector3 nextPos = transform.position + direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nextPos) > currentDistance) { transform.position = _targetPos; return; }
        transform.position = nextPos;
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 360 * rotateSpeed * Time.deltaTime);
    }

    public float moveSpeed,rotateSpeed;

    

}
