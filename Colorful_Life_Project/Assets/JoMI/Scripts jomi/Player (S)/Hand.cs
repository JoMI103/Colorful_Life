using System;
using UnityEngine;

public enum handState
{
    Body,
    Attacking,
    Grabbing
}

public class Hand : MonoBehaviour
{
    public handState currentHandState;

    public float moveSpeed, rotateSpeed;

    Vector3  _bodyPos, _lastBodyPos;
    Transform _targetTransform;
 

    Vector3 _direction, _currentVelocity, _bodyInfluence;
    float _currentDistance;

    public PlayerHands _hands;
    [SerializeField] bool left;



    private void Start() {
        currentHandState = handState.Body;
        _lastBodyPos = _hands.transform.position;
    }

    public bool _onTarget;
    public bool _isAttacking;
    public bool _isReturning;

    public void updateTargetTransform(Transform targetTransform) {
        _targetTransform = targetTransform; 
    }

    public void ReturningTime() { _isReturning = false; }

    public void updateHand()
    {
        _bodyPos = _hands.transform.position;
        _currentDistance = Vector3.Distance(transform.position, _targetTransform.position);

        switch (currentHandState) {
            case handState.Body:
                Move();
                Rotate();
                FixPositionWithPlayerMovement();
                break;
            case handState.Attacking:
                if (_isAttacking)
                {
                    
                    BurstMove();
                    FastRotate();
                }
                else
                {
                    Move();
                    Rotate();
                    FixPositionWithPlayerMovement();
                }


                break;    
            case handState.Grabbing:
                if (_onTarget) {
                    Move();
                    Rotate();
                    FixPositionWithPlayerMovement();
                }
                else {
                    _onTarget = isOnTargetPos();
                    Move();
                    FastRotate();
                }
                break;

        }

       
        
    }

    public bool isOnTargetPos()
    {
        if(Vector3.Distance(transform.position,_targetTransform.position) < 0.2f) return true;  return false;  
    }

    

    private void FixPositionWithPlayerMovement() {
        this.transform.position += Vector3.Lerp(_bodyPos - _lastBodyPos,Vector3.zero , Mathf.Lerp(0,1, _currentDistance - 0.3f));
        _lastBodyPos = _bodyPos;
    }

    private void BurstMove()
    {

        _direction = (_targetTransform.position - transform.position).normalized; 

        _currentVelocity = (_direction + someStartOffsetDirection) * moveSpeed * getVelocityFactorFunc2(_currentDistance);//+ v3 * f3;

        someStartOffsetDirection *= 0.8f;
        Vector3 nextPos = transform.position + _currentVelocity * Time.deltaTime;

        if (Vector3.Distance(transform.position, nextPos) > _currentDistance)
        {
            _currentVelocity = Vector3.zero; _bodyInfluence = Vector3.zero;
            transform.position = _targetTransform.position;
            if(checkHits(_targetTransform.position)) _isAttacking = false;
            return;
        }

        if (checkHits(nextPos)) { _targetTransform.position = transform.position; return; }
        


        transform.position = nextPos;
    }

    [SerializeField] private float punchForce;

    private bool checkHits(Vector3 nextpos)
    {
        Vector3 dir = nextpos - transform.position;


        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.2f, dir, Vector3.Distance(transform.position, nextpos));
        foreach (RaycastHit hit in hits)
        {
            MonoBehaviour[] allScripts = hit.collider.gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour mono in allScripts)
            {
                if (mono is IHittable)
                {
                    (mono as IHittable).Hit(this.gameObject, dir * punchForce, hit.point, 10);
                    return true;
                }
            }
        }
        return false;
    }

    private void Move()
    {
        _bodyInfluence = (_bodyPos - transform.position).normalized * Mathf.Pow(Vector3.Distance(transform.position,_bodyPos), -1);
      
        _direction = (_targetTransform.position - transform.position).normalized;
        _currentVelocity = (_direction - _bodyInfluence) * moveSpeed * getVelocityFactorFunc(_currentDistance);//+ v3 * f3;
        Vector3 nextPos = transform.position + _currentVelocity  * Time.deltaTime;


        
        if (Vector3.Distance(transform.position, nextPos) > _currentDistance ) 
        {
            _currentVelocity = Vector3.zero; _bodyInfluence = Vector3.zero;
            transform.position = _targetTransform.position;
            return; 
        }
        
        
        transform.position = nextPos;
    }

    public void StartGrabbing()
    {
        currentHandState = handState.Grabbing;
        initialRotation = transform.rotation;
        _onTarget = false;
    }

    public void AttackMode()
    {
        _isAttacking = false;
        currentHandState = handState.Attacking;
    }

    private Vector3 someStartOffsetDirection;
    public float fff;

    public void StartAttacking(Transform burstTarget)
    {
        _isAttacking = true;
        _targetTransform = burstTarget;
        Vector3 dir = (burstTarget.position - transform.position).normalized;
        Vector3 randomDir = Vector3.Lerp(Vector3.up, Vector3.down, UnityEngine.Random.Range(0f, 1f));
        someStartOffsetDirection = Vector3.Cross(dir, randomDir) * fff;

        initialRotation = transform.rotation;
    }

    public void Return(Transform returnTransform)
    {
        _isReturning = true; _isAttacking = false; Invoke("ReturningTime", 0.2f);
        _targetTransform = returnTransform;
    }



    [SerializeField, Range(1.1f, 2f)] float coiso;

    private void Rotate()
    {
       
        Quaternion farRotation = Quaternion.LookRotation(Vector3.up, _bodyInfluence.normalized);
        Quaternion q = Quaternion.Lerp(_targetTransform.rotation, farRotation, Mathf.Exp(_currentDistance - _currentDistance / coiso) - 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 360 * rotateSpeed * Time.deltaTime);
    }

    Quaternion initialRotation;

    private void FastRotate()
    {
        float dist = _currentDistance / Vector3.Distance(_bodyPos, _targetTransform.position);
        Quaternion q = Quaternion.Lerp( _targetTransform.rotation, initialRotation, dist);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 360 * rotateSpeed * Time.deltaTime);
    }




    [SerializeField, Range(0.01f, 30f)] float dontknow;

    float getVelocityFactorFunc(float x)
    {
        float aux1 = 1 + x * dontknow;
        if (aux1 == 0) return 0;
        float y = 1 - 1 / aux1;
        return y;
    }

    [SerializeField, Range(0.01f, 30f)] float dontknow1;
    [SerializeField, Range(-1, 1)] float dontknow2;

    float getVelocityFactorFunc2(float x)
    {
        float aux1 = dontknow2 + x * dontknow1;
        if (aux1 == 0) return 0;
        float y = 1 + 1 / aux1;
        return y;
    }


    private void OnDrawGizmos()
    {
        Color DColorCurrentVelocity = Color.green, DColorBodyInfluence = Color.blue;

        Debug.DrawLine(transform.position, transform.position + _currentVelocity, DColorCurrentVelocity);
        Debug.DrawLine(transform.position, transform.position - _bodyInfluence, DColorBodyInfluence);
    }
    
}
