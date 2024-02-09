using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _dialogueVisualObject;
    [SerializeField] private TMPro.TextMeshPro _dialogueText;
    [SerializeField] private TMPro.TextMeshPro _dialogueSpeaker;
    [SerializeField] private Speech _speech;

    public string PromptMessage { get; set; }
    public GameObject PlayerGO { get; set; }

    public GameObject InteractableGO => this.gameObject;

    private bool _firstTime = true;

    public void Interact(GameObject player)
    {
        if (_firstTime)
        {
            PlayerGO = player;
            DialogueManager.Instance.StartDialogue(_speech, _dialogueVisualObject, _dialogueText, _dialogueSpeaker);
            _firstTime = false;
        } else
        {
            if (!DialogueManager.Instance.NextDialogue())
            {
                _firstTime = true;
            }
        }
        
    }

}
