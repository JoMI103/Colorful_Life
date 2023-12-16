using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jomi.CharController3D {
    public class InputManager : MonoBehaviour {

        private PlayerInput playerInput;
        public PlayerInput.OnFootActions onFoot;

        private void Awake() {
            playerInput = new PlayerInput();
            onFoot = playerInput.OnFoot;
        }

        private void OnEnable() {
            onFoot.Enable();
            //Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable() {
            onFoot.Disable();
        }
    }
}