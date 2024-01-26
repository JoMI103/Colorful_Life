using jomi.CharController3D;
using jomi.utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static HandStateMachine;

public class PlayerContext : MonoBehaviour {

    [SerializeField] Transform debugTransform;

    #region Variables

    [SerializeField] private Camera _mainCamera;

    private CharacterController _characterController;
    private PlayerInput.OnFootActions _onFoot;


    [SerializeField] private PlayerBaseStatsSO _playerBaseStats;
    [SerializeField] private HandBaseStatsSO _handBaseStats;

    private Vector3 _mousePosition;

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _appliedMovement;
    private bool _isMovementPressed;


    private bool _isAttackPressed;

    //jumping variables
    private bool _isJumpPressed;
    private bool _isJumping;
    private bool _requireNewJumpPress;
    private float _initialJumpVelocity;
    private int _jumpCount;
    Dictionary<int, float> _initialJumpVelocities = new();
    Dictionary<int, float> _jumpGravities = new();
    Coroutine _currentJumpResetRoutine = null;

    private float _gravity = -9.8f;

    //hands Variables

    [SerializeField] private HandStateMachine _leftHand;
    [SerializeField] private HandStateMachine _rightHand;

    [SerializeField] private Animator _leftAnimator;
    [SerializeField] private Animator _rightAnimator;
    [SerializeField] private Animator _leftRightAnimator;
    [SerializeField] private Transform _leftAnimatedTransform;
    [SerializeField] private Transform _rightAnimatedTransform;

    #endregion

    #region Movement and Hands State Machines Variables, Getters and Setters

    //Movement state machine
    private Dictionary<MovementState, HierarchicalBaseState<MovementState>> _movementStates = new();
    private HierarchicalBaseState<MovementState> _currentMovementState;

    /// Getters and Setters
    public Dictionary<MovementState, HierarchicalBaseState<MovementState>> MovementStates { get => _movementStates; }
    public ref HierarchicalBaseState<MovementState> CurrentMovementStateRef { get => ref _currentMovementState; }

    //Hands state machine
    private Dictionary<HandsGroupState, InnerBaseState<HandsGroupState>> _handsGroupStates = new();
    private InnerBaseState<HandsGroupState> _currentHandsGroupState;

    /// Getters and Setters
    public Dictionary<HandsGroupState, InnerBaseState<HandsGroupState>> HandsGroupStates { get => _handsGroupStates; }
    public ref InnerBaseState<HandsGroupState> CurrentHandsGroupStateRef { get => ref _currentHandsGroupState; }

    #endregion

    #region Getters and Setters

    public CharacterController CharacterController { get => _characterController; }

    public Vector2 CurrentMovementInput { get => _currentMovementInput; }
    public bool IsMovementPressed { get => _isMovementPressed; }
    public float CurrentMovementY { get => _currentMovement.y; set => _currentMovement.y = value; }
    public float AppliedMovementX { get => _appliedMovement.x; set => _appliedMovement.x = value; }
    public float AppliedMovementY { get => _appliedMovement.y; set => _appliedMovement.y = value; }
    public float AppliedMovementZ { get => _appliedMovement.z; set => _appliedMovement.z = value; }
    
    public bool IsJumpPressed { get => _isJumpPressed; }
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public bool RequireNewJumpPress { get => _requireNewJumpPress; set => _requireNewJumpPress = value; }
    public int JumpCount { get => _jumpCount; set => _jumpCount = value; }
    public Dictionary<int, float> InitialJumpVelocities { get => _initialJumpVelocities; }
    public Dictionary<int, float> JumpGravities { get => _jumpGravities;  }
    public Coroutine CurrentJumpResetRoutine { get => _currentJumpResetRoutine; set => _currentJumpResetRoutine = value; }
    
    public float Gravity { get => _gravity; set => _gravity = value; }

    public PlayerBaseStatsSO PlayerBaseStats { get => _playerBaseStats;  }
    public HandBaseStatsSO HandBaseStats { get => _handBaseStats;  }
    public bool IsAttackPressed { get => _isAttackPressed; }

    public Vector3 MousePosition { get => _mousePosition; }


    public HandStateMachine LeftHand { get => _leftHand; }
    public HandStateMachine RightHand { get => _rightHand; }
    public Animator LeftAnimator { get => _leftAnimator; }
    public Animator RightAnimator { get => _rightAnimator; }
    public Transform LeftAnimatedTransform { get => _leftAnimatedTransform; }
    public Transform RightAnimatedTransform { get => _rightAnimatedTransform;  }
    public Transform DebugTransform { get => debugTransform; set => debugTransform = value; }

    #endregion



    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _onFoot = GetComponent<InputManager>().OnFoot;

