using UnityEngine;

namespace jomi.CharController3D {
    public class ThirdPersonInteract : MonoBehaviour
    {
        private PlayerInput.OnFootActions _onFoot;
        private IInteractable currentLookingInteractable;

        public LayerMask interactableMask;
        public float interactableDistance;


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


            Ray ray = new(transform.position, transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, interactableDistance);
            if (hits.Length > 1)
            {
                foreach (RaycastHit hit in hits) {
                    MonoBehaviour[] allScripts = hit.collider.gameObject.GetComponentsInChildren<MonoBehaviour>();
                    for (int i = 0; i < allScripts.Length; i++)
                    {
                        if (allScripts[i] is IInteractable) {
                            hitedInteractable = allScripts[i] as IInteractable;
                            if (hitedInteractable != currentLookingInteractable)
                            {
                                currentLookingInteractable = hitedInteractable;
                            }
                            return;
                        }
                    }
                }
            }

            
        }
    }
}