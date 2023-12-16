using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jomi.interactableSystem;

namespace jomi.CharController3D
{
    public class PlayerInteract : MonoBehaviour
    {

        private PlayerInput.OnFootActions _onFoot;
        private PlayerLook _playerLook;

        private Interactable currentLookingInteractable;

        public LayerMask interactableMask;
        public float interactableDistance;

        void Start()
        {
            _onFoot = GetComponent<InputManager>().onFoot;
            _onFoot.Interact.performed += ctx => Interact();
            _playerLook = GetComponent<PlayerLook>();
        }

        void Interact()
        {
            currentLookingInteractable?.BaseInteract(this.gameObject);
        }


        void Update()
        {
            Interactable hitedInteractable = null;


            Ray ray = new(_playerLook.playerCamera.transform.position, _playerLook.playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactableDistance, interactableMask))
            {
                hitedInteractable = hit.collider.GetComponent<Interactable>();

            }

            if (hitedInteractable != currentLookingInteractable)
            {
                currentLookingInteractable?.startLooking();
                hitedInteractable?.stopLooking();
                currentLookingInteractable = hitedInteractable;
            }
        }
    }
}