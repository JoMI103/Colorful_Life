using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jomi.CharController3D {        
    public class PlayerMotor : MonoBehaviour {
        
        private CharacterController _controller;
        private PlayerInput.OnFootActions _onFoot;

        private Vector3 _playerVelocity;
        private bool _isGrounded;

        public float speed = 5.0f;
        public float gravity = -9.8f;
        public float jumpHeight = 3f;

        private bool _crouching, _lerpCrouch;
        private float _crouchTimer;

        private bool _sprinting;

        public bool followRotation;

        void Start() {
            _controller = GetComponent<CharacterController>();
            _onFoot = GetComponent<InputManager>().OnFoot;
            Debug.Log(_onFoot);
            _onFoot.Jump.performed += ctx => Jump();
            _onFoot.Crouch.performed += ctx => Crouch();
            _onFoot.Sprint.performed += ctx => Sprint();
        }

        private void FixedUpdate() {
            ProcessMove(_onFoot.Movement.ReadValue<Vector2>());
        }

        void Update() {
            _isGrounded = _controller.isGrounded;

            if (_lerpCrouch)
            {
                _crouchTimer += Time.deltaTime;
                float p = _crouchTimer / 1; p *= p;

                if(_crouching)
                    _controller.height = Mathf.Lerp(_controller.height, 1, p);
                else
                    _controller.height = Mathf.Lerp(_controller.height, 2, p);

                if(p > 1) {
                    _lerpCrouch = true;
                    _crouchTimer = 0f;
                }
            }
        }

        public void ProcessMove(Vector2 input) {
            //PlayerInputs
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            moveDirection = followRotation ? transform.TransformDirection(moveDirection) : moveDirection;
            _controller.Move(moveDirection * speed * Time.deltaTime);

            //Gravity
            _playerVelocity.y += gravity * Time.deltaTime;
            if(_isGrounded && _playerVelocity.y< 0) 
                _playerVelocity.y = -2;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        public void Jump() {
            if (!_isGrounded) return;
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        public void Crouch() {
            _crouching = !_crouching;
            _crouchTimer = 0;
            _lerpCrouch = true;
        }

        public void Sprint() {
            _sprinting = !_sprinting;
            if(_sprinting)  speed = 8f; else speed= 5f;
        }
    }
}

