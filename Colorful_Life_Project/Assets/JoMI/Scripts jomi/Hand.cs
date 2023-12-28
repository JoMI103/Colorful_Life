using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hand : MonoBehaviour
{
    Vector3 _targetPos, _bodyPos;
    Quaternion _targetRotation;

    Vector3 _currentVelocity;

    Rigidbody _rb;
    [SerializeField] bool left;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void updateTarget(Vector3 tPos, Quaternion tRot, Vector3 bodyPos) {

        _targetPos = tPos; _targetRotation = tRot; _bodyPos = bodyPos;
    }

    private void Update() {

        Move();
        Rotate();
    }


    private void Move()
    {
        float currentDistance = Vector3.Distance(transform.position, _targetPos);
        if (currentDistance < 0.01f) return;

        Vector3 v1 = _bodyPos - _targetPos;
        Vector3 v2 = _bodyPos - transform.position;

        float f3 = Vector3.Dot(v1, v2);
        Vector3 v3 = Vector3.one;
        if (left)
         v3 = Vector3.Cross(v1, v2).normalized;
        else
         v3 = Vector3.Cross(v2, v1).normalized;

        Vector3 direction = (_targetPos - transform.position).normalized;
        Vector3 nextPos = transform.position + (direction + v3 * f3) * moveSpeed * Time.deltaTime;


       

        if (Vector3.Distance(transform.position, nextPos) > currentDistance) { transform.position = _targetPos; return; }
        transform.position = nextPos;
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 360 * rotateSpeed * Time.deltaTime);
    }

    public float moveSpeed,rotateSpeed;

    

}
