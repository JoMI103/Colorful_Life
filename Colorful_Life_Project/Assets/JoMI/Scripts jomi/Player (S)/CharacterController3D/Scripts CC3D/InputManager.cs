using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jomi.CharController3D {
    public class InputManager : MonoBehaviour {

        private PlayerInput _playerInput;


        public PlayerInput.OnFootActions OnFoot {
            get { if (_playerInput == null) InitializePlayerInput();  return _playerInput.OnFoot; } }



        private void InitializePlayerInput()
        {
            _playerInput = new PlayerInput();
            _playerInput.OnFoot.Enable();
        }

        private void OnEnable() {
            _playerInput?.OnFoot.Enable();
            //Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable() {
            _playerInput?.OnFoot.Disable();
        }
    }
}