using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jomi.CharController3D {
    public class PlayerLook : MonoBehaviour {
        private PlayerInput.OnFootActions _onFoot;

        public Camera playerCamera;
        private float _xRotation;
        [SerializeField,Range(0,2)] private float _xSensitivity = 1.0f, _ySensitivity = 1.0f;

        private void Start() {
            _onFoot = GetComponent<InputManager>().OnFoot;
        }

        private void LateUpdate() {
            ProcessLook(_onFoot.Look.ReadValue<Vector2>());
        }

        public void ProcessLook(Vector2 input) {
            float mouseX = input.x;
            float mouseY = input.y;
            //calculate camera rotation
            _xRotation -= mouseY * _ySensitivity * 0.1f;
            _xRotation = Mathf.Clamp(_xRotation, -80.0f, 80.0f);
            //apply to camera
            playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            //rotate player to look left and right with the body
            transform.Rotate(Vector3.up * mouseX  * _xSensitivity * 0.1f);
        }
    }
}
