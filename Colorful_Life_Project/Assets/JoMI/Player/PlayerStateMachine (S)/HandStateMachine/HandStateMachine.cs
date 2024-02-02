using System.Collections.Generic;
using UnityEngine;

public class HandStateMachine : MonoBehaviour
{
    [SerializeField] private HandBaseStatsSO _handBaseStats;

    [SerializeField] private Transform _playerBody;

    [SerializeField] private Transform _animationTransform;
    [SerializeField] private Transform _baseTransform;
    [SerializeField] private Transform _followTransform;


    float _currentTargetDistance;
    Vector3 _playerBodyInfluence;
    Vector3 _currentTargetDirection;

    private Vector3 _currentVelocity;
    private float _currentPunchForce;
    private Vector3 _lastBodyPos;

    #region State Machine Variables Setters and Getters
    //Hands state machine
    private Dictionary<HandState, InnerBaseState<HandState>> _handStates = new();
    private InnerBaseState<HandState> _currentHandState;

    /// Getters and Setters
    public Dictionary<HandState, InnerBaseState<HandState>> HandStates { get => _handStates; }
    public ref InnerBaseState<HandState> CurrentHandStateRef { get => ref _currentHandState; }
    public InnerBaseState<HandState> CurrentHandState { get => _currentHandState; }

    #endregion


    #region Setters and Getters
   
    public Vector3 CurrentVelocity { get => _currentVelocity; set => _currentVelocity = value; }
    public HandBaseStatsSO HandBaseStats { get => _handBaseStats;  }
    public Transform PlayerBody { get => _playerBody; }

    public Transform AnimationTransform { get => _animationTransform;  }
    public float CurrentPunchForce { get => _currentPunchForce; set => _currentPunchForce = value; }
    public float CurrentTargetDistance { get => _currentTargetDistance; }
    public Vector3 PlayerBodyInfluence { get => _playerBodyInfluence; }
    public Vector3 CurrentTargetDirection { get => _currentTargetDirection; }

    public Transform BaseTransform { get => _baseTransform; }
    public Transform FollowTransform { get => _followTransform; }


    #endregion

    private void Awake()
    {
        void SetHandStateMachineStates()
        {
            _handStates.Add(HandState.Animate, new HandAnimatedState(HandState.Animate, this));
            _handStates.Add(HandState.Burst, new HandBurstState(HandState.Burst, this));
            _handStates.Add(HandState.Follow, new HandFollowState(HandState.Follow, this));
            _handStates.Add(HandState.Free, new HandFreeState(HandState.Free, this));
        }
        SetHandStateMachineStates();
        _currentHandState = _handStates[HandState.Free];
    }
    void Start()
    {
        _currentHandState.EnterState();   
    }

    void Update()
    {

        _currentHandState.UpdateState();
        _lastBodyPos = _playerBody.position;
    }

    

    public void SwitchState(HandState handState)
    {
        if (_currentHandState.StateKey == handState) return;

        _currentHandState.ExitState();
        _currentHandState = HandStates[handState];
        _currentHandState.EnterState();
    }



    
     

    
    

    #region movement methods

    public void FixPositionWithPlayerMovement()
    {
        this.transform.position += Vector3.Lerp(_playerBody.position - _lastBodyPos, Vector3.zero, Mathf.Lerp(0, 1, Vector3.Distance(_baseTransform.position, transform.position) - 0.3f));
        _lastBodyPos = _playerBody.position;
    }

    public void CalculateForBaseTarget()
    {
        _currentTargetDistance = Vector3.Distance(_baseTransform.position, transform.position);

        _playerBodyInfluence = (PlayerBody.position - transform.position).normalized *
                Mathf.Pow(Vector3.Distance(transform.position, PlayerBody.position), _handBaseStats.BodyInflunce);

        _currentTargetDirection = (_baseTransform.position - transform.position).normalized;

    }

    public void CalculateForFollowTarget()
    {
        _currentTargetDistance = Vector3.Distance(_followTransform.position, transform.position);

        _playerBodyInfluence = (PlayerBody.position - transform.position).normalized *
                Mathf.Pow(Vector3.Distance(transform.position, PlayerBody.position), -1);

        _currentTargetDirection = (_followTransform.position - transform.position).normalized;
    }


    public void HandleMovement(float AdditionalVelocity)
    {
        CurrentVelocity = (CurrentTargetDirection - PlayerBodyInfluence) *
           HandBaseStats.MoveSpeed * AdditionalVelocity * GetVelocityFactorFunc(CurrentTargetDistance);

        Vector3 nextPos = transform.position + CurrentVelocity * Time.deltaTime;

        if (Vector3.Distance(transform.position, nextPos) > CurrentTargetDistance)
        {
            CurrentVelocity = Vector3.zero;
            transform.position = BaseTransform.position;
            return;
        }

        transform.position = nextPos;
    }
  

    public float GetVelocityFactorFunc(float x)
    {
        float aux1 = 1 + x * HandBaseStats.Func1;
        if (aux1 == 0) return 0;
        float y = 1 - 1 / aux1;
        return y;
    }


    #endregion


    public enum HandState
    {
        Follow,
        Burst,
        Animate,
        Free,
    }
}
