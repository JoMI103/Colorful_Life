using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerContext;

public class SceneInstances : MonoBehaviour
{
    [SerializeField] private UI_Manager _uimanager;
    [SerializeField] private PlayerContext _playerContext;
    [SerializeField] private AudioManager _audioManger;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private RespawnManager _respawnManager;
    public UI_Manager Uimanager { get => _uimanager;}
    public PlayerContext PlayerContext { get => _playerContext; }
    public AudioManager AudioManger { get => _audioManger; }
    public DialogueManager DialogueManager { get => _dialogueManager;  }
    public RespawnManager RespawnManager { get => _respawnManager; }

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
        int index = scene.buildIndex;
        if (index < 0 || index >= startPos.Length) index = 0 ;
        _playerContext.CharacterController.enabled = false;
        _playerContext.gameObject.transform.position = startPos[index];
        _playerContext.CharacterController.enabled = true;
        _cameraManager.CameraToPlayerOffSet = cameraToPLayerOffSet[index];

        _playerContext.SceneLoaded();
    }

}
