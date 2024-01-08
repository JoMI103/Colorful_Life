using jomi.CharController3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsStateMachine : MonoBehaviour
{
    [SerializeField] private BoxCollider grabbableArea;
    private MonoBehaviour _currentIGrabbable;
    MonoBehaviour _grabbedObject;
    bool _grabbed;
    private bool _isPressingAttackMode;
    private Vector3 _grabbedOffset;


    [SerializeField] private Hand leftHand, rightHand;
    private (Hand, Hand) _leftRightHands;
    [SerializeField] private Transform leftBaseTransform, righBaseTransform;
    private (Transform, Transform) _baseLeftRightTransform;
    private (Quaternion, Quaternion) _baseDefaultRotations;

    [SerializeField] private Transform leftGrabbleTransform, righGrabbleTransform;
    private (Transform, Transform) _grabbleLeftRightTransform;

    //State Machine Vars
    HandsBaseState _currentsState;
    HandsStateFactory _states;


    private PlayerInput.OnFootActions _onFoot;
    private PlayerLookMouse _playerLookMouse;
    [SerializeField] Transform attackTransform;

    //getters & setters
    public HandsBaseState CurrentsState {  get { return _currentsState; } set { _currentsState = value; } }
    public MonoBehaviour GrabbedObject { get { return _grabbedObject; } set { _grabbedObject = value; } }
    public (Transform, Transform) BaseLeftRightTransform { get => _baseLeftRightTransform; }
    public (Transform, Transform) GrabbleLeftRightTransform { get => _grabbleLeftRightTransform; set => _grabbleLeftRightTransform = value; }
    public (Hand, Hand) LeftRightHands { get => _leftRightHands; }
    public bool Grabbed { get => _grabbed; set => _grabbed = value; }
    public MonoBehaviour CurrentIGrabbable { get => _currentIGrabbable; }
    public (Quaternion, Quaternion) BaseDefaultRotations { get => _baseDefaultRotations;  }
    public bool IsPressingAttackMode { get => _isPressingAttackMode;  }
    public Vector3 GrabbedOffset { get => _grabbedOffset; set => _grabbedOffset = value; }

    private void Awake()
    {
        _playerLookMouse = GetComponent<PlayerLookMouse>();

        _leftRightHands = (leftHand, rightHand);
        _baseLeftRightTransform = (leftBaseTransform, righBaseTransform);
        _grabbleLeftRightTransform = (leftGrabbleTransform, righGrabbleTransform);
        _baseDefaultRotations = (_baseLeftRightTransform.Item1.rotation, _baseLeftRightTransform.Item2.rotation);
        
        _states = new HandsStateFactory(this); 
        _currentsState = _states.Body();
        _currentsState.EnterState();
      
     


    }
    private void Start()
    {

        _onFoot = GetComponent<InputManager>().onFoot;
        _onFoot.Attack.performed += ctx => CurrentsState.AttackAction(getAttackTransform());
        _onFoot.Interact.performed += ctx => CurrentsState.GrabAction();
        _onFoot.StateAction.started += OnStateAction;
        _onFoot.StateAction.canceled += OnStateAction;


    }

    Transform getAttackTransform()
    {
        //if selecting enemy return their transform
        //if controller dontknow
        attackTransform.position = _playerLookMouse.getcurrentMousePosition();
        attackTransform.forward = transform.forward; 
        return attackTransform;
    }

    void OnStateAction(InputAction.CallbackContext context)
    {
        _isPressingAttackMode = context.ReadValueAsButton();
    }


    void Update() {
        FindGrabbableObjects();


        _currentsState.UpdateState();
    }


    private void FindGrabbableObjects()
    {
        MonoBehaviour hitedGrabbable = null;

        foreach (Collider c in Physics.OverlapBox(transform.position + (transform.rotation * grabbableArea.center), grabbableArea.size)) 
            foreach (MonoBehaviour script in c.gameObject.GetComponentsInChildren<MonoBehaviour>())
                if (script is IGrabbable)
                    hitedGrabbable = script;

        if (hitedGrabbable != _currentIGrabbable) _currentIGrabbable = hitedGrabbable;
    }
}
