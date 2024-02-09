using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
//TO-DO: No audioGroupList, fazer uma lista de um objeto proprio, onde se possa colocar um float e uma string).
public class AudioManager : MonoBehaviour
{
    [Header("Background Musics Configuration")]
    [Space]

    [SerializeField]
    private AudioClipDictionaryPage[] m_backgroundMusicsData;
    private Dictionary<string, AudioClip> _backgroundMusics = new Dictionary<string, AudioClip>();
    [Range(0, 1)] [SerializeField]
    private float m_backgroundVolume;
    public float BackgroundVolume {
        get {
            return m_backgroundVolume;
        }

        set { 
            m_backgroundVolume = value;
            BackgroundMusicAudioSource.volume = value;
        } 
    }
    [SerializeField]
    private bool _playOnStart;
    [SerializeField]
    private string _backgroundMusicToPlay;

    [Space][Space][Space]

    [Header("Audio Clips Configuration")]
    [Space]

    [SerializeField]
    private AudioClipDictionaryPage[] m_audioClipsData;
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    
    [Space]

    [Range(0, 1)] [SerializeField]
    private float _GlobalSoundEffectsVolume;
    public float GlobalSoundEffectsVolume {
        get {
            return _GlobalSoundEffectsVolume;
        }

        set
        {
            _GlobalSoundEffectsVolume = value;
            UpdateSoundEffectsVolume();
        }
    }

    [Space]

    [SerializeField]
    private List<AudioSource> _activeAudioSourceList = new List<AudioSource>();
    private Dictionary<AudioSource, float> _orignalVolumeDictionary = new Dictionary<AudioSource, float>();

    [Space][Space][Space]

    [Header("Custom Audio Groups")]
    [Space]

    [SerializeField]
    [Serialize]
    public List<AudioGroup> AudioGroups = new List<AudioGroup>(); //Needs to have a private set
    private Dictionary<string, AudioGroup> _audioGroupDictionary = new Dictionary<string, AudioGroup>();


    [Space][Space][Space]

    [Header("General Configuration")]
    [Space]

    public AudioSource BackgroundMusicAudioSource;

