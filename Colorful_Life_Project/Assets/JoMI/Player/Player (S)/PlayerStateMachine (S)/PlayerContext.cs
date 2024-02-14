using jomi.CharController3D;
using jomi.utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static PlayerInfo;

public class PlayerContext : MonoBehaviour, IHittable
{


    #region Variables

    [SerializeField] private Camera _mainCamera;

    private CharacterController _characterController;
    private PlayerInput.OnFootActions _onFoot;


    [SerializeField] private PlayerBaseStatsSO _playerBaseStats;
    [SerializeField] private HandBaseStatsSO _handBaseStats;

    private PlayerInfo _playerInfo;

    private bool _isBeingHitted;
    private Vector3 _hitDirection;

    private Vector3 _mousePosition;

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _appliedMovement;
    private bool _isMovementPressed;


    private bool _isAttackPressed;

    //jumping variables
    private bool _isJumpPressed;
    private bool _requireNewJumpPress;
    private float _jumpVelocity;
    private float _jumpGravity;


    private float _gravity = -9.8f;

    private float _spellDamageTimer;
    private const float SpellDmgCoolDown = 1;

    //hands Variables
    [SerializeField] private HandStateMachine _leftHand;
    [SerializeField] private HandStateMachine _rightHand;

    [SerializeField] private Animator _leftAnimator;
    [SerializeField] private Animator _rightAnimator;
    [SerializeField] private Animator _leftRightAnimator;

    private bool _interactPressed;
    private bool _requireNewInteractPress;


    private bool _isDashPressed;
    private bool _requireNewDashPress;

    private IGrabbable _currentIGrabbable;
    private IInteractable _currentIInteractable;


    private IGrabbable _grabbedObject;


    [SerializeField] private GameObject _despairParticleSystems;
    [SerializeField] private GameObject _KilledParticles;
    [SerializeField] private GameObject _bodyModel;

    #endregion

    #region Movement and Hands State Machines Variables, Getters and Setters

    //Movement state machine
    private Dictionary<MovementState, HierarchicalBaseState<MovementState>> _movementStates = new();
    private HierarchicalBaseState<MovementState> _currentMovementState;

    /// Getters and Setters
    public Dictionary<MovementState, HierarchicalBaseState<MovementState>> MovementStates { get => _movementStates; }
    public ref HierarchicalBaseState<MovementState> CurrentMovementStateRef { get => ref _currentMovementState; }
    public  HierarchicalBaseState<MovementState> CurrentMovementState { get =>  _currentMovementState; }

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
    public bool RequireNewJumpPress { get => _requireNewJumpPress; set => _requireNewJumpPress = value; }
    public float JumpVelocity { get => _jumpVelocity; }
    public float JumpGravity { get => _jumpGravity; }
    
    public float Gravity { get => _gravity; set => _gravity = value; }

    public PlayerBaseStatsSO PlayerBaseStats { get => _playerBaseStats;  }
    public HandBaseStatsSO HandBaseStats { get => _handBaseStats;  }
    public bool IsAttackPressed { get => _isAttackPressed; }

    public Vector3 MousePosition { get => _mousePosition; }


    public HandStateMachine LeftHand { get => _leftHand; }
    public HandStateMachine RightHand { get => _rightHand; }
    public Animator LeftAnimator { get => _leftAnimator; }
    public Animator RightAnimator { get => _rightAnimator; }
    public Animator LeftRightAnimator { get => _leftRightAnimator;}
    public IGrabbable CurrentIGrabbable { get => _currentIGrabbable; set => _currentIGrabbable = value; }
    public IGrabbable GrabbedObject { get => _grabbedObject; set => _grabbedObject = value; }

    internal PlayerInfo PlayerInfo { get => _playerInfo; set => _playerInfo = value; }
    public bool IsBeingHitted { get => _isBeingHitted; set => _isBeingHitted = value; }
    public Vector3 HitDirection { get => _hitDirection;  }
    public Camera GetMainCameraFromPlayerContext { get => _mainCamera; set => _mainCamera = value; }
    public bool InteractPressed { get => _interactPressed;  }
    public bool RequireNewInteractPress { get => _requireNewInteractPress; set => _requireNewInteractPress = value; }
    public IInteractable CurrentIInteractable { get => _currentIInteractable; set => _currentIInteractable = value; }
    public CharacterController CharacterController1 { get => _characterController; set => _characterController = value; }
    public bool RequireNewDashPress { get => _requireNewDashPress; set => _requireNewDashPress = value; }
    public bool IsDashPressed { get => _isDashPressed; set => _isDashPressed = value; }

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

