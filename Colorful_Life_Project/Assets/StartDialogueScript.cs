using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogueScript : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueVisualObject;
    [SerializeField] private TMPro.TextMeshPro _dialogueText;
    [SerializeField] private TMPro.TextMeshPro _dialogueSpeaker;
    [SerializeField] public Speech Speech;

    public void StartDialogue ()
    {
        SceneInstances.Instance.DialogueManager.StartDialogue(Speech, _dialogueVisualObject, _dialogueText, _dialogueSpeaker);
    }

    public void EndDialogue ()
    {
        SceneInstances.Instance.DialogueManager.EndDialogue();
    }

}