    [Serializable]
    public struct AudioClipDictionaryPage
    {
        public string Name;
        public AudioClip AudioClip;
    }


    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get { return _instance; }
    }

    protected void SingletonSetup()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Awake()
    {
        SingletonSetup();
        MakeDictionaries();
    }

    private void UpdateSoundEffectsVolume()
    {
        foreach (AudioSource audioSource in _activeAudioSourceList)
        {
            audioSource.volume = _orignalVolumeDictionary[audioSource] * GlobalSoundEffectsVolume;
        }
    }

    private void MakeDictionaries()
    {
        foreach (AudioClipDictionaryPage backgroundMusic in m_backgroundMusicsData)
        {
            _backgroundMusics.Add(backgroundMusic.Name, backgroundMusic.AudioClip);
        }

        foreach (AudioClipDictionaryPage audioClip in m_audioClipsData)
        {
            _audioClips.Add(audioClip.Name, audioClip.AudioClip);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        CheckVariables();
        
        if (_orignalVolumeDictionary.Count > 0)
            UpdateSoundEffectsVolume();

        if (_playOnStart)
            PlayBackgroundMusic(_backgroundMusicToPlay);

        SceneManager.activeSceneChanged += ResetAudioSources;
        
    }

    private void ResetAudioSources(Scene current, Scene next)
    {
        _activeAudioSourceList = new List<AudioSource>();
        _orignalVolumeDictionary = new Dictionary<AudioSource, float>();

        foreach (AudioGroup audioGroup in AudioGroups)
        {
            audioGroup.List = new List<AudioSource>();
        }
}


    private void CheckVariables()
    {
        if (BackgroundMusicAudioSource == null)
        {
            BackgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (AudioGroups != null)
        {
            foreach (AudioGroup audioGroup in AudioGroups)
            {
                if (audioGroup.Name == null)
                    audioGroup.Name = "";

                if (audioGroup.List == null)
                    audioGroup.List = new List<AudioSource>();

                if (audioGroup.Volume < 0f)
                    audioGroup.Volume = 0f;
                else if (audioGroup.Volume > 1f) 
                    audioGroup.Volume = 1f;

                _audioGroupDictionary.Add(audioGroup.Name, audioGroup);
            }
        } else
        {
            AudioGroups = new List<AudioGroup>();
        }
    }

    public void PlayBackgroundMusic(string backgroundMusicName)
    {
        AudioClip audioClipToPlay = null;

        try
        {
            audioClipToPlay = _backgroundMusics[backgroundMusicName];

        } catch (KeyNotFoundException exception)
        {
            Debug.Log("AudioManager on PlayBackgroundMusic: no AudioClip with name [" + _backgroundMusicToPlay + "]. Exception: " + exception.Message);
            return;
        }

        BackgroundMusicAudioSource.volume = BackgroundVolume;
        BackgroundMusicAudioSource.clip = audioClipToPlay;

        if (!BackgroundMusicAudioSource.isPlaying)
            BackgroundMusicAudioSource.Play();
    }

    public void PlayingBackgroundMusic(bool reproduce)
    {
        if (reproduce)
        {
            BackgroundMusicAudioSource.Play();
        } else
        {
            BackgroundMusicAudioSource.Stop();
        }
    }

    public void Play(string audioClipName, GameObject targetObjectToPlayClip)
    {
        Debug.Log("BAGAS: " + GlobalSoundEffectsVolume);
        AudioClip audioClipToPlay = null;

        try
        {
            audioClipToPlay = _audioClips[audioClipName];
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log("AudioManager on Play: no AudioClip with name [" + audioClipName + "]. Exception: " + exception.Message);
            return;
        }

        AudioSource targetAudioSource = targetObjectToPlayClip.GetComponent<AudioSource>();

        if (targetAudioSource != null) {
            if (!_activeAudioSourceList.Contains(targetAudioSource))
            {
                _orignalVolumeDictionary.Add(targetAudioSource, targetAudioSource.volume);
                targetAudioSource.volume = targetAudioSource.volume * GlobalSoundEffectsVolume;
                _activeAudioSourceList.Add(targetAudioSource);
            }

        } else
        {
            targetAudioSource = targetObjectToPlayClip.AddComponent<AudioSource>();
            _orignalVolumeDictionary.Add(targetAudioSource, targetAudioSource.volume);
            targetAudioSource.volume = targetAudioSource.volume * GlobalSoundEffectsVolume;
            _activeAudioSourceList.Add(targetAudioSource);
        }

        Debug.Log(GlobalSoundEffectsVolume.ToString());
        
        targetAudioSource.clip = audioClipToPlay;
        targetAudioSource.Play();
    }

    private AudioGroup RetrieveAudioGroupByName(string audioGroupName, string methodThatCalled)
    {
        AudioGroup audioGroup = null;

        try
        {
            audioGroup = _audioGroupDictionary[audioGroupName];
        }
        catch (KeyNotFoundException exception)
        {
            audioGroup = new AudioGroup(audioGroupName, new List<AudioSource>(), 1f); //*
            _audioGroupDictionary.Add(audioGroupName, audioGroup);
            AudioGroups.Add(audioGroup);

            Debug.Log("AudioManager on " + methodThatCalled + ": no Audio Group with name [" + audioGroupName + "]. Creating a new one...");
        }

        return audioGroup;
    }

    public void Play(string audioClipName, GameObject targetObjectToPlayClip, string audioGroupName)
    {
        //Debug.Log("BAGAS: " + GlobalSoundEffectsVolume);
        AudioClip audioClipToPlay = null;

        try
        {
            audioClipToPlay = _audioClips[audioClipName];
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log("AudioManager on Play: no AudioClip with name [" + audioClipName + "]. Exception: " + exception.Message);
            return;
        }

        AudioSource targetAudioSource = targetObjectToPlayClip.GetComponent<AudioSource>();

        AudioGroup audioGroupList = RetrieveAudioGroupByName(audioGroupName, "Play");

        if (targetAudioSource != null)
        {
            if (!_activeAudioSourceList.Contains(targetAudioSource))
            {
                float defaultVolume = targetAudioSource.volume;
                _orignalVolumeDictionary.Add(targetAudioSource, defaultVolume);
                targetAudioSource.volume = (defaultVolume * audioGroupList.Volume) * GlobalSoundEffectsVolume;
                _activeAudioSourceList.Add(targetAudioSource);
                audioGroupList.List.Add(targetAudioSource);
            } else if (!audioGroupList.List.Contains(targetAudioSource))
            {
                targetAudioSource.volume = (_orignalVolumeDictionary[targetAudioSource] * audioGroupList.Volume) * GlobalSoundEffectsVolume;
                audioGroupList.List.Add(targetAudioSource);
            }

        }
        else
        {
            targetAudioSource = targetObjectToPlayClip.AddComponent<AudioSource>(); //*
            _orignalVolumeDictionary.Add(targetAudioSource, targetAudioSource.volume);
            targetAudioSource.volume = (targetAudioSource.volume * audioGroupList.Volume) * GlobalSoundEffectsVolume;
            _activeAudioSourceList.Add(targetAudioSource);
            audioGroupList.List.Add(targetAudioSource);
        }

        //Debug.Log(_orignalVolumeDictionary[targetAudioSource].ToString());

        targetAudioSource.clip = audioClipToPlay;
        targetAudioSource.Play();
    }

    public void ChangeVolume (float newVolume, string audioGroupName)
    {
        AudioGroup audioGroup = RetrieveAudioGroupByName(audioGroupName, "ChangeVolume");
        audioGroup.Volume = newVolume;

        if (audioGroup.List.Count > 0)
        {
            foreach (AudioSource audioSource in audioGroup.List)
            {
                audioSource.volume = (_orignalVolumeDictionary[audioSource] * newVolume) * GlobalSoundEffectsVolume;
            }
        }
    }

    private int i = 0;

    public void DebugThis(string args)
    {
        Debug.Log(AudioGroups[0].List[0].volume) ;
    }

    [MenuItem("GameObject/Managers/Audio Manager", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject audioManager = new GameObject("Audio Manager");
        audioManager.AddComponent<AudioManager>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(audioManager, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(audioManager, "Create " + audioManager.name);
        Selection.activeObject = audioManager;
    }

    
}
