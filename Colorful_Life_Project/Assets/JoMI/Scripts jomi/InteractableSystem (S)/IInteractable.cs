using UnityEngine;

public interface IInteractable {
    string PromptMessage { get; }
    GameObject PlayerGO { get; }
    void Interact(GameObject player);
}
