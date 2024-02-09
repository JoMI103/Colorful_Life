using System.Collections.Generic;
using UnityEngine;

namespace jomi.CharController3D {
    public class ThirdPersonInteract : MonoBehaviour
    {

        [SerializeField] private PlayerContext _playerContext;

        [SerializeField] private BoxCollider _grabbableArea;

        private PlayerInput.OnFootActions _onFoot;
        private IInteractable currentLookingInteractable;

        [SerializeField] Transform _coiso;


        void Start()
        {
            _onFoot = GetComponent<InputManager>().OnFoot;
            _onFoot.Interact.performed += ctx => Interact();
        }

        void Interact()
        {
            currentLookingInteractable?.Interact(this.gameObject);
        }

        void Update()
        {
            IInteractable hitedInteractable = null;

            _playerContext.CurrentIGrabbable = null;
            _playerContext.CurrentIInteractable = null;

            List<IInteractable> currentInteractables = new List<IInteractable>();
            List<IGrabbable> currentGrabbables = new List<IGrabbable>();


            foreach (Collider c in Physics.OverlapBox(transform.position + (transform.rotation * _grabbableArea.center), _grabbableArea.size))
                foreach (MonoBehaviour script in c.gameObject.GetComponentsInChildren<MonoBehaviour>())
                {
                    if (script is IGrabbable)
                        currentGrabbables.Add(script as IGrabbable);
                    if (script is IInteractable)
                        currentInteractables.Add(script as IInteractable);
                }

            float smallDistance = 100;
            IInteractable closestInteractable = null;
            IGrabbable closestGrabbable = null;

            foreach (var intt in currentInteractables)
            {
                float currentDistace = Vector3.Distance(intt.InteractableGO.transform.position, _playerContext.MousePosition);
                if (currentDistace < smallDistance)
                {
                    smallDistance = currentDistace;
                    closestInteractable = intt;
                }
            }

            foreach (var grab in currentGrabbables)
            {
                float currentDistace = Vector3.Distance(grab.GrabbableGO.transform.position, _playerContext.MousePosition);
                if (currentDistace < smallDistance)
                {
                    smallDistance = currentDistace;
                    closestInteractable = null;
                    closestGrabbable = grab;
                }
            }

            if (closestGrabbable != null) {
                _playerContext.CurrentIGrabbable = closestGrabbable;
                _coiso.transform.position = closestGrabbable.GrabbableGO.transform.position; 
            
            }
            if (closestInteractable != null) {
                _playerContext.CurrentIInteractable = closestInteractable;
                _coiso.transform.position = closestInteractable.InteractableGO.transform.position; 
            }
        }
    }
}