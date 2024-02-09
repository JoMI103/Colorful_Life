using UnityEngine;

public interface IInteractable {
    string PromptMessage { get; }
    GameObject PlayerGO { get; }
    GameObject InteractableGO { get; }

    void Interact(GameObject player);
}
