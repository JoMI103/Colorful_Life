using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstances : MonoBehaviour
{
    [SerializeField] private UI_Manager _uimanager;
    [SerializeField] private PlayerContext _playerContext;
    [SerializeField] private AudioManager _audioManger;
    [SerializeField] private DialogueManager _dialogueManager;
    public UI_Manager Uimanager { get => _uimanager;}
    public PlayerContext PlayerContext { get => _playerContext; }
    public AudioManager AudioManger { get => _audioManger; }
    public DialogueManager DialogueManager { get => _dialogueManager;  }

    #region [Singleton]
    public static SceneInstances Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This object will not be destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
