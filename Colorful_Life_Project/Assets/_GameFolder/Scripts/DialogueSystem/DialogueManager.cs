using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject _dialogueObject;
    private Speech _currentSpeech;
    private TMPro.TMP_Text _dialogueText;
    private TMPro.TMP_Text _speakerName;
    private Speech _nextSpeech;
    [SerializeField]
    private SpeechGameObjectDictionaryPage[] _speechGameObjectDictionaryPages;
    private Dictionary<string, SpeechGameObjectDictionaryPage> _speechGameObjectDictionary = new Dictionary<string, SpeechGameObjectDictionaryPage>();


    [Serializable]
    public class SpeechGameObjectDictionaryPage
    {
        public string Name;
        public GameObject DialogueObject;
        public TMPro.TMP_Text DialogueText;
        public TMPro.TMP_Text SpeakerName;
    }

    private void Awake()
    {
        InitializeSingleton();

        foreach (SpeechGameObjectDictionaryPage page in _speechGameObjectDictionaryPages)
        {
            _speechGameObjectDictionary.Add(page.Name, page);
        }
    }


    public void StartDialogue(Speech speech, GameObject dialogueObject, TMPro.TMP_Text dialogueText, TMPro.TMP_Text speakerName)
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
                _dialogueObject.SetActive(false);
                
                try  {
                    SpeechGameObjectDictionaryPage UIElementInfo = _speechGameObjectDictionary[_currentSpeech.GameObjectToChange];
                    _dialogueObject = UIElementInfo.DialogueObject;
                    _dialogueText = UIElementInfo.DialogueText;
                    _speakerName = UIElementInfo.SpeakerName;
                }
                catch (KeyNotFoundException excpt)
                {
                    Debug.Log(excpt);
                }

                
                _speakerName.text = _currentSpeech.Speaker;

                _dialogueObject.SetActive(true);
            }

            _dialogueText.text = _currentSpeech.Words;

            return true;

        } else
        {
            EndDialogue();
            return false;
        }
    }

    private void ChangeDialogueObject(string gameObjectToShow)
    {
        
    }

    public void EndDialogue()
    {
        _dialogueObject.SetActive(false);

        Debug.Log("TERMINOU");
    }

}
