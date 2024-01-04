using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{
    Vector3 _targetPos, _bodyPos, _lastBodyPos;

    Quaternion _targetRotation;
    public float moveSpeed, rotateSpeed;
    public bool stop;

    Rigidbody _rb;
    public PlayerHands _hands;
    [SerializeField] bool left;

    private void Start()
    {

        _lastBodyPos = _hands.transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    public void updateTarget(Vector3 tPos, Quaternion tRot ) {

        _targetPos = tPos; _targetRotation = tRot; 
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Z)) stop = !stop;


        _bodyPos = _hands.transform.position;
        Move();
        Rotate();
        FixPositionWithPlayerMovement();
    }

    private void FixPositionWithPlayerMovement()
    {
        if (!stop)
        {
        }
            this.transform.position += Vector3.Lerp(_bodyPos - _lastBodyPos,Vector3.zero , Mathf.Lerp(0,1, currentDistance - 0.3f));
            if (left) Debug.Log(Mathf.Lerp(0, 1, currentDistance - 0.3f));
        _lastBodyPos = _bodyPos;
    }

    Vector3 _direction ,_currentVelocity , _bodyInfluence;
    float currentDistance;


    private void Move()
    {
        currentDistance = Vector3.Distance(transform.position, _targetPos);

        _bodyInfluence = (_bodyPos - transform.position).normalized * Mathf.Pow(Vector3.Distance(transform.position,_bodyPos), -1);
       /* 
        Vector3 v2 = _bodyPos - transform.position;

        float f3 = Vector3.Dot(v1, v2);
        Vector3 v3 = Vector3.one;
        if (left)
         v3 = Vector3.Cross(v1, v2).normalized;
        else
         v3 = Vector3.Cross(v2, v1).normalized;
       */
         _direction = (_targetPos - transform.position).normalized;
        _currentVelocity = (_direction - _bodyInfluence) * moveSpeed * getVelocityFactorFunc(currentDistance);//+ v3 * f3;
        Vector3 nextPos = transform.position + _currentVelocity  * Time.deltaTime;


        if (stop) return;
        
        if (Vector3.Distance(transform.position, nextPos) > currentDistance ) 
        {
            _currentVelocity = Vector3.zero; _bodyInfluence = Vector3.zero;
            transform.position = _targetPos; 
            return; 
        }
        
        transform.position = nextPos;
    }

    [SerializeField, Range(1.1f, 2f)] float coiso;

    private void Rotate()
    {
        Quaternion farRotation = Quaternion.LookRotation( Vector3.up, _bodyInfluence.normalized);
        Quaternion q = Quaternion.Lerp(_targetRotation, farRotation, Mathf.Exp(currentDistance - currentDistance / coiso ) - 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 360 * rotateSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Color DColorCurrentVelocity = Color.green, DColorBodyInfluence = Color.blue;

        Debug.DrawLine(transform.position, transform.position + _currentVelocity , DColorCurrentVelocity);
        Debug.DrawLine(transform.position, transform.position - _bodyInfluence, DColorBodyInfluence);

    }


    [SerializeField, Range(0.01f, 30f)] float dontknow;

    float getVelocityFactorFunc(float x)
    {
        float aux1 = 1 + x * dontknow;
        if (aux1 == 0) return 0;
        float y = 1 - 1 / aux1;
       
        return y;
    }

}
