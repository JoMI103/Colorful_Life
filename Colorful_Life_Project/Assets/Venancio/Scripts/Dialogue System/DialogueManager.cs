using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject _dialogueObject;
    private Speech _currentSpeech;
    private TMPro.TextMeshPro _dialogueText;
    private TMPro.TextMeshPro _speakerName;
    private Speech _nextSpeech;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialogue(Speech speech, GameObject dialogueObject, TextMeshPro dialogueText, TextMeshPro speakerName)
    {
        if (speech == null)

        this._currentSpeech = speech;
        this._dialogueObject = dialogueObject;
        this._dialogueText = dialogueText;
        this._speakerName = speakerName;

        if (!dialogueObject.activeInHierarchy)
            dialogueObject.SetActive(true);

        this._nextSpeech = speech;

        NextDialogue();

    }

    public bool NextDialogue()
    {
        if (_nextSpeech != null)
        {
            _currentSpeech = _nextSpeech;
            _nextSpeech = _currentSpeech.NextSpeech;

            string nextSpeaker = _currentSpeech.Speaker;

            if (!(nextSpeaker.Equals(_speakerName.text)
                || nextSpeaker == null
                || nextSpeaker.Equals("")))
            {
                _speakerName.text = nextSpeaker;
            }

            _dialogueText.text = _currentSpeech.Words;

            return true;

        } else
        {
            EndDialogue();
            return false;
        }
    }

    public void EndDialogue()
    {
        _dialogueObject.SetActive(false);

        Debug.Log("TERMINOU");
    }

}
