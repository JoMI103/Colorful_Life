
#if UNITY_EDITOR
using UnityEditor;

namespace jomi.interactableSystem {
    //True will affect child classes of interactable 
    [CustomEditor(typeof(Interactable), true)]
    public class InteractableEditor : Editor {
        public override void OnInspectorGUI() {
            Interactable interactable = target as Interactable;
            
            if(target.GetType() == typeof(EventOnlyInteractable)) {
                interactable.promptMessage = EditorGUILayout.TextField("Prompt Message", interactable.promptMessage);
                EditorGUILayout.HelpBox("EventOnlyInteract can only use unityevents.", MessageType.Info);
                if (interactable.GetComponent<InteractionEvent>() == null) {
                    interactable.useEvents = true;
                    interactable.gameObject.AddComponent<InteractionEvent>();
                }

                return;
            }


            base.OnInspectorGUI();

            if(interactable.useEvents) {
                if (interactable.GetComponent<InteractionEvent>() == null) 
                    interactable.gameObject.AddComponent<InteractionEvent>();
            }
            else {
                if (interactable.GetComponent<InteractionEvent>() != null)
                    DestroyImmediate( interactable.GetComponent<InteractionEvent>());
            }
        }
    }
}

#endif