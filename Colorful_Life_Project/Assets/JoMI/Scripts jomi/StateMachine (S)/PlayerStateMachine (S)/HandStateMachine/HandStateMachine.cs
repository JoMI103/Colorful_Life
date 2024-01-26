using System.Collections.Generic;
using UnityEngine;

public class HandStateMachine : MonoBehaviour
{
    [SerializeField] private HandBaseStatsSO _handBaseStats;

    [SerializeField] private Transform _playerBody;

    [SerializeField] private Transform _animationTransform;

    private Vector3 _targetFollowPos;
    private Vector3 _targetBasePos;
    private Vector3 _targetFollowRotation;
    private Vector3 _targetBaseRotation;


    private Vector3 _currentVelocity;


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
    public Vector3 TargetFollowPos { get => _targetFollowPos; }
    public Vector3 TargetFollowRotation { get => _targetFollowRotation;  }
    public Vector3 TargetBasePos { get => _targetBasePos;  }
    public Vector3 TargetBaseRotation { get => _targetBaseRotation;  }
    public Transform PlayerBody { get => _playerBody; }

    public Transform AnimationTransform { get => _animationTransform;  }

    public void SetFollowTarget(Vector3 TargetPosition, Vector3 TargetRotation) {
        _targetFollowPos = TargetPosition;
        _targetFollowRotation = TargetRotation;
      
    }

    public void SetBaseTarget(Vector3 TargetPosition, Vector3 TargetRotation)
    {
        _targetBasePos = TargetPosition;
        _targetBaseRotation = TargetRotation;
  
    }

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
    }

    

    public void SwitchState(HandState handState)
    {
        if (_currentHandState.StateKey == handState) return;

        _currentHandState.ExitState();
        _currentHandState = HandStates[handState];
        _currentHandState.EnterState();
    }


    #region movement methods

    public void HandleHandToBaseMovement()
    {
        float currentDistance = Vector3.Distance(TargetBasePos, transform.position);

        Vector3 playerBodyInfluenceForce = (PlayerBody.position - transform.position).normalized *
                Mathf.Pow(Vector3.Distance(transform.position, PlayerBody.position), -1);

        Vector3 directionToTarget = (TargetBasePos - transform.position).normalized;

        CurrentVelocity = (directionToTarget - playerBodyInfluenceForce) *
            HandBaseStats.MoveSpeed * getVelocityFactorFunc(currentDistance); //+ v3 * f3;

        //Debug.Log(getVelocityFactorFunc(_ctx.CurrentDistance));

        Vector3 nextPos = transform.position + CurrentVelocity * Time.deltaTime;



        if (Vector3.Distance(transform.position, nextPos) > currentDistance)
        {
            Debug.LogError("1");
            CurrentVelocity = Vector3.zero; 
            transform.position = TargetBasePos;
            return;
        }


        transform.position = nextPos;
    }

    float getVelocityFactorFunc(float x)
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
