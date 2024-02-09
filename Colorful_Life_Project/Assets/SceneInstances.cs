using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public CameraManager _cameraManager;
    public Vector3[] startPos;
    public Vector3[] cameraToPLayerOffSet;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _playerContext.gameObject.transform.position = startPos[scene.buildIndex];
        _cameraManager.CameraToPlayerOffSet = cameraToPLayerOffSet[scene.buildIndex];
    }

}
