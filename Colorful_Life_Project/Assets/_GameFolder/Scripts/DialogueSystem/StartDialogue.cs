using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _dialogueVisualObject;
    [SerializeField] private TMPro.TextMeshPro _dialogueText;
    [SerializeField] private TMPro.TextMeshPro _dialogueSpeaker;
    [SerializeField] public Speech Speech;

    public string PromptMessage { get; set; }
    public GameObject PlayerGO { get; set; }

    public GameObject InteractableGO => this.gameObject;

    private bool _firstTime = true;

    public void Interact(GameObject player)
    {
        if (_firstTime)
        {
            PlayerGO = player;
            SceneInstances.Instance.DialogueManager.StartDialogue(Speech, _dialogueVisualObject, _dialogueText, _dialogueSpeaker);
            
            _firstTime = false;
        } else
        {
            if (!SceneInstances.Instance.DialogueManager.NextDialogue())
            {
                _firstTime = true;
            }
        }
        
    }

}