using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    string promptMessage { get; }
    GameObject PlayerGO { get; }
    void Interact(GameObject player);
}