        _onFoot.Interact.started += OnInteract;
        _onFoot.Interact.canceled += OnInteract;

        _onFoot.Dash.started += OnDash;
        _onFoot.Dash.canceled += OnDash;



        void SetMovementStateMachineStates()
        {
            _movementStates.Add(MovementState.Idle, new PlayerIdleState(MovementState.Idle, this));
            _movementStates.Add(MovementState.Walk, new PlayerWalkState(MovementState.Walk, this));
            _movementStates.Add(MovementState.Dash, new PlayerDashState(MovementState.Dash, this));
            _movementStates.Add(MovementState.Stop, new PlayerStopState(MovementState.Stop, this));
            _movementStates.Add(MovementState.KnockBack, new PlayerKnockbackState(MovementState.KnockBack, this));
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
            _handsGroupStates.Add(HandsGroupState.Grab, new HandsGroupGrabState(HandsGroupState.Grab, this));
            _handsGroupStates.Add(HandsGroupState.Carry, new HandsCarryState(HandsGroupState.Carry, this));
        }
        SetHandsStateMachineStates();
        _currentHandsGroupState = _handsGroupStates[HandsGroupState.Idle];
        
        void SetUpJumpVariables()
        {
            float timeToApex = _playerBaseStats.MaxJumpTime / 2;
            _jumpGravity = -2 * _playerBaseStats.MaxJumpHeight / Mathf.Pow(timeToApex, 2);
            _jumpVelocity = 2 * _playerBaseStats.MaxJumpHeight / timeToApex;
        }
        SetUpJumpVariables();
    }

    public void SceneLoaded()
    {
        Debug.Log("ENTRIE NESSA BUCETA");
        StopAllCoroutines();
        _playerInfo = new PlayerInfo(this, _playerBaseStats.MaxHP, Magic.None, _playerBaseStats.DespairMaxTime);
        _currentHandsGroupState = _handsGroupStates[HandsGroupState.Idle];
        _currentMovementState = _movementStates[MovementState.Grounded];
        StartCoroutine(AddDespair());
        _currentMovementState.EnterState();
        _currentHandsGroupState.EnterState();

        _despairParticleSystems.SetActive(false);
        _KilledParticles.SetActive(false);
        _bodyModel.SetActive(true);

    }


    IEnumerator AddDespair()
    {
        while (true)
        {
            _playerInfo.Despair += 0.0166f;
            yield return new WaitForSeconds(1);
        }
    }

    private void Update() {

        _mousePosition = Jaux.GetcurrentMousePosition(_mainCamera, transform.position.y) ;

        void HandleBodyRotation() {
            const float TAU = 2 * Mathf.PI;
            transform.rotation = Quaternion.LookRotation(
                Vector3.RotateTowards(transform.forward, 
                (_mousePosition - transform.position).normalized, 
                TAU * Time.deltaTime * _playerBaseStats.RotationSpeed, 0.0f));
        }
        HandleBodyRotation();


        _currentMovementState.UpdateStates();
        _currentHandsGroupState.UpdateState();

        if (_spellDamageTimer < 0 && _playerInfo.CurrentMagic != Magic.None) { 
            _spellDamageTimer = SpellDmgCoolDown; 
            _playerInfo.CurrentHP -= 1; 
        } else _spellDamageTimer -= Time.deltaTime;
        
    }

    private void FixedUpdate() {
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    public void Hit(GameObject hittedBy, Vector3 hitDirection, Vector3 inpactPosition, int damage)
    {
        if (_isBeingHitted || _currentMovementState.StateKey == MovementState.Stop) return;
        _isBeingHitted = true;
        _hitDirection = hitDirection;
        _currentMovementState = _currentMovementState.SwitchRootStateFromOutSide(MovementStates[MovementState.KnockBack]);
        _playerInfo.CurrentHP -= damage ;
        Invoke(nameof(EndImunity), PlayerBaseStats.HittedImunityTime);
    }

    private void EndImunity() => _isBeingHitted = false;

    public void Killed()
    {
        stop(true);
        Debug.LogWarning("Morreu");
        _despairParticleSystems.SetActive(false);

        _bodyModel.SetActive(false);
        _KilledParticles.SetActive(true);

        SceneInstances.Instance.RespawnManager.PlayerKilled();
        //death sequence
    }

  

    public IEnumerator StartDespairDeath()
    {
        stop(true);
        while (!_characterController.isGrounded)
        {
            yield return null;
        }
        _despairParticleSystems.SetActive(true);
        yield return new WaitForSeconds(3);
        Killed();
        yield return new WaitForSeconds(PlayerBaseStats.DespairAniDuration - 3);
    }

    public void stop(bool ok)
    {
        if(ok) _currentMovementState = _currentMovementState.SwitchRootStateFromOutSide(MovementStates[MovementState.Stop]);
        else _currentMovementState = _currentMovementState.SwitchRootStateFromOutSide(MovementStates[MovementState.Grounded]);
    }

    #region outSideMethods

    public bool AddSpell(Magic magic)
    {
        if (_playerInfo.CurrentMagic == Magic.None) { _spellDamageTimer = SpellDmgCoolDown;  _playerInfo.CurrentMagic = magic; return true; }
        return false;
    }

    #endregion

    #region Inputs

    void OnMovementInput(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>();

        Vector3 Input3D = new(_currentMovementInput.x, 0, _currentMovementInput.y);
        Quaternion lastRotation;
        lastRotation = _mainCamera.transform.rotation;

        _mainCamera.transform.forward = Vector3.Cross(Vector3.up, -_mainCamera.transform.right);
        Input3D = _mainCamera.transform.rotation * Input3D;
        _mainCamera.transform.rotation = lastRotation;
        Vector2 finalInput = new Vector2(Input3D.x, Input3D.z).normalized;
        _currentMovementInput = finalInput;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void OnJump(InputAction.CallbackContext context) {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    void OnAttack(InputAction.CallbackContext context) { 
        _isAttackPressed = context.ReadValueAsButton(); 
    }
    void OnInteract(InputAction.CallbackContext context)
    {
        _interactPressed = context.ReadValueAsButton();
        _requireNewInteractPress = false;
    }

    void OnDash(InputAction.CallbackContext context)
    {
        _isDashPressed = context.ReadValueAsButton();
        _requireNewDashPress = false;
    }



    #endregion

    #region StateEnums

    public enum MovementState
    {
        Grounded,
        Fall,
        Jumping,
        KnockBack,
        Stop,
            Idle,
            Walk,
            Dash,
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


public class PlayerInfo
{
    PlayerContext _ctx;
    Magic _currentMagic;
    int _currentHP;

    bool _depairHasStarted;
    float _maxDespair;
    float _currentDespair;
    public int CurrentHP
    {
        get => _currentHP; set
        {
            _currentHP = value; 
            SceneInstances.Instance.Uimanager.SetSlideLife(_currentHP);
            if (_currentHP <= 0) _ctx.Killed();
            Debug.Log("HP = " + _currentHP);
        }
    }
    public Magic CurrentMagic
    {
        get => _currentMagic; set
        {
            _currentMagic = value; 
            SceneInstances.Instance.Uimanager.UnlockAbilities(_currentMagic);
        }
    }
    public float Despair
    {
        get => _currentDespair; set
        {
            _currentDespair = value;
            if (_currentDespair >= _maxDespair)
            {
                if (!_depairHasStarted)
                {
                    _ctx.StartCoroutine(_ctx.StartDespairDeath());
                    _depairHasStarted = true;
                }
            }
            SceneInstances.Instance.Uimanager.SetSlideDespair(  _currentDespair / _maxDespair);
        }
    }

    public PlayerInfo(PlayerContext ctx,int MaxHp, Magic startMagic, float MaxDespair)
    {
        _depairHasStarted = false;
        _ctx = ctx;
        _currentHP = MaxHp;
        _currentDespair = 0;
        _currentMagic = startMagic;
        _maxDespair = MaxDespair;
        SceneInstances.Instance.Uimanager.SetSlideMaxLife(MaxHp);
        SceneInstances.Instance.Uimanager.UnlockAbilities(_currentMagic);
        SceneInstances.Instance.Uimanager.SetSlideMaxDespair(MaxDespair);
    }


    public enum Magic
    {
        Rage,
        Guilt,
        Sadness,
        None
    }
}