        _onFoot.Movement.started += OnMovementInput;
        _onFoot.Movement.performed += OnMovementInput;
        _onFoot.Movement.canceled += OnMovementInput;
        _onFoot.Jump.started += OnJump;
        _onFoot.Jump.canceled += OnJump;
        _onFoot.Attack.started += OnAttack;
        _onFoot.Attack.canceled += OnAttack;


        
        void SetMovementStateMachineStates()
        {
            _movementStates.Add(MovementState.Idle, new PlayerIdleState(MovementState.Idle, this));
            _movementStates.Add(MovementState.Walk, new PlayerWalkState(MovementState.Walk, this));
            _movementStates.Add(MovementState.Grounded, new PlayerGroundedState(MovementState.Grounded, this));
            _movementStates.Add(MovementState.Fall, new PlayerFallState(MovementState.Fall, this));
            _movementStates.Add(MovementState.Jumping, new PlayerJumpingState(MovementState.Jumping, this));
        }
        SetMovementStateMachineStates();
        _currentMovementState = _movementStates[MovementState.Grounded];
        
        void SetHandsStateMachineStates()
        {
            _handsGroupStates.Add(HandsGroupState.Idle, new HandsGroupIdleState(HandsGroupState.Idle, this));
            _handsGroupStates.Add(HandsGroupState.Attack, new HandsGroupAttackState(HandsGroupState.Attack, this));
            _handsGroupStates.Add(HandsGroupState.SpellCast, new HandsGroupSpellCastState(HandsGroupState.SpellCast, this));
            //_movementStates.Add(HandsState.Carry, new PlayerFallState(HandsState.Carry, this));
            //_movementStates.Add(HandsState.SpellCast, new PlayerJumpingState(HandsState.SpellCast, this));
        }
        SetHandsStateMachineStates();
        _currentHandsGroupState = _handsGroupStates[HandsGroupState.Idle];
        
        void SetUpJumpVariables()
        {
            float timeToApex = _playerBaseStats.MaxJumpTime / 2;
            float initialGRavity = -2 * _playerBaseStats.MaxJumpHeight / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * _playerBaseStats.MaxJumpHeight / timeToApex;
            float secondJumpGravity = -2 * (_playerBaseStats.MaxJumpHeight + 2) / Mathf.Pow(timeToApex * 1.25f, 2);
            float secondJumpInitialVelocity = 2 * (_playerBaseStats.MaxJumpHeight + 2) / (timeToApex * 1.25f);
            float thirdJumpGravity = -2 * (_playerBaseStats.MaxJumpHeight + 4) / Mathf.Pow(timeToApex * 1.5f, 2);
            float thirdJumpInitialVelocity = 2 * (_playerBaseStats.MaxJumpHeight + 4) / (timeToApex * 1.5f);

            _initialJumpVelocities.Add(1, _initialJumpVelocity);
            _initialJumpVelocities.Add(2, secondJumpInitialVelocity);
            _initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

            _jumpGravities.Add(0, initialGRavity);
            _jumpGravities.Add(1, initialGRavity);
            _jumpGravities.Add(2, secondJumpGravity);
            _jumpGravities.Add(3, thirdJumpGravity);
        }
        SetUpJumpVariables();
    }

    private void Start() {
        _currentMovementState.EnterState();
        _currentHandsGroupState.EnterState();
    }

    private void Update() {

        void SetHandsBaseTarget()
        {
            Vector3 leftPos, rightPos;
            Vector3 offset = HandBaseStats.BodyHandOffSet;
            rightPos = transform.rotation * offset + transform.position;
            leftPos = transform.rotation * new Vector3(-offset.x, offset.y, offset.z) + transform.position;
            LeftHand.SetBaseTarget(leftPos, HandBaseStats.BodyHandRotation);
            RightHand.SetBaseTarget(rightPos, HandBaseStats.BodyHandRotation);
        }
        SetHandsBaseTarget();

        _currentMovementState.UpdateStates();
        _currentHandsGroupState.UpdateState();
        _mousePosition = Jaux.GetcurrentMousePosition(_mainCamera, transform.position.y);

        void HandleRotation()
        {
            const float TAU = 2 * Mathf.PI;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (_mousePosition - transform.position).normalized, TAU * Time.deltaTime * _playerBaseStats.RotationSpeed, 0.0f));
        }
        HandleRotation();
    }

    

    private void FixedUpdate() {
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

   

    #region Inputs

    void OnMovementInput(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void OnJump(InputAction.CallbackContext context) {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    void OnAttack(InputAction.CallbackContext context) { 
        _isAttackPressed = context.ReadValueAsButton(); 
    }

    #endregion

    #region StateEnums

    public enum MovementState
    {
        Grounded,
        Fall,
        Jumping,
            Idle,
            Walk,
            Dash,
            Hitted, // falta adicionar
    }

    public enum HandsGroupState
    {
        Idle,
        Attack,
        Grab,
        Carry,
        SpellCast,
    }
    #endregion
}
