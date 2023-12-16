using System.Collections;
using System.Collections.Generic;
using jomi.interactableSystem;
using UnityEngine;
using UnityEngine.LowLevel;

namespace jomi.CharController3D {
    public class ThirdPersonInteract : MonoBehaviour
    {
        private PlayerInput.OnFootActions _onFoot;
        private Interactable currentLookingInteractable;

        public LayerMask interactableMask;
        public float interactableDistance;


        void Start()
        {
            _onFoot = GetComponent<InputManager>().onFoot;
            _onFoot.Interact.performed += ctx => Interact();
        }

        void Interact()
        {
            currentLookingInteractable?.BaseInteract(this.gameObject);
        }

        void Update()
        {
            Interactable hitedInteractable = null;


            Ray ray = new(transform.position, transform.forward);

